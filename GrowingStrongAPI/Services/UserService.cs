using System;
using System.Linq;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Helpers.Extensions;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace GrowingStrongAPI.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IFoodEntryRepository _foodEntryRepository;
        private IMapper _mapper;
        private readonly ILogger _logger;
        private IAuthenticationHelper _authenticationHelper;
        private IJwtHelper _jwtHelper;
        private IUserProfileCalculator _userProfileCalculator;

        public UserService(IUserRepository userRepository,
                           IFoodEntryRepository foodEntryRepository,
                           IMapper mapper,
                           ILogger<IUserService> logger,
                           IAuthenticationHelper authenticationHelper,
                           IJwtHelper jwtHelper,
                           IUserProfileCalculator userProfileCalculator)
        {
            _userRepository = userRepository;
            _foodEntryRepository = foodEntryRepository;
            _mapper = mapper;
            _logger = logger;
            _authenticationHelper = authenticationHelper;
            _jwtHelper = jwtHelper;
            _userProfileCalculator = userProfileCalculator;
        }

        public AuthenticateUserResponse Authenticate(string emailAddress, string password)
        {
            AuthenticateUserResponse response = new AuthenticateUserResponse();

            _logger.LogInformation($"Authenticating user with email: {emailAddress}");

            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.BAD_REQUEST,
                                                 Constants.AuthenticateUserMessages.InvalidCredentials);
                return response;
            }

            User user = null;

            try
            {
                user = _userRepository.GetByEmailAddress(emailAddress);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.SharedErrorMessages.FailedToRetrieveUser);
                return response;
            }

            if (user is null)
            {
                response.ResponseStatus.SetError(ResponseStatusCode.UNAUTHORIZED,
                                                 Constants.AuthenticateUserMessages.InvalidCredentials);
                return response;
            }

            try
            {
                bool passwordIsCorrect = _authenticationHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

                if (!passwordIsCorrect)
                {
                    response.ResponseStatus.SetError(ResponseStatusCode.UNAUTHORIZED,
                                                     Constants.AuthenticateUserMessages.InvalidCredentials);
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.AuthenticateUserMessages.InvalidPasswordHashOrSaltLength);
                return response;
            }

            string tokenString = _jwtHelper.GenerateJWT(user.UserId, ConfigurationsHelper.JWTSecret);

            if (string.IsNullOrEmpty(tokenString))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.AuthenticateUserMessages.FailedToGenerateJWT);
                return response;
            }

            _logger.LogInformation("Successfully authenticated user");

            UserDto userDto = _mapper.Map<UserDto>(user);

            response.ResponseStatus.SetOk();
            response.UserDto = userDto;
            response.Token = tokenString;

            return response;
        }

        public GetUserByIdResponse GetUserById(int id)
        {
            GetUserByIdResponse response = new GetUserByIdResponse();

            _logger.LogInformation($"Getting user by id: {id}");

            User user = null;

            try
            {
                user = _userRepository.GetById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.SharedErrorMessages.FailedToRetrieveUser);

                return response;
            }

            if (user is null)
            {
                response.ResponseStatus.SetError(ResponseStatusCode.NOT_FOUND,
                                                 Constants.SharedErrorMessages.UserDoesNotExist);

                return response;
            }

            UserDto userDto = _mapper.Map<UserDto>(user);

            response.ResponseStatus.SetOk();
            response.UserDto = userDto;

            return response;
        }

        public RegisterUserResponse Register(RegistrationModel registrationModel)
        {
            RegisterUserResponse response = new RegisterUserResponse();

            if (string.IsNullOrEmpty(registrationModel.EmailAddress) || string.IsNullOrEmpty(registrationModel.Password))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.BAD_REQUEST,
                                                 Constants.RegisterUserMessages.NullOrEmptyCredentials);
                return response;
            }

            User retrievedUser = null;

            try
            {
               retrievedUser = _userRepository.GetByEmailAddress(registrationModel.EmailAddress);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                 Constants.SharedErrorMessages.FailedToRetrieveUser);
            }

            if (!(retrievedUser is null))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.CONFLICT,
                                                 Constants.RegisterUserMessages.UserAlreadyExists);
                return response;
            }

            try
            {
                byte[] passwordHash, passwordSalt;
                _authenticationHelper.CreatePasswordHash(registrationModel.Password, out passwordHash, out passwordSalt);
                registrationModel.PasswordHash = passwordHash;
                registrationModel.PasswordSalt = passwordSalt;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.RegisterUserMessages.FailedToCreatePasswordHash);
                return response;
            }

            double bmr = _userProfileCalculator.CalculateBMR(registrationModel.Sex,
                                                             registrationModel.Weight,
                                                             registrationModel.Height,
                                                             registrationModel.BirthDate.Age());

            if (bmr == 0)
            {
                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.RegisterUserMessages.CalculatedBMRIsInvalid);

                return response;
            }

            registrationModel.Bmr = bmr;

            double tdee = _userProfileCalculator.CalculateTDEE(registrationModel.Bmr,
                                                               registrationModel.ActivityLevel);

            if (tdee == 0)
            {
                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.RegisterUserMessages.CalculatedTDEEIsInvalid);

                return response;
            }

            registrationModel.Tdee = tdee;

            try
            {
                int userId = _userRepository.Register(registrationModel);

                UserDto userDto = _mapper.Map<UserDto>(registrationModel);
                userDto.UserId = userId;

                userDto.Profile = _mapper.Map<UserProfileDto>(registrationModel);
                userDto.Targets = _mapper.Map<UserTargetsDto>(registrationModel);


                _logger.LogInformation("Successfully registered user");
                response.ResponseStatus.SetOk();
                response.userDto = userDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.RegisterUserMessages.FailedToCreateUser);
            }
            
            return response;
        }

        public GetUserFoodEntriesResponse GetUserFoodEntries(int userId)
        {

            GetUserFoodEntriesResponse response = new GetUserFoodEntriesResponse();

            User user = null;

            try
            {
                user = _userRepository.GetById(userId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.CONFLICT,
                                 Constants.SharedErrorMessages.FailedToRetrieveUser);
            }

            if (user is null)
            {
                response.ResponseStatus.SetError(ResponseStatusCode.NOT_FOUND,
                                                 Constants.SharedErrorMessages.UserDoesNotExist);

                return response;
            }

            try
            {
                List<FoodEntry> foodEntries = _foodEntryRepository.GetFoodEntriesOfUser(userId);
                List<FoodEntryDto> foodEntryDtos = _mapper.Map<List<FoodEntryDto>>(foodEntries);

                response.ResponseStatus.SetOk();
                response.FoodEntryDtos = foodEntryDtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.GetUserFoodEntriesMessages.FailedToRetrieveFoodEntry);
            }

            return response;
        }

        public UpdateUserDetailsResponse UpdateUserDetails(UserDetailsUpdateModel updateModel)
        {
            UpdateUserDetailsResponse response = new UpdateUserDetailsResponse();

            //TODO: Handle with non existent user id

            try
            {
                _userRepository.UpdateUserDetails(updateModel);

                response.ResponseStatus.SetOk();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.UpdateFoodEntryMessages.FailedToUpdateFoodEntry);
            }

            return response;
        }
    }
}

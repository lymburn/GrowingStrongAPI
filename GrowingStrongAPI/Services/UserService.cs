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

        public UserService(IUserRepository userRepository,
                           IFoodEntryRepository foodEntryRepository,
                           IMapper mapper,
                           ILogger<IUserService> logger,
                           IAuthenticationHelper authenticationHelper,
                           IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _foodEntryRepository = foodEntryRepository;
            _mapper = mapper;
            _logger = logger;
            _authenticationHelper = authenticationHelper;
            _jwtHelper = jwtHelper;
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
            
            User user = _userRepository.GetByEmailAddress(emailAddress);

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
            catch (Exception)
            {
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

            response.ResponseStatus.SetOk(Constants.AuthenticateUserMessages.Success);
            response.UserDto = userDto;
            response.Token = tokenString;

            return response;
        }

        public IEnumerable<User> GetAll()
        {
            _logger.LogInformation("Getting all users");

            return _userRepository.GetAll();
        }

        public UserDto GetById(int id)
        {
            _logger.LogInformation($"Getting user by id: {id}");

            User user = _userRepository.GetById(id);
            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public CreateUserResponse Create(User user, string password)
        {
            CreateUserResponse response = new CreateUserResponse();

            if (string.IsNullOrEmpty(user.EmailAddress) || string.IsNullOrEmpty(password))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.BAD_REQUEST,
                                                 Constants.CreateUserMessages.NullOrEmptyCredentials);
                return response;
            }

            if (!(_userRepository.GetByEmailAddress(user.EmailAddress) is null))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.CONFLICT,
                                                 Constants.CreateUserMessages.UserAlreadyExists);
                return response;
            }

            try
            {
                byte[] passwordHash, passwordSalt;
                _authenticationHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.CreateUserMessages.FailedToCreatePasswordHash);
                return response;
            }

            try
            {
                int userId = _userRepository.Create(user);
                user.UserId = userId;

                _logger.LogInformation("Successfully created user");

                UserDto userDto = _mapper.Map<UserDto>(user);
                response.ResponseStatus.SetOk(Constants.CreateUserMessages.Success);
                response.userDto = userDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.CreateUserMessages.FailedToCreateUser);
            }
            
            return response;
        }

        public IList<FoodEntryDto> GetUserFoodEntries(int userId)
        {
            List<FoodEntry> foodEntries = _foodEntryRepository.GetFoodEntriesOfUser(userId);
            List<FoodEntryDto> foodEntryDtos = _mapper.Map<List<FoodEntryDto>>(foodEntries);

            return foodEntryDtos;
        }
    }
}

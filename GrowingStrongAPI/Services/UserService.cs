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
        private IMapper _mapper;
        private readonly ILogger _logger;
        private IAuthenticationHelper _authenticationHelper;
        private IJwtHelper _jwtHelper;

        public UserService(IUserRepository userRepository,
                           IMapper mapper,
                           ILogger<IUserService> logger,
                           IAuthenticationHelper authenticationHelper,
                           IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _authenticationHelper = authenticationHelper;
            _jwtHelper = jwtHelper;
        }

        public AuthenticateUserResponse Authenticate(string emailAddress, string password)
        {
            AuthenticateUserResponse response = new AuthenticateUserResponse();
            response.ResponseStatus.SetOk();

            _logger.LogInformation($"Authenticating user with email: {emailAddress}");

            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
            {
                response.ResponseStatus.SetError(Constants.AuthenticateUserMessages.InvalidCredentials);
                return response;
            }
            
            User user = _userRepository.GetByEmailAddress(emailAddress);

            if (user is null)
            {
                response.ResponseStatus.SetError(Constants.AuthenticateUserMessages.InvalidCredentials);
                return response;
            }

            try
            {
                bool passwordCorrect = _authenticationHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

                if (!passwordCorrect)
                {
                    response.ResponseStatus.SetError(Constants.AuthenticateUserMessages.InvalidCredentials);
                    return response;
                }
            }
            catch (Exception e)
            {
                response.ResponseStatus.SetError(e.ToString());
                return response;
            }

            string tokenString = _jwtHelper.GenerateJWT(user.Id, ConfigurationsHelper.JWTSecret);

            if (string.IsNullOrEmpty(tokenString))
            {
                response.ResponseStatus.SetError(Constants.AuthenticateUserMessages.FailedToGenerateJWT);
                return response;
            }

            _logger.LogInformation("Successfully authenticated user");

            response.ResponseStatus.SetOk(Constants.AuthenticateUserMessages.Success);
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

        public void Create(User user, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Password cannot be null or empty");
                }

                if (!(_userRepository.GetByEmailAddress(user.EmailAddress) is null))
                {
                    _logger.LogInformation("Email address already exists");
                    return;
                }

                byte[] passwordHash, passwordSalt;

                _authenticationHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;

                user.PasswordSalt = passwordSalt;

                _userRepository.Create(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

        }
    }
}

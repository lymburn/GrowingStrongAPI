using System;
using System.Linq;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Helpers;
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

        public UserService(IUserRepository userRepository,
                           IMapper mapper,
                           ILogger<IUserService> logger,
                           IAuthenticationHelper authenticationHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _authenticationHelper = authenticationHelper;
        }

        public UserDto Authenticate(string emailAddress, string password)
        {
            _logger.LogInformation($"Authenticating user with email: {emailAddress}");

            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
            {
                _logger.LogError("Email address or password is null/empty");
                return null;
            }
                
            User user = _userRepository.GetByEmailAddress(emailAddress);

            if (user is null)
            {
                _logger.LogError($"Failed to retrieve user with email address: {emailAddress}");
                return null;
            }

            if (!_authenticationHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogError("Failed to verify user password");
                return null;
            }

            _logger.LogInformation("Successfully authenticated user");

            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
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

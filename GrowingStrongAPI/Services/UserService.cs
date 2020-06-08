using System;
using System.Linq;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Models;
using AutoMapper;

namespace GrowingStrongAPI.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService(IUserRepository userRepository,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public UserDto Authenticate(string emailAddress, string password)
        {
            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
            {
                return null;
            }
                
            User user = _userRepository.GetByEmailAddress(emailAddress);

            if (user is null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public UserDto GetById(int id)
        {
            User user = _userRepository.GetById(id);
            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public void Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }

            if (!(_userRepository.GetByEmailAddress(user.EmailAddress) is null))
            {
                Console.WriteLine("Email address already exists");
                return;
            }

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;

            user.PasswordSalt = passwordSalt;

            _userRepository.Create(user);
        }

        //Helpers
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password is null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password value cannot be empty or whitesapce only");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;

                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password is null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password value cannot be empty or whitesapce only");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).");
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

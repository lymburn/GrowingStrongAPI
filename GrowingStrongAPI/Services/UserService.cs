using System;
using System.Linq;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.DataAccess;

namespace GrowingStrongAPI.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User Create(User user, string password)
        {
            if (!(_userRepository.GetByUsername(user.Username) is null))
            {
                Console.WriteLine("Username already exists");
                return null;
            }

            _userRepository.Create(user);

            return null;
        }
    }
}

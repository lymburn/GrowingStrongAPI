using System;
using System.Collections.Generic;
using System.Linq;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.DataAccess;

namespace GrowingStrongAPI.Services
{
    public class UserService : IUserService
    {
        private IUserDataAccess _userDataAccess;

        public UserService(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        public User Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            return _userDataAccess.FindUserById(id).FirstOrDefault();
        }

        public User Create(User user, string password)
        {
            if (_userDataAccess.FindUserByUsername(user.Username).Any())
            {
                Console.WriteLine("Username already exists");
                return null;
            }

            _userDataAccess.InsertUser(user);

            return null;
        }
    }
}

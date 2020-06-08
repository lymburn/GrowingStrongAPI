using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.Services
{
    public interface IUserService
    {
        UserDto Authenticate(string emailAddress, string password);
        IEnumerable<User> GetAll();
        UserDto GetById(int id);
        void Create(User user, string password);
    }
}

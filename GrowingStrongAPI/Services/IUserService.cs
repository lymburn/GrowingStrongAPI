using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
    }
}

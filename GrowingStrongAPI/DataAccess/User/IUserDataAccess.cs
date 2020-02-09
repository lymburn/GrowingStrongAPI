using System;
using GrowingStrongAPI.Models;
using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IUserDataAccess
    {
        void InsertUser(User user);
        List<User> FindUserById(int id);
        List<User> FindUserByUsername(string username);
    }
}

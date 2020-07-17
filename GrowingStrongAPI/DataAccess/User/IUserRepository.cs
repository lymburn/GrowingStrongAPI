using System;
using GrowingStrongAPI.Entities;
using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByEmailAddress(string emailAddress);
        int Create(User user);
    }
}

using System;
using GrowingStrongAPI.Entities;
using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
        IEnumerable<User> GetAll();
    }
}

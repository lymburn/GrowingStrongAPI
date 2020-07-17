using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.Services
{
    public interface IUserService
    {
        AuthenticateUserResponse Authenticate(string emailAddress, string password);
        UserDto GetById(int id);
        CreateUserResponse Create(User user, string password);
        public GetUserFoodEntriesResponse GetUserFoodEntries(int userId);
    }
}

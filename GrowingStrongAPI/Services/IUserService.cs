using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.Services
{
    public interface IUserService
    {
        AuthenticateUserResponse Authenticate(string emailAddress, string password);
        GetUserByIdResponse GetUserById(int id);
        CreateUserResponse Create(User user, string password);
        GetUserFoodEntriesResponse GetUserFoodEntries(int userId);
    }
}

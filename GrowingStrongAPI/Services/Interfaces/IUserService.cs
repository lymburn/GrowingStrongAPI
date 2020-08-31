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
        RegisterUserResponse Register(RegistrationModel registrationModel);
        GetUserFoodEntriesResponse GetUserFoodEntries(int userId);
        UpdateUserDetailsResponse UpdateUserDetails(UserDetailsUpdateModel updateModel);
        UpdateUserProfileResponse UpdateUserProfile(UserProfile profile);
        UpdateUserTargetsResponse UpdateUserTargets(UserTargets targets);
    }
}

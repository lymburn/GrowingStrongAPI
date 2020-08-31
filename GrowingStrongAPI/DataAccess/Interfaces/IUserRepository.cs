using System;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Models;

using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByEmailAddress(string emailAddress);
        int Register(RegistrationModel registrationModel);
        void UpdateUserDetails(UserDetailsUpdateModel updateModel);
        void UpdateUserProfile(UserProfile profile);
        void UpdateUserTargets(UserTargets targets);
    }
}

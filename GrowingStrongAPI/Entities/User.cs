using System;
using System.Collections.Generic;

namespace GrowingStrongAPI.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public IList<FoodEntry> FoodEntries { get; set; }
        public UserProfile UserProfile { get; set; }
        public UserTargets UserTargets { get; set; }
    }
}

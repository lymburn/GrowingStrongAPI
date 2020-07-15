﻿using System;
namespace GrowingStrongAPI.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

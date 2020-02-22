using System;
namespace GrowingStrongAPI.Entities
{
    public class User : EntityBase
    {
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

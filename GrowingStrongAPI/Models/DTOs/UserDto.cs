using System;
using GrowingStrongAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GrowingStrongAPI.Models
{
    public class UserDto
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public int UserId { get; set; }

        public UserProfileDto Profile { get; set; }

        public UserTargetsDto Targets { get; set; }
    }
}

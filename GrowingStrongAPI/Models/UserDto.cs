using System;
using System.ComponentModel.DataAnnotations;

namespace GrowingStrongAPI.Models
{
    public class UserDto
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public int Id { get; set; }
    }
}

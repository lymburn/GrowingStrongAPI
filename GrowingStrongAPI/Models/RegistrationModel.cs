using System;
using System.ComponentModel.DataAnnotations;

namespace GrowingStrongAPI.Models
{
    public class RegistrationModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public string ActivityLevel { get; set; }

        [Required]
        public string WeightGoalTimeline { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public double Bmr { get; set; }

        public double Tdee { get; set; }
    }
}

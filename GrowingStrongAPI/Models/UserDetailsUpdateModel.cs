using System;
namespace GrowingStrongAPI.Models
{
    public class UserDetailsUpdateModel
    {
        public int UserId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double Bmr { get; set; }
        public string ActivityLevel { get; set; }
        public double Tdee { get; set; }
        public double GoalWeight { get; set; }
        public string WeightGoalTimeline { get; set; }
    }
}

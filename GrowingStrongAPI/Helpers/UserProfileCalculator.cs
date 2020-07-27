using System;
namespace GrowingStrongAPI.Helpers
{
    public class UserProfileCalculator : IUserProfileCalculator
    {
        //Using Harris-Benedict Equation
        public double CalculateBMR(string sex, double weight, double height, int age)
        {
            double bmr = 0;

            if (sex.Equals(Constants.Sexes.Male))
            {
                bmr = 88.362 + (13.397 * weight) + (4.799 * height) - (5.677 * age);
            }
            else
            {
                bmr = 447.593 + (9.247 * weight) + (3.098 * height) - (4.330 * age);
            }

            return bmr;
        }

        public double CalculateTDEE(double bmr, string activityLevel)
        {
            double tdee = 0;

            switch (activityLevel)
            {
                case Constants.ActivityLevels.Sedentary:
                    tdee = 1.2 * bmr;
                    break;
                case Constants.ActivityLevels.Light:
                    tdee = 1.375 * bmr;
                    break;
                case Constants.ActivityLevels.Moderate:
                    tdee = 1.55 * bmr;
                    break;
                case Constants.ActivityLevels.Extreme:
                    tdee = 1.725 * bmr;
                    break;
            }

            return tdee;
        }
    }
}

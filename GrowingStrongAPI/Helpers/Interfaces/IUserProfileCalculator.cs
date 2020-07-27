using System;
namespace GrowingStrongAPI.Helpers
{
    public interface IUserProfileCalculator
    {
        public double CalculateBMR(string sex, double weight, double height, int age);

        public double CalculateTDEE(double bmr, string activityLevel);
    }
}

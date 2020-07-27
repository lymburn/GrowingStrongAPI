using System;
namespace GrowingStrongAPI.Helpers
{
    public static class Constants
    {
        public static class SharedErrorMessages
        {
            public const string UserDoesNotExist = "Unable to find user with this id";
            public const string FailedToRetrieveUser = "Error occurred trying to retrieve user";
            public const string FoodEntryDoesNotExist = "Unable to find food entry with this id";
        }

        public static class AuthenticationHelperExceptions
        {
            public const string InvalidPasswordHashLength = "Invalid length of password hash (64 bytes expected)";
            public const string InvalidPasswordSaltLength = "Invalid length of password salt (128 bytes expected)";
        }

        public static class AuthenticateUserMessages
        {
            public const string InvalidCredentials = "Invalid email address or password";
            public const string InvalidPasswordHashOrSaltLength = "Invalid password hash or salt length";
            public const string FailedToGenerateJWT = "Failed to generate JWT";
        }

        public static class RegisterUserMessages
        {
            public const string NullOrEmptyCredentials = "Email or password is null or empty";
            public const string UserAlreadyExists = "User with this email already exists";
            public const string FailedToCreatePasswordHash = "Failed to create password hash";
            public const string FailedToCreateUser = "Failed to create user";
            public const string CalculatedBMRIsInvalid = "Calculated invalid BMR";
            public const string CalculatedTDEEIsInvalid = "Calculated invalid TDEE";
        }

        public static class GetUserFoodEntriesMessages
        {
            public const string FailedToRetrieveFoodEntry = "Failed to retrieve food entries for this user";
        }

        public static class CreateFoodEntryMessages
        {
            public const string FailedToCreateFoodEntry = "Failed to create food entry";
        }

        public static class UpdateFoodEntryMessages
        {
            public const string FailedToUpdateFoodEntry = "Failed to update food entry";
        }

        public static class DeleteFoodEntryMessages
        {
            public const string FailedToDeleteFoodEntry = "Failed to delete food entry";
        }

        public static class GetFoodsStartingWithPatternMessages
        {
            public const string InvalidQuery = "Invalid query text";
            public const string FailedToGetFoods = "Failed to get foods";
        }

        public static class ActivityLevels
        {
            public const string Sedentary = "Sedentary";
            public const string Light = "Light";
            public const string Moderate = "Moderate";
            public const string Extreme = "Extreme";
        }

        public static class Sexes
        {
            public const string Male = "Male";
            public const string Female = "Female";
        }
    }
}

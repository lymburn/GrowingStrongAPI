using System;
namespace GrowingStrongAPI.Helpers
{
    public static class Constants
    {
        public static class SharedErrorMessages
        {
            public const string UserDoesNotExist = "Unable to find user with this id";
            public const string FailedToRetrieveUser = "Error occurred trying to retrieve user";
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
            public const string Success = "Successfully authenticated user";
        }

        public static class GetUserByIdMessages
        {
            public const string Success = "Successfully retrieved user";
        }

        public static class CreateUserMessages
        {
            public const string NullOrEmptyCredentials = "Email or password is null or empty";
            public const string UserAlreadyExists = "User with this email already exists";
            public const string FailedToCreatePasswordHash = "Failed to create password hash";
            public const string FailedToCreateUser = "Failed to create user";
            public const string Success = "Successfully created user";
        }

        public static class GetUserFoodEntriesMessages
        {
            public const string FailedToRetrieveFoodEntry = "Failed to retrieve food entries for this user";
            public const string Success = "Successfully retrieved food entries for this user";
        }

        public static class UpdateFoodEntryMessages
        {
            public const string FoodEntryDoesNotExist = "Unable to find food entry with this id";
            public const string FailedToUpdateFoodEntry = "Failed to update food entry";
            public const string Success = "Successfully update food entry";
        }
    }
}

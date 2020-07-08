using System;
namespace GrowingStrongAPI.Helpers
{
    public static class Constants
    {
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
    }
}

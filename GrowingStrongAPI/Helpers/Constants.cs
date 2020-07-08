using System;
namespace GrowingStrongAPI.Helpers
{
    public static class Constants
    {
        public static class AuthenticateUserMessages
        {
            public const string InvalidCredentials = "Invalid email address or password";
            public const string FailedToGenerateJWT = "Failed to generate JWT";
            public const string Success = "Successfully authenticated user";
        }
    }
}

using System;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Helpers.Extensions;

namespace GrowingStrongAPI.Schemas
{
    public static class UserSchema
    {
        public static string Table { get; } = "user_details";

        public static class Columns
        {
            public static string Id { get; } = nameof(User.Id).ToSnakeCase();
            public static string Username { get; } = nameof(User.Username).ToSnakeCase();
            public static string EmailAddress { get; } = nameof(User.EmailAddress).ToSnakeCase();
            public static string FirstName { get; } = nameof(User.FirstName).ToSnakeCase();
            public static string LastName { get; } = nameof(User.LastName).ToSnakeCase();
        }
    }
}

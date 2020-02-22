using System;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Helpers.Extensions;

namespace GrowingStrongAPI.Helpers.Schemas
{
    public static class UserSchema
    {
        public static string Table { get; } = "user_accounts";

        public static class Columns
        {
            public static string Id { get; } = nameof(User.Id).ToSnakeCase();
            public static string EmailAddress { get; } = nameof(User.EmailAddress).ToSnakeCase();
            public static string PasswordHash { get; } = nameof(User.PasswordHash).ToSnakeCase();
            public static string PasswordSalt { get; } = nameof(User.PasswordSalt).ToSnakeCase();
        }
    }
}

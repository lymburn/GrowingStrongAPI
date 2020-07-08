using System;
namespace GrowingStrongAPI.Models
{
    public static class ResponseStatusCode
    {
        public static readonly int OK = 200;
        public static readonly int BAD_REQUEST = 400;
        public static readonly int UNAUTHORIZED = 401;
        public static readonly int CONFLICT = 409;
        public static readonly int INTERNAL_SERVER_ERROR = 500;
    }
}

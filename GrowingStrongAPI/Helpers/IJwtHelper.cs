using System;
namespace GrowingStrongAPI.Helpers
{
    public interface IJwtHelper
    {
        public string GenerateJWT(int userId, string secret);
    }
}

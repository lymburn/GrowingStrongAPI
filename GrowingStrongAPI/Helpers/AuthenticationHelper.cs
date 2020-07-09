using System;
namespace GrowingStrongAPI.Helpers
{
    public class AuthenticationHelper: IAuthenticationHelper
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            try
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;

                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password is null || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException(Constants.AuthenticationHelperExceptions.InvalidPasswordHashLength);
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException(Constants.AuthenticationHelperExceptions.InvalidPasswordSaltLength);
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

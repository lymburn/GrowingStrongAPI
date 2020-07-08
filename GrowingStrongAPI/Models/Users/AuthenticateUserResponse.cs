using System;
namespace GrowingStrongAPI.Models
{
    public class AuthenticateUserResponse: BaseResponse
    {
        public string Token { get; set; }

        public AuthenticateUserResponse()
        {
            Token = string.Empty;
        }
    }
}

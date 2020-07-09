using System;
namespace GrowingStrongAPI.Models
{
    public class AuthenticateUserResponse: BaseResponse
    {
        public string Token { get; set; }
        public UserDto UserDto { get; set; }

        public AuthenticateUserResponse()
        {
            Token = string.Empty;
            UserDto = new UserDto();
        }
    }
}

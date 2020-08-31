using System;
namespace GrowingStrongAPI.Models
{
    public class RegisterUserResponse : BaseResponse
    {
        public UserDto userDto;

        public RegisterUserResponse()
        {
            userDto = new UserDto();
        }
    }
}
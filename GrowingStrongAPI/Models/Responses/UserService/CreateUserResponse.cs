using System;
namespace GrowingStrongAPI.Models
{
    public class CreateUserResponse : BaseResponse
    {
        public UserDto userDto;

        public CreateUserResponse()
        {
            userDto = new UserDto();
        }
    }
}
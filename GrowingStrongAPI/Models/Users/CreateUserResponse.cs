using System;
namespace GrowingStrongAPI.Models.Users
{
    public class CreateUserResponse: BaseResponse
    {
        public UserDto userDto;

        public CreateUserResponse()
        {
            userDto = new UserDto();
        }
    }
}

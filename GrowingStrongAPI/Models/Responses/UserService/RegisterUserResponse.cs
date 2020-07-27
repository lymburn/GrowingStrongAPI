using System;
namespace GrowingStrongAPI.Models
{
    public class RegisterUserResponse : BaseResponse
    {
        public UserDto userDto;
        public UserProfileDto userProfileDto;
        public UserTargetsDto userTargetsDto;

        public RegisterUserResponse()
        {
            userDto = new UserDto();
            userProfileDto = new UserProfileDto();
            userTargetsDto = new UserTargetsDto();
        }
    }
}
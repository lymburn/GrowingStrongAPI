using System;
namespace GrowingStrongAPI.Models
{
    public class GetUserByIdResponse : BaseResponse
    {
        public UserDto UserDto { get; set; }

        public GetUserByIdResponse()
        {
            UserDto = new UserDto();
        }
    }
}

using System;
namespace GrowingStrongAPI.Models
{
    public class BaseResponse
    {
        public ResponseStatus ResponseStatus { get; set; }

        public BaseResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
    }
}

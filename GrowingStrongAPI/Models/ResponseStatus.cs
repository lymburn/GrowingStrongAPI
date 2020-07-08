using System;
namespace GrowingStrongAPI.Models
{
    public class ResponseStatus
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public ResponseStatus()
        {
            Message = string.Empty;
            Status = ResponseStatusCode.S_ERROR;
        }
    }
}

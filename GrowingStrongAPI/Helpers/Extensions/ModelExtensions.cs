using System;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Helpers.Extensions
{
    public static class ModelExtensions
    {
        public static void SetError(this ResponseStatus responseStatus, string errorMessage)
        {
            responseStatus.Status = ResponseStatusCode.S_ERROR;
            responseStatus.Message = errorMessage;
        }

        public static void SetOk(this ResponseStatus responseStatus)
        {
            responseStatus.Status = ResponseStatusCode.S_OK;
            responseStatus.Message = "OK";
        }

        public static void SetOk(this ResponseStatus responseStatus, string message)
        {
            responseStatus.SetOk();
            responseStatus.Message = message;
        }
    }
}

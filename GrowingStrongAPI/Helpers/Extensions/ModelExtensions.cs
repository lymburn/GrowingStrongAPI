using System;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Helpers.Extensions
{
    public static class ModelExtensions
    {
        public static void SetError(this ResponseStatus responseStatus, int statusCode, string errorMessage)
        {
            responseStatus.Status = statusCode;
            responseStatus.Message = errorMessage;
        }

        public static void SetOk(this ResponseStatus responseStatus)
        {
            responseStatus.Status = ResponseStatusCode.OK;
        }

        public static void SetOk(this ResponseStatus responseStatus, string message)
        {
            responseStatus.SetOk();
            responseStatus.Message = message;
        }

        public static bool HasError(this ResponseStatus responseStatus)
        {
            return responseStatus.Status != ResponseStatusCode.OK;
        }
    }
}

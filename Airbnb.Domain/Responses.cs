using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain
{
    public class Responses
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

        #region SuccessResponses
        public static async Task<Responses> SuccessResponse(object Token, string message)
        {
            return new Responses
            {
                IsSuccess = true,
                Message = message,
                Data = Token
            };
        }
        public static async Task<Responses> SuccessResponse(string Email,string message)
        {
            return new Responses
            {
                IsSuccess = true,
                Data = Email,
                Message = message
            };
        }
        #endregion


        // Fail Response
        #region Failur Responses
        public static async Task<Responses> FailurResponse(string? Message)
        {
            return new Responses
            {
                IsSuccess = false,
                Message = Message,
            };
        }
        public static async Task<Responses> FailurResponse(object? Errors)
        {
            return new Responses
            {
                IsSuccess = false,
                Data = Errors
            };
        } 
        #endregion
    }
}

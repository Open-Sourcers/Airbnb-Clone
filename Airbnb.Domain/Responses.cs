using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain
{
    public class Responses
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        public Responses(HttpStatusCode statusCode = HttpStatusCode.OK) => StatusCode = statusCode;

        #region SuccessResponses
        public static async Task<Responses> SuccessResponse(string? Message)
        {
            return new Responses
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = Message,
            };
        }
        public static async Task<Responses> SuccessResponse(object? data)
        {
            return new Responses
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data = data,
            };
        }
        public static async Task<Responses> SuccessResponse(object? Token, string message)
        {
            return new Responses
            {
                StatusCode=HttpStatusCode.OK,
                IsSuccess = true,
                Message = message,
                Data = Token
            };
        }
        public static async Task<Responses> SuccessResponse(string Email,string message)
        {
            return new Responses
            {
                StatusCode=HttpStatusCode.OK,
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
                StatusCode=HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = Message,
            };
        }
        public static async Task<Responses> FailurResponse(string? Message,HttpStatusCode statusCode)
        {
            return new Responses
            {
                StatusCode = statusCode,
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
        public static async Task<Responses> FailurResponse(HttpStatusCode statusCode)
        {
            return new Responses
            {
                StatusCode=statusCode,
                IsSuccess = false,
            };
        }
        public static async Task<Responses> FailurResponse(object? Errors, HttpStatusCode statusCode)
        {
            return new Responses
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Data = Errors
            };
        }
       
        #endregion
    }
}

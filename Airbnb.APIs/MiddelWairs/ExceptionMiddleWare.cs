
using Airbnb.Domain;
using System.Net;
using System.Text.Json;

namespace Airbnb.APIs.MiddelWairs
{
    public class ExceptionMiddleWare : IMiddleware
    {
        private readonly IHostEnvironment _hostEnv;

        private readonly ILogger<ExceptionMiddleWare> _logger;
        public ExceptionMiddleWare(IHostEnvironment hostEnv, ILogger<ExceptionMiddleWare> logger)
        {
            _hostEnv = hostEnv;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var Response = await Responses.FailurResponse(HttpStatusCode.InternalServerError);
                await context.Response.WriteAsync(JsonSerializer.Serialize(Response));
            }

        }
    }
}

/*
   
 */
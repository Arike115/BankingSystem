using System.Net;
using Microsoft.AspNetCore.SignalR;
using Solhigson.Framework.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace BankingSystem.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly IHub _hub;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, IHub hub)
        {
            _hub = hub;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _hub.CaptureException(e);

                await HandleExceptionAsync(context);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";


            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Internal Server Error"
            };

            await httpContext.Response.WriteAsync(response.SerializeToJson());
        }

    }
}

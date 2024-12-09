using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using BankingSystem.Domain.Model;

namespace BankingSystem.Middleware
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IHub _hub;

        public ValidationFilter(IHub hub)
        {
            _hub = hub;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (!context.ModelState.IsValid)
            {
                if (context.ActionDescriptor.DisplayName.Contains("Login") || context.ActionDescriptor.DisplayName.Contains("InitiatePasswordReset"))
                {
                    await next();
                }

                var modelStateErrors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

                var errorResponse = new ErrorResponse();

                foreach (var error in modelStateErrors)
                {
                    foreach (var subError in error.Value)
                    {
                        errorResponse.Errors.Add(subError);
                    }
                }
                errorResponse.Message = errorResponse.Errors.Count() > 0 ? errorResponse.Errors.First() : "Please correct the errors and try again.";
                // errorResponse.Message = "Validation errors were found. Please review and retry.";
                errorResponse.TraceId = Activity.Current.Id;
                // errorResponse.ValidationErrors = validationErrors;
                context.Result = new BadRequestObjectResult(errorResponse);
                _hub?.CaptureException(new BankingException($@"Action - {context.ActionDescriptor.DisplayName}
                    {Environment.NewLine}
                    arguements - {JsonConvert.SerializeObject(context.ActionArguments)}
                    {Environment.NewLine}
                    result - {JsonConvert.SerializeObject(errorResponse)}
                    "));

                return;
            }


            await next();
        }

    }

    public class BankingException : Exception
    {

        public BankingException(string message, int? returnCode = null) : base(message)
        {
            ReturnCode = returnCode;
        }

        public BankingException(string message, Exception inner, int? returnCode = null)
            : base(message, inner)
        {
            ReturnCode = returnCode;
        }

        public int? ReturnCode { get; set; }
    }
}

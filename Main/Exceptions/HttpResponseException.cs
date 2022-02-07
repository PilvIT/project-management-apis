using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Main.Exceptions
{
    /// <summary>
    /// Mimics the behavior of HttpResponseException, the original exception was removed since ASP.NET Core 5. 
    ///
    /// <a href="https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-5.0#use-exceptions-to-modify-the-response">
    ///     Official Guide
    /// </a>
    /// </summary>
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }
        
        /// <summary>
        /// Optional error message, it can be anything.
        /// </summary>
        public object? ResponseData { get; set; }

        protected HttpResponseException(int httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }
    }

    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No need to perform anything
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is not HttpResponseException exception) return;
            
            context.Result = new ObjectResult(exception.ResponseData)
            {
                StatusCode = exception.StatusCode
            };
            context.ExceptionHandled = true;
        }
    }
}

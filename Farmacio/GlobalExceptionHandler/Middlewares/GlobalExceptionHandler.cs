using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace GlobalExceptionHandler.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HandlableException exception)
            {
                await HandleResponseBasedOnException(context, exception);
            }
        }

        private Task HandleResponseBasedOnException(HttpContext context, HandlableException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.Code;

            var responseObject = JObject.FromObject(new
            {
                StatusCode = exception.Code,
                ErrorMessage = exception.Message
            });

            return context.Response.WriteAsync(responseObject.ToString());
        }
    }
}
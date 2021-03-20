using Microsoft.AspNetCore.Builder;

namespace GlobalExceptionHandler.Extensions
{
    public static class GlobalExceptionHandlerMiddleware
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<Middlewares.GlobalExceptionHandler>();
        }
    }
}
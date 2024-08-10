using API.Errors;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace API.Midlleware
{
    public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
                await HandleExceptionAsync(context, ex, env);
			}
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment()
                   ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                   : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(json);
        }
    }
}

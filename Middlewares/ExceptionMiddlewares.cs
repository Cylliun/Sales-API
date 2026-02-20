using System.Net;
using System.Text.Json;

namespace SalesApi.Middlewares
{
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddlewares(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (AppException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                    {
                        error = ex.Message,
                    });
            }

            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Um erro inesperado ocorreu.",
                    });
            }

        }

    }
}

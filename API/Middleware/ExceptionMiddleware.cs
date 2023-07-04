using Application.Core;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; //Bir sonraki middleware bileşenine yönlendirmek için kullanılır.
        private readonly ILogger<ExceptionMiddleware> _logger; //Hata günlüğü (log) işlemleri için kullanılır.
        private readonly IHostEnvironment _env; //Uygulamanın çalıştığı ortam bilgisini sağlar.

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                                ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                                : new AppException(context.Response.StatusCode, "Internal Server Error");

                var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}

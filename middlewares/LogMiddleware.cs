using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace myTasks.Middlewares
{
    public class LogMiddleware
    {
        private RequestDelegate next;
        private ILogger<LogMiddleware> logger;

        public LogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<LogMiddleware>();
        }

        public async Task Invoke(HttpContext c)
        {
            var sw = new Stopwatch();
            sw.Start();
            await next(c);
            sw.Stop();

             var requestTime = DateTime.Now;
            string logMessage = $"{requestTime.ToString()} - {c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms. User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}";

            using (var writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine(logMessage);
            }

           // logger.LogDebug(logMessage);
        }
    }

    public static partial class MiddleExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Point.Common.LoggerProvider;

namespace Point.Common.Middleware.Logger
{
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogResponseMiddleware> _logger;

        public LogResponseMiddleware(RequestDelegate next, ILogger<LogResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var bodyStream = context.Response.Body;

            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

            var tags = context.Request.Path.Value.Split('/').Where(x => !String.IsNullOrEmpty(x) && !x.Equals("api")).ToList();
            tags.Add(context.Request.Method);
            tags.Add("response");

            var tagsString = String.Join(",", tags).ToLowerInvariant();

            var headers = context.Response.Headers.ToDictionary(x => x.Key, x => String.Join(", ", x.Value));
            var statusCode = context.Response.StatusCode;

            var responseData = new
            {
                StatusCode = statusCode,
                Headers = headers,
                Body = new JRaw(responseBody)
            };

            _logger.Log(LogLevel.Information, 0, new LogData(Guid.NewGuid(), tagsString, "key:test", responseData), null, LoggerHelper.Formatter);

            responseBodyStream.Seek(0, SeekOrigin.Begin);

            await responseBodyStream.CopyToAsync(bodyStream);
        }
    }
}

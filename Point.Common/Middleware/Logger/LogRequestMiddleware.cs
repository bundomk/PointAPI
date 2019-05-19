using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Point.Common.LoggerProvider;
using Microsoft.AspNetCore.Http.Extensions;

namespace Point.Common.Middleware.Logger
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestMiddleware> _logger;

        public LogRequestMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var url = UriHelper.GetDisplayUrl(context.Request);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
            var headers = context.Request.Headers.ToDictionary(x => x.Key, x => String.Join(", ", x.Value));

            var requestData = new
            {
                Url = url,
                Method = context.Request.Method,
                Headers = headers,
                Body = new JRaw(requestBodyText)
            };

            var tags = context.Request.Path.Value.Split('/').Where(x => !String.IsNullOrEmpty(x) && !x.Equals("api")).ToList();
            tags.Add(context.Request.Method);
            tags.Add("request");

            var tagsString = String.Join(",", tags).ToLowerInvariant();

            _logger.Log(LogLevel.Information, 0, new LogData(Guid.NewGuid(), tagsString, "key:test", requestData), null, LoggerHelper.Formatter);

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            await _next(context);

            context.Request.Body = originalRequestBody;
        }
    }
}

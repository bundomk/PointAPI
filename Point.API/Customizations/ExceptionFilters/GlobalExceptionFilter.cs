using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Point.Common.LoggerProvider;
using System;
using System.Linq;
using System.Net;

namespace Point.API.Customizations.ExceptionFilters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Reason = context.Exception?.ToString()
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

#if DEBUG
            var httpContext = context.HttpContext;

            var url = UriHelper.GetDisplayUrl(httpContext.Request);

            var tags = httpContext.Request.Path.Value.Split('/').Where(x => !String.IsNullOrEmpty(x) && !x.Equals("api")).ToList();
            tags.Add(httpContext.Request.Method);
            tags.Add("error");

            var tagsString = String.Join(",", tags).ToLowerInvariant();

            _logger.Log(LogLevel.Error, 0, new LogData(Guid.NewGuid(), tagsString, "key:test"), context.Exception, LoggerHelper.Formatter);
#endif
        }

        public void Dispose()
        {
        }
    }
}

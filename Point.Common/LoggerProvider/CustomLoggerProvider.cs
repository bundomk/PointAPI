using Microsoft.Extensions.Logging;
using System;

namespace Point.Common.LoggerProvider
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;

        public CustomLoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(categoryName, _filter);
        }

        public void Dispose()
        { }
    }
}

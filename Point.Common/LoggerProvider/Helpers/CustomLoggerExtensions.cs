using Microsoft.Extensions.Logging;
using System;

namespace Point.Common.LoggerProvider
{
    public static class CustomLoggerExtensions
    {
        public static ILoggerFactory AddCustomLogger(this ILoggerFactory factory, Func<string, LogLevel, bool> filter = null)
        {
            factory.AddProvider(new CustomLoggerProvider(filter));

            return factory;
        }

        public static ILoggerFactory AddCustomLogger(this ILoggerFactory factory, LogLevel minLevel)
        {
            return AddCustomLogger(factory, (x, logLevel) => logLevel >= minLevel);
        }
    }
}

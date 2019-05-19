using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Point.Common.LoggerProvider
{
    public class CustomLogger : ILogger
    {
        private string _categoryName;
        private Func<string, LogLevel, bool> _filter;

        public CustomLogger(string categoryName, Func<string, LogLevel, bool> filter)
        {
            _categoryName = categoryName;
            _filter = filter;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if(!(state is LogData))
            {
                return;
            }
            
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            var logData = state as LogData;

            // use log data

            var logFilePath = $@"C:\Logger\Point\log_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.txt";

            Directory.CreateDirectory(@"C:\Logger\Point");

            File.AppendAllText(logFilePath, message);
        }
        
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}

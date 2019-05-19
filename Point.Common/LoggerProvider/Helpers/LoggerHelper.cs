using Newtonsoft.Json;
using System;

namespace Point.Common.LoggerProvider
{
    public static class LoggerHelper
    {
        public static Func<LogData, Exception, string> Formatter = FormatFullMessage;

        private static string FormatFullMessage(LogData logData, Exception ex)
        {
            if(logData == null)
            {
                throw new ArgumentNullException($"[LoggerHelper.FormatFullMessage]: {nameof(logData)} is NULL");
            }

            if (ex != null)
            {
                logData.Message = ex.ToString();
            }

            return JsonConvert.SerializeObject(logData);
        }

        private static string FormatMessage(LogData logData, Exception ex)
        {
            if (logData == null)
            {
                throw new ArgumentNullException($"[LoggerHelper.FormatMessage]: {nameof(logData)} is NULL");
            }

            if (ex != null)
            {
                return ex.ToString();
            }

            return logData.Message.ToString();
        }
    }
}

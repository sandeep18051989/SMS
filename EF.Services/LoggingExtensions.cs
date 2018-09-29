using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Service;
using System;

namespace EF.Services
{
    public static class LoggingExtensions
    {
        public static void Debug(this ISystemLogService logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception, user);
        }
        public static void Information(this ISystemLogService logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception, user);
        }
        public static void Warning(this ISystemLogService logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception, user);
        }
        public static void Error(this ISystemLogService logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception, user);
        }
        public static void Fatal(this ISystemLogService logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception, user);
        }

        private static void FilteredLog(ISystemLogService logger, LogLevel level, string message, Exception exception = null, User user = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            string fullMessage = exception == null ? string.Empty : exception.ToString();
            logger.InsertSystemLog(level, message, fullMessage, user);
        }
    }
}

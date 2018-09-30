using EF.Core.Data;
using EF.Core.Enums;
using System;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface ISystemLogService
    {
        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">SystemLog item</param>
        void DeleteLog(SystemLog log);

        /// <summary>
        /// Deletes a log items
        /// </summary>
        /// <param name="logs">SystemLog items</param>
        void DeleteLogs(IList<SystemLog> logs);

        /// <summary>
        /// Clears a log
        /// </summary>
        void ClearLog();

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <param name="fromUtc">Log item creation from; null to load all records</param>
        /// <param name="toUtc">Log item creation to; null to load all records</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Log level; null to load all records</param>
        /// <returns>Log item items</returns>
        IList<SystemLog> GetAllSystemLogs(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel ? Level = null, bool? isFixed = null);

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">SystemLog item identifier</param>
        /// <returns>Log item</returns>
        SystemLog GetSystemLogById(int systemlogId);

        /// <summary>
        /// Get SystemLog items by identifiers
        /// </summary>
        /// <param name="logIds">SystemLog item identifiers</param>
        /// <returns>SystemLog items</returns>
        IList<SystemLog> GetSystemLogByIds(int[] systemlogIds);

        /// <summary>
        /// Inserts a SystemLog item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="customer">The customer to associate log record with</param>
        /// <returns>A SystemLog item</returns>
        SystemLog InsertSystemLog(LogLevel Level, string Message, string StackTrace = "", User user = null);
    }
}

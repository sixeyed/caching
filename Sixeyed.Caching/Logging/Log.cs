using log4net.Config;
using Sixeyed.Caching.Extensions;
using System;
using System.Collections.Generic;

namespace Sixeyed.Caching.Logging
{
    /// <summary>
    /// Log class wrapping configured log4net appenders
    /// </summary>
    public class Log
    {
        private static Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>();
        private static object _loggerSyncLock = new object();        

        private static Logger GetLogger(string loggerName = null)
        {
            if (loggerName.IsNullOrEmpty())
            {
                loggerName = GetDefaultLoggerName();
            }
            if (!_loggers.ContainsKey(loggerName))
            {
                lock (_loggerSyncLock)
                {
                    if (!_loggers.ContainsKey(loggerName))
                    {
                        XmlConfigurator.Configure();
                        _loggers[loggerName] = new Logger(loggerName);
                    }
                }
            }
            return _loggers[loggerName];
        }

        public static string GetDefaultLoggerName()
        {
            return LoggerName.Default;
        }

        /// <summary>
        /// Gets a named logger to write to
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static Logger Using(string loggerName)
        {
            return GetLogger(loggerName);
        }

        /// <summary>
        /// Formats and writes a DEBUG-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public static void Debug(string message, params object[] args)
        {
            GetLogger().Debug(message, args);
        }

        /// <summary>
        /// Formats and writes an INFO-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public static void Info(string message, params object[] args)
        {
            GetLogger().Info(message, args);
        }

        /// <summary>
        /// Formats and writes a WARN-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public static void Warn(string message, params object[] args)
        {
            GetLogger().Warn(message, args);
        }

        /// <summary>
        /// Formats and writes a ERROR-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public static void Error(string message, params object[] args)
        {
            GetLogger().Error(message, args);
        }
        
        /// <summary>
        /// Formats and writes a ERROR-level message to the log, using the log4net configuration, appending exception details
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public static void Error(Exception ex, string message, params object[] args)
        {
            GetLogger().Error(ex, message, args);
        }
        /// <summary>
        /// Formats and writes a FATAL-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public static void Fatal(string message, params object[] args)
        {
            GetLogger().Fatal(message, args);
        }


        private struct LoggerName
        {
            public const string Default = "Sixeyed.Caching";
        }
    }
}


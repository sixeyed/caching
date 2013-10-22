using log4net;
using log4net.Config;
using log4net.Core;
using Sixeyed.Caching.Extensions;
using System;
using System.Collections.Generic;

namespace Sixeyed.Caching.Logging
{
    /// <summary>
    /// Log class wrapping log4net <see cref="ILog"/>
    /// </summary>
    public class Logger
    {
        public string Name { get; set; }
        private ILog _logger;
        private List<LogLevel> _enabledLevels;

        public Logger(string name)
        {
            Name = name;
            XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(Name);
        }

        /// <summary>
        /// Writes a DEBUG-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Expression to evaluate the log message</param>
        public void Debug(Func<string> message)
        {
            WriteLog(LogLevel.Debug, message);
        }

        /// <summary>
        /// Formats and writes a DEBUG-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public void Debug(string message, params object[] args)
        {
            Debug(() => (message.FormatWith(args)));
        }

        /// <summary>
        /// Writes an INFO-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Expression to evaluate the log message</param>
        public void Info(Func<string> message)
        {
            WriteLog(LogLevel.Info, message);
        }

        /// <summary>
        /// Formats and writes an INFO-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public void Info(string message, params object[] args)
        {
            Info(() => (message.FormatWith(args)));
        }

        /// <summary>
        /// Writes a WARN-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Expression to evaluate the log message</param>
        public void Warn(Func<string> message)
        {
            WriteLog(LogLevel.Warn, message);
        }

        /// <summary>
        /// Formats and writes a WARN-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public void Warn(string message, params object[] args)
        {
            Warn(() => (message.FormatWith(args)));
        }

        /// <summary>
        /// Writes an ERROR-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Expression to evaluate the log message</param>
        public void Error(Func<string> message)
        {
            WriteLog(LogLevel.Error, message);
        }

        /// <summary>
        /// Formats and writes a ERROR-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public void Error(string message, params object[] args)
        {
           Error(() => (message.FormatWith(args)));
        }

        /// <summary>
        /// Formats and writes a ERROR-level message to the log, using the log4net configuration, appdending exception details
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public void Error(Exception ex, string message, params object[] args)
        {
            WriteLog(LogLevel.Error, () => (message.FormatWith(args) + ": {0}".FormatWith(ex)));
        }

        /// <summary>
        /// Writes a FATAL-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Expression to evaluate the log message</param>
        public void Fatal(Func<string> message)
        {
            WriteLog(LogLevel.Fatal, message);
        }

        /// <summary>
        /// Formats and writes a FATAL-level message to the log, using the log4net configuration
        /// </summary>
        /// <param name="message">Log message format</param>
        /// <param name="args">Log message arguments</param>
        public void Fatal(string message, params object[] args)
        {
            Fatal(() => (message.FormatWith(args)));
        }

        private void WriteLog(LogLevel level, Func<string> messageFunc)
        {
            if (IsLogEnabled(level))
            {
                var message = messageFunc();
                switch (level)
                {
                    case LogLevel.Debug:
                        _logger.Debug(message);
                        break;
                    case LogLevel.Error:
                        _logger.Error(message);
                        break;
                    case LogLevel.Fatal:
                        _logger.Fatal(message);
                        break;
                    case LogLevel.Info:
                        _logger.Info(message);
                        break;
                    case LogLevel.Warn:
                        _logger.Warn(message);
                        break;
                }
            }
        }

        private void WriteErrorLog(int errorTypeId, Exception ex, Func<string> messageFunc)
        {
            if (IsLogEnabled(LogLevel.Error))
            {
                var message = messageFunc();
                var loggingEvent = new LoggingEvent(GetType(), _logger.Logger.Repository, _logger.Logger.Name, Level.Error, message, ex);
                loggingEvent.Properties["EventID"] = errorTypeId;
                _logger.Logger.Log(loggingEvent);
            }
        }           

        private bool IsLogEnabled(LogLevel level)
        {
            EnsureLogLevelEnabledCache();
            return _enabledLevels.Contains(level);
        }

        private void EnsureLogLevelEnabledCache()
        {
            if (_enabledLevels == null)
            {
                if (_enabledLevels == null)
                {
                    _enabledLevels = new List<LogLevel>(5);
                    if (_logger.IsDebugEnabled)
                    {
                        _enabledLevels.Add(LogLevel.Debug);
                    }
                    if (_logger.IsInfoEnabled)
                    {
                        _enabledLevels.Add(LogLevel.Info);
                    }
                    if (_logger.IsWarnEnabled)
                    {
                        _enabledLevels.Add(LogLevel.Warn);
                    }
                    if (_logger.IsErrorEnabled)
                    {
                        _enabledLevels.Add(LogLevel.Error);
                    }
                    if (_logger.IsFatalEnabled)
                    {
                        _enabledLevels.Add(LogLevel.Fatal);
                    }
                }
            }
        }
    }
}


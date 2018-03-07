using System;
using log4net;

namespace SC2BM.Logging.Loggers
{
    public class FileLogger : ILogger
    {
        private readonly ILog _log;

        public FileLogger(ILog log)
        {
            _log = log;
        }

        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Error(Exception exception)
        {
            _log.Error(exception);
        }

        public void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }
    }
}

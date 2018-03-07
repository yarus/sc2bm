using System;

namespace SC2BM.Logging
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(string message);
        void Error(Exception exception);
        void Error(string message, Exception exception);
        void Info(string message);
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
    }
}

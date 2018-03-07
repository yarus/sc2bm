using System.Collections.Generic;
using log4net;
using log4net.Config;
using SC2BM.Logging.Loggers;

namespace SC2BM.Logging
{
    public static class Logger
    {
        private enum LoggerType
        {
            Server,
            Client,
            Common
        }

        private static readonly Dictionary<LoggerType, ILogger> FileLogManagers = new Dictionary<LoggerType, ILogger>
		{
			{ LoggerType.Server, new FileLogger(LogManager.GetLogger(LoggerType.Server.ToString())) },
			{ LoggerType.Client, new FileLogger(LogManager.GetLogger(LoggerType.Client.ToString())) },
			{ LoggerType.Common, new FileLogger(LogManager.GetLogger(LoggerType.Common.ToString())) },
		};

        public static void Initialize()
        {
            XmlConfigurator.Configure();
        }

        private static ILogger Get(LoggerType type)
        {
            return FileLogManagers[type];
        }

        public static ILogger Server
        {
            get
            {
                return Get(LoggerType.Server);
            }
        }

        public static ILogger Client
        {
            get
            {
                return Get(LoggerType.Client);
            }
        }

        public static ILogger Common
        {
            get
            {
                return Get(LoggerType.Common);
            }
        }
    }
}

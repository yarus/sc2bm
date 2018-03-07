using System;
using System.Configuration;

namespace SC2BM.Core.Configuration
{
	public static class Config
	{
		public static LoggingSection Logging
		{
			get { return (LoggingSection) ConfigurationManager.GetSection("logging"); }
		}

		public static WebConfigEnvironment WebConfigEnvironment
		{
			get
			{
				WebConfigEnvironment e = WebConfigEnvironment.Debug;
				Enum.TryParse(ConfigurationManager.AppSettings["Environment"], out e);
				return e;
			}
		}

		public static bool RequireSSL
		{
			get { return (WebConfigEnvironment >= WebConfigEnvironment.Alpha); }
		}
	}
}
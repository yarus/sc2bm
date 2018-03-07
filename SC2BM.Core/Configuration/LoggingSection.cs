using System;
using System.Configuration;

namespace SC2BM.Core.Configuration
{
	public enum ExceptionsEmailLevel
	{
		Normal,
		Detailed
	}

	public class LoggingSection : ConfigurationSection
	{
		[ConfigurationProperty("exceptions", DefaultValue = "false", IsRequired = false)]
		public Boolean Exceptions
		{
			get
			{
				return (Boolean)this["exceptions"];
			}
			set
			{
				this["exceptions"] = value;
			}
		}

		[ConfigurationProperty("activity", DefaultValue = "false", IsRequired = false)]
		public Boolean Activity
		{
			get
			{
				return (Boolean)this["activity"];
			}
			set
			{
				this["activity"] = value;
			}
		}

		[ConfigurationProperty("httpRequests", DefaultValue = "false", IsRequired = false)]
		public Boolean HttpRequests
		{
			get
			{
				return (Boolean)this["httpRequests"];
			}
			set
			{
				this["httpRequests"] = value;
			}
		}

		[ConfigurationProperty("sessionStart", DefaultValue = "false", IsRequired = false)]
		public Boolean SessionStart
		{
			get
			{
				return (Boolean)this["sessionStart"];
			}
			set
			{
				this["sessionStart"] = value;
			}
		}

		[ConfigurationProperty("emails", DefaultValue = "false", IsRequired = false)]
		public Boolean Emails
		{
			get
			{
				return (Boolean)this["emails"];
			}
			set
			{
				this["emails"] = value;
			}
		}

		[ConfigurationProperty("exceptionsEmailLevel", DefaultValue = ExceptionsEmailLevel.Normal, IsRequired = false)]
		public ExceptionsEmailLevel ExceptionsEmailLevel
		{
			get
			{
				return (ExceptionsEmailLevel)this["exceptionsEmailLevel"];
			}
			set
			{
				this["exceptionsEmailLevel"] = value;
			}
		}
	}
}
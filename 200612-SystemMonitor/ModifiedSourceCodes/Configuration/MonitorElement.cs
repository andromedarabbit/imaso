using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SystemMonitor.Configuration
{
	class MonitorElement : ConfigurationElement
	{
		[ConfigurationProperty("runFrequency")]
		public TimeSpan? RunFrequency
		{
			get { return (TimeSpan?)base["runFrequency"]; }
			set { base["runFrequency"] = value; }
		}

		[ConfigurationProperty("type")]
		public string TypeName
		{
			get { return (string)base["type"]; }
			set { base["type"] = value; }
		}

		[ConfigurationProperty("settings", IsDefaultCollection = false)]
		public SettingElementCollection Settings
		{
			get { return (SettingElementCollection)base["settings"]; }
		}

		[ConfigurationProperty("notifiers", IsDefaultCollection = false)]
		public NotifierElementCollection Notifiers
		{
			get { return (NotifierElementCollection)base["notifiers"]; }
		}
	}
}

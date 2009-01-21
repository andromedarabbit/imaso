using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SystemMonitor.Configuration
{
	class MonitorSettingsSection : ConfigurationSection
	{
		[ConfigurationProperty("monitors", IsDefaultCollection = false)]
		public MonitorElementCollection Monitors
		{
			get { return (MonitorElementCollection)base["monitors"]; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SystemMonitor.Configuration
{
	class NotifierElement : ConfigurationElement
	{
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
	}
}

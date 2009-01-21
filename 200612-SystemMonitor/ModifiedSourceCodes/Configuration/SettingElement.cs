using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SystemMonitor.Configuration
{
	class SettingElement : ConfigurationElement
	{
		[ConfigurationProperty("name")]
		public string Name
		{
			get { return (string)base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("value")]
		public string Value
		{
			get { return (string)base["value"]; }
			set { base["value"] = value; }
		}
	}
}

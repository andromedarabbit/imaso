using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SystemMonitor.Configuration
{
	class MonitorElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new MonitorElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return element.GetHashCode();
		}

		public MonitorElement this[int index]
		{
			get { return (MonitorElement)BaseGet(index); }

			set 
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		protected override bool IsElementName(string elementName)
		{
			return elementName == "monitor";
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMapAlternate; }
		}
	}
}

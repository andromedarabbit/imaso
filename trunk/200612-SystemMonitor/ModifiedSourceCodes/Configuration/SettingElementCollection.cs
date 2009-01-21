using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SystemMonitor.Configuration
{
	class SettingElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new SettingElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return element.GetHashCode();
		}

		public SettingElement this[int index]
		{
			get { return (SettingElement)BaseGet(index); }

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
			return elementName == "setting";
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMapAlternate; }
		}
	}
}

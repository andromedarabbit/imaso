using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SystemMonitor.Configuration
{
	class NotifierElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new NotifierElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return element.GetHashCode();
		}

		public NotifierElement this[int index]
		{
			get { return (NotifierElement)BaseGet(index); }

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
			return elementName == "notifier";
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMapAlternate; }
		}
	}
}

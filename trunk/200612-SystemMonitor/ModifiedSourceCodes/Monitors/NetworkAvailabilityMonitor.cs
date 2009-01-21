using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;

namespace SystemMonitor.Monitors
{
	class NetworkAvailabilityMonitor : MonitorBase
	{
		public override string Description
		{
			get { return "Monitoring network availability."; }
		}

		protected override void Initialize(Dictionary<string, string> settings)
		{
			NetworkChange.NetworkAvailabilityChanged += delegate(object sender, NetworkAvailabilityEventArgs e)
			{
				Notify("Network Availability", "Network availability has changed to: " + (e.IsAvailable ? "Available" : "Unavailable"));
			};
		}

		public override MonitorType MonitorType
		{
			get { return MonitorType.Persistent; }
		}

		public override Icon Icon
		{
			get { return Properties.Resources.NetworkAvailabilityIcon; }
		}
	}
}

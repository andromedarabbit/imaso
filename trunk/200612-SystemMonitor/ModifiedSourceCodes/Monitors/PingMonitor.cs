using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;
using System.Drawing;

namespace SystemMonitor.Monitors
{
	class PingMonitor : MonitorBase
	{
		private string _host;

		public override void Execute()
		{
			Ping ping = new Ping();

			try
			{
				PingReply reply = ping.Send(_host);
				if (reply.Status != IPStatus.Success)
				{
					Notify("Failed ping", string.Format("Ping of '{0}' failed with the '{1}' status.", _host, reply.Status.ToString()));
				}
			}
			catch (PingException ex)
			{
				Notify("Failed ping", string.Format("Ping failed: {1}", _host, ex.InnerException.Message));
			}
			catch (Exception ex)
			{
				Notify("Failed ping", string.Format("Ping of '{0}' failed with an exception: {1}", _host, ex.Message));
			}
		}

		protected override void Initialize(Dictionary<string, string> settings)
		{
			_host = settings["host"];
		}

		public override string Description
		{
			get { return string.Format("Pinging '{0}'.", _host); }
		}

		public override MonitorType MonitorType
		{
			get { return MonitorType.Scheduled; }
		}

		public override Icon Icon
		{
			get { return Properties.Resources.PingIcon; }
		}
	}
}

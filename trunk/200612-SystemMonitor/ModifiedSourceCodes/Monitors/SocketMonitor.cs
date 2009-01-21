using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;

namespace SystemMonitor.Monitors
{
	class SocketMonitor : MonitorBase
	{
		private string _host;
        private int _port;

		public override void Execute()
		{
		    try 
		    {
		        using(TcpClient client = new TcpClient())
		        {
		            client.Connect(_host, _port);
		            if(client.Connected == false)
		            {
		                Notify("Failed connect", "Connect failed.");
    		        }
	    	    }
		    }
		    catch(Exception ex)
		    {
                Notify("Failed connect", string.Format("Connect of '{0}:{1}' failed with an exception: {2}", _host, _port, ex.Message));
		    }
		}

		protected override void Initialize(Dictionary<string, string> settings)
		{
			_host = settings["host"];
            _port = int.Parse(settings["port"]);
		}

		public override string Description
		{
            get { return string.Format("Connecting '{0}:{1}'.", _host, _port); }
		}

		public override MonitorType MonitorType
		{
			get { return MonitorType.Scheduled; }
		}

		public override Icon Icon
		{
            get { return Properties.Resources.SocketIcon; }
		}
	}
}

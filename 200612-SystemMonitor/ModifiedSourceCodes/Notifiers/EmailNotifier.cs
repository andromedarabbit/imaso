using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace SystemMonitor.Notifiers
{
	class EmailNotifier : NotifierBase
	{
		private string _host;
		private string _to;
		private string _from;

		public override void Execute(string title, string message)
		{
			SmtpClient mailClient = new SmtpClient();
			mailClient.Host = _host;
			mailClient.Send(_to, _from, title, message);
		}

		public override void Initialize(Dictionary<string, string> settings)
		{
			_host = settings["host"];
			_to = settings["to"];
			_from = settings["from"];
		}
	}
}

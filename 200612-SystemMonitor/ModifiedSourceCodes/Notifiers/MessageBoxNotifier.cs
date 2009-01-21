using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SystemMonitor.Notifiers
{
	class MessageBoxNotifier : NotifierBase
	{
		public override void Execute(string title, string message)
		{
			MessageBox.Show(message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}

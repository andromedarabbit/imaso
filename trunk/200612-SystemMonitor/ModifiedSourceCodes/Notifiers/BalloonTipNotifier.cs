using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SystemMonitor.Notifiers
{
	class BalloonTipNotifier : NotifierBase
	{
		private ToolTipIcon _icon;

		public override void Execute(string title, string message)
		{
			Program.ShowBalloonTip(title, message, _icon);
		}

		public override void Initialize(Dictionary<string, string> settings)
		{
			_icon = (ToolTipIcon)Enum.Parse(typeof(ToolTipIcon), settings["icon"]);
		}
	}
}

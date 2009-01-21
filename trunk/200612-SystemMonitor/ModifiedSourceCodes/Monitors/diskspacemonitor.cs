using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Drawing;

namespace SystemMonitor.Monitors
{
	class DiskSpaceMonitor : MonitorBase
	{
		private string	_driveLetter;
		private long	_freeMegabytes;

		public override string Description
		{
			get { return string.Format("Monitoring drive '{0}' for free space below {1:n0} MB.", _driveLetter, _freeMegabytes); }
		}

		public override void Execute()
		{
			DriveInfo driveInfo = new DriveInfo(_driveLetter);

			if (driveInfo.AvailableFreeSpace < (1024 * 1024 * _freeMegabytes))
			{
				string message = string.Format("There are fewer than {0:n0} MB free on drive '{1}'.", _freeMegabytes, driveInfo.ToString());
				Notify("Disk space low", message);
			}
		}

		public override MonitorType MonitorType
		{
			get { return MonitorType.Scheduled; }
		}

		protected override void Initialize(Dictionary<string, string> settings)
		{
			_driveLetter = settings["driveLetter"];
			_freeMegabytes = int.Parse(settings["freeMegabytes"]);
		}

		public override Icon Icon
		{
			get { return Properties.Resources.DiskIcon; }
		}
	}
}

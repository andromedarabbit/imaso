using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace SystemMonitor.Monitors
{
    class EventLogMonitor : MonitorBase
    {
		string _source;
		string _logName;

        public EventLogMonitor()
        {}

		public override MonitorType MonitorType
		{
			get { return MonitorType.Persistent; }
		}

		protected override void Initialize(Dictionary<string, string> settings)
		{
			_source = settings["source"];
			_logName = settings["logName"];

			EventLog eventLog = new EventLog(_logName);
			eventLog.EnableRaisingEvents = true;
			eventLog.EntryWritten += new EntryWrittenEventHandler(EventLog_EntryWritten);
		}

		private void EventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
		{
			string source = _source.ToLower();

			if (e.Entry.Source.ToLower() == source)
			{
				string message = string.Format("The following entry has been written to the '{0}' log by '{1}':\n\n{2}",
					_logName, e.Entry.Source, e.Entry.Message);
				Notify("Event log", message);
			}
		}

		public override string Description
		{
			get { return string.Format("Monitoring the '{0}' log for items from the '{1}' source.", _logName, _source); }
		}

		public override Icon Icon
		{
			get { return Properties.Resources.EventLogIcon; }
		}
	}
}

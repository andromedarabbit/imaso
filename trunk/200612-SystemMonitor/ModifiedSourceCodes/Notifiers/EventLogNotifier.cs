using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SystemMonitor.Notifiers
{
    class EventLogNotifier : NotifierBase
    {
        private string _machineName;
        private string _source;
        private string _log;
        
        private EventLog _eventLog = new EventLog();
        
        public override void Initialize(Dictionary<string, string> settings)
        {
            _machineName = settings["machine_name"];
            _source = settings["source"];
            _log = settings["log"];
        }
        
        public override void Execute(string title, string message)
        {
            InitializeEventLog();
            _eventLog.WriteEntry(string.Format("Notification!!! {0}{0}[TITLE] {1}{0}[MESSAGE]{2}", Environment.NewLine, title, message), EventLogEntryType.Warning);
        }

        private void InitializeEventLog()
        {
            if (!EventLog.SourceExists(_source))
            {
                EventLog.CreateEventSource(_source, _log);
            }
            _eventLog.MachineName = _machineName;
            _eventLog.Source = _source;
        }
    }
    
}

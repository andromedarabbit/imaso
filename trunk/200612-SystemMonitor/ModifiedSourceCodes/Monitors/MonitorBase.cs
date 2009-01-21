using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace SystemMonitor.Monitors
{
    abstract class MonitorBase : IMonitor
    {
        private List<INotifier>             _notifiers = new List<INotifier>();
		private TimeSpan?					_runFrequency;

		public abstract string Description { get; }

		public abstract Icon Icon { get; }

		public abstract MonitorType MonitorType { get; }

		public virtual void Execute()
		{}

		protected virtual void Initialize(Dictionary<string, string> settings)
		{}

		public List<INotifier> Notifiers
        {
            get { return _notifiers; }
        }

        public TimeSpan? RunFrequency 
		{
			get { return _runFrequency; }
		}

		protected void Notify(string title, string message)
		{
			foreach (INotifier notifier in Notifiers)
			{
				notifier.Execute(title, message);
			}
		}

		public void Initialize(TimeSpan? runFrequency, Dictionary<string, string> settings)
		{
			_runFrequency = runFrequency;
			Initialize(settings);
		}
	}
}

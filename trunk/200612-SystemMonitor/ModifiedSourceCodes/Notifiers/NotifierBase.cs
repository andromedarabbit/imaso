using System;
using System.Collections.Generic;
using System.Text;

namespace SystemMonitor.Notifiers
{
    abstract class NotifierBase : INotifier
    {
        public abstract void Execute(string title, string message);

		public virtual void Initialize(Dictionary<string, string> settings)
		{}
	}
}

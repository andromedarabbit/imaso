using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SystemMonitor
{
    interface IMonitor
    {
        List<INotifier>	Notifiers { get; }
        TimeSpan?		RunFrequency { get; }
		string			Description { get; }
		void			Execute();
		MonitorType		MonitorType { get; }
		void			Initialize(TimeSpan? runFrequency, 
								   Dictionary<string, string> settings);
		Icon			Icon { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SystemMonitor
{
    interface INotifier
    {
		void Initialize(Dictionary<string, string> settings);
        void Execute(string title, string message);
    }
}

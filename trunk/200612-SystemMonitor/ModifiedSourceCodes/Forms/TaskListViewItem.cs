using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SystemMonitor.Forms
{
	class TaskListViewItem : ListViewItem
	{
		private IMonitor	_monitor;
		private Task		_task;

		public TaskListViewItem(Task task)
			: base(new string[] { "", "", "", "" })
		{
			_task = task;
			_monitor = task.Monitor;
			_task.Changed += delegate
			{
				UpdateSubItems();
			};

			UpdateSubItems();
		}

		public IMonitor Monitor
		{
			get { return _monitor; }
		}

		public Task Task
		{
			get { return _task; }
		}

		private void UpdateSubItems()
		{
			SubItems[0].Text = _monitor.Description;
			SubItems[1].Text = _task.Status.ToString();

			if (_monitor.MonitorType != MonitorType.Persistent)
			{
				SubItems[2].Text = _task.LastRunTime.HasValue ? _task.LastRunTime.Value.ToShortTimeString() : "(none)";
				SubItems[3].Text = _task.NextRunTime.ToShortTimeString();
			}
		}
	}
}

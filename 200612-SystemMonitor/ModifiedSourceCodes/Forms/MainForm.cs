using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SystemMonitor.Monitors;
using SystemMonitor.Notifiers;
using SystemMonitor.Configuration;

namespace SystemMonitor.Forms
{
    public partial class MainForm : Form
    {
		private bool _closing;

        public MainForm()
        {
            InitializeComponent();
 			CreateGroups();
			CreateTasks();
			SetUpEventHandlers();

			notifyIcon.Icon = Icon;
			notifyIcon.Text = Application.ProductName;
		}

		private void CreateGroups()
		{
			foreach (string monitorType in Enum.GetNames(typeof(MonitorType)))
			{
				tasksListView.Groups.Add(new ListViewGroup(monitorType));
			}
		}

		private void CreateTasks()
		{
			MonitorSettingsSection section = (MonitorSettingsSection)ConfigurationManager.GetSection("monitorSettings");

			foreach (MonitorElement monitorElement in section.Monitors)
			{
				// IMonitor monitor = (IMonitor)Activator.CreateInstance(Type.GetType(monitorElement.TypeName));
                Type monitorType = Type.GetType(monitorElement.TypeName);
			    CheckTypeInstance(monitorType, monitorElement.TypeName);
			    IMonitor monitor = (IMonitor)Activator.CreateInstance(monitorType);
			    
				Dictionary<string, string> settings = new Dictionary<string, string>();

				foreach (SystemMonitor.Configuration.SettingElement settingElement in monitorElement.Settings)
				{
					settings.Add(settingElement.Name, settingElement.Value);
				}

				foreach (NotifierElement notifierElement in monitorElement.Notifiers)
				{
					// INotifier notifier = (INotifier)Activator.CreateInstance(Type.GetType(notifierElement.TypeName));
                    Type notifierType = Type.GetType(notifierElement.TypeName);
				    CheckTypeInstance(notifierType, notifierElement.TypeName);
                    INotifier notifier = (INotifier)Activator.CreateInstance(notifierType);
				    
				    monitor.Notifiers.Add(notifier);
                    
					Dictionary<string, string> notifierSettings = new Dictionary<string, string>();

					foreach (SystemMonitor.Configuration.SettingElement settingElement in notifierElement.Settings)
					{
						notifierSettings.Add(settingElement.Name, settingElement.Value);
					}

					notifier.Initialize(notifierSettings);
				}

				monitor.Initialize(monitorElement.RunFrequency, settings);

				ListViewItem lvi = new TaskListViewItem(new Task(monitor));
				tasksListView.SmallImageList.Images.Add(monitor.Icon);
				lvi.ImageIndex = tasksListView.SmallImageList.Images.Count - 1;
				lvi.Group = tasksListView.Groups[(int)monitor.MonitorType];
				tasksListView.Items.Add(lvi);
			}
		}

        private static void CheckTypeInstance(Type monitorType, string monitorName)
        {
            if (monitorType == null) 
            {
                throw new ApplicationException(string.Format("Class '{0}' has not been found. Check app.config.", monitorName)); 
            }
        }

        protected override void OnClosing(CancelEventArgs e)
		{
			if (!_closing)
			{
				e.Cancel = true;
				Hide();
			}
		}

        public void SetUpEventHandlers()
        {
            exitMenuItem.Click += delegate
            {
				_closing = true;
                Close();
            };

			showMenuItem.Click += delegate
			{
				Show();
			};

			notifyIcon.DoubleClick += delegate
			{
				Show();
				BringToFront();
			};

			tasksListView.SelectedIndexChanged += delegate
			{
				SetRunNowMenuItem();
			};

			runNowMenuItem.Click += delegate
			{
				ExecuteSelectedTask();
			};
        }

		private void SetRunNowMenuItem()
		{
			runNowMenuItem.Enabled = tasksListView.SelectedItems.Count == 1;

			if (tasksListView.SelectedItems.Count == 1)
			{
				TaskListViewItem listViewItem = ((TaskListViewItem)tasksListView.SelectedItems[0]);
				runNowMenuItem.Enabled = listViewItem.Monitor.MonitorType == MonitorType.Scheduled;
			}
		}

		private void ExecuteSelectedTask()
		{
			TaskListViewItem listViewItem = (TaskListViewItem)tasksListView.SelectedItems[0];
			listViewItem.Task.Run();
		}

		public void ShowBalloonTip(string title, string text, ToolTipIcon icon)
		{
			notifyIcon.ShowBalloonTip(2000, title, text, icon);
		}
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SystemMonitor.Forms;


namespace SystemMonitor
{
    static class Program
    {
		private static MainForm _mainForm;

        [STAThread]
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);
      
            Application.EnableVisualStyles();
			_mainForm = new MainForm();
            Application.Run(_mainForm);
        }

		public static void ShowBalloonTip(string title, string text, ToolTipIcon icon)
		{
			_mainForm.ShowBalloonTip(title, text,icon);
		}

        static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show(e.Message, "처리되지 않은 예외가 발생했습니다");
        }
    }
}
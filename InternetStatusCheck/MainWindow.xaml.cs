using System;
using System.Windows;
using System.Windows.Media;

namespace InternetStatusCheck
{
	using System.Net.NetworkInformation;
	using System.Threading;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Timer timer;

		public MainWindow()
		{
			InitializeComponent();
			
			timer = new Timer(this.CheckForInternet, null, 0, 10000);
		}

		void CheckForInternet(object o)
		{
			this.UpdateLastCheck("checking...");
			Thread.Sleep(1000);
			if (this.TestInternet())
			{
				this.Succuss();
			}

			this.UpdateLastCheck(DateTime.Now.ToString("T"));
		}

		bool TestInternet()
		{
			var pinger = new Ping();

			try
			{
				var reply = pinger.Send("www.google.com");
			
				return reply.Status == IPStatus.Success;
			}
			catch (Exception e)
			{
				// doesn't matter why we're here.  It didn't work, so you don't have internet.
				return false;
			}
		}

		void Succuss()
		{
			this.Dispatcher.Invoke( () =>
				{
					this.StatusLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 110));
					this.StatusLabel.Content = "Success";
					this.Alarm.Play();
					this.timer.Dispose();
				});
		}

		void UpdateLastCheck(string text)
		{
			this.Dispatcher.Invoke(() => this.LastCheckLabel.Content = text);
		}
	}
}

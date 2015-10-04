#region References

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestR.Extensions;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		#region Properties

		public string LogPath { get; set; }

		#endregion

		#region Methods

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			LogPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			Current.DispatcherUnhandledException += MainThreadExceptionHandler;
			AppDomain.CurrentDomain.UnhandledException += DomainExceptionHandler;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
		}

		private void DomainExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			var exception = e.ExceptionObject as Exception;
			if (exception != null)
			{
				File.WriteAllText(LogPath + Guid.NewGuid() + ".TestR.Error", exception.ToDetailedString());
			}
		}

		private void MainThreadExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			File.WriteAllText(LogPath + Guid.NewGuid() + ".TestR.Error", e.Exception.ToDetailedString());
			e.Handled = true;
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			File.WriteAllText(LogPath + Guid.NewGuid() + ".TestR.Error", e.Exception.ToDetailedString());
			e.SetObserved();
		}

		#endregion
	}
}
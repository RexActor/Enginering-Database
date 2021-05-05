using System;

//using System.Collections.Generic;
using System.Windows;

namespace Enginering_Database
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			base.OnStartup(e);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.ExceptionObject.ToString());
		}

		private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			//MessageBox.Show("An unhandled expection just occured:" + e.Exception.Message + " Stack Overflow: " +e.Exception.StackTrace , "App exception catcher - for unhandled exceptions", MessageBoxButton.OK, MessageBoxImage.Warning);
			//e.Handled = true;
		}
	}
}
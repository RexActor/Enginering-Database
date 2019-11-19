using Engineering_Database;
using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Threading;


//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
//[assembly: AssemblyVersion("2.10.*")]
namespace Enginering_Database
{


	public partial class MainWindow : Window
	{


		readonly string userName = WindowsIdentity.GetCurrent().Name;
		readonly UserSettings userSett = new UserSettings();

		public MainWindow()
		{





			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;



			DatabaseClass dtC = new DatabaseClass();


			Application currApp = Application.Current;
			currApp.ShutdownMode = ShutdownMode.OnLastWindowClose;





			InitializeComponent();
			OnStartupCheckMaintenance();
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			DateTime buildDate = new DateTime(2000, 1, 1)
									.AddDays(version.Build).AddSeconds(version.Revision * 2);
			string displayableVersion = $"{version}";
			VersionData.Content = displayableVersion;


			dtC.ConnectDB();

			userSett.openSettings();


			DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(MaintenanceActivation);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
			dispatcherTimer.Start();



			fileExistsLabelData.Content = dtC.DBStatus();





			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);





			LocalTimeData.Content = DateTime.Now.ToString("HH:mm:ss");


		}



		static void MyHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			UserErrorWindow userError = new UserErrorWindow();
			userError.errorMessage = e.Message;
			userError.CallWindow();

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

			addData addnew = new addData();
			addnew.ShowDialog();

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			viewDatabase win2 = new viewDatabase();

			win2.ShowDialog();
		}

		private void UpdateDatabaseButton_Click(object sender, RoutedEventArgs e)
		{


			if (userName == UserSettings.SubAdmin1 || userName == UserSettings.SubAdmin2 || userName == UserSettings.UserName)
			{

				updateDatabase updateDB = new updateDatabase();

				updateDB.ShowDialog();
			}
			else
			{

				PasswordRequest("UpdateDB");





			}
		}
		private void SettingsShow(object sender, RoutedEventArgs e)
		{
			UserSettings userSett = new UserSettings();

			if (userName != UserSettings.SubAdmin1 || userName != UserSettings.SubAdmin2 || userName != UserSettings.UserName)
			{

				PasswordRequest("Settings");
			}
			else
			{


				userSett.ShowDialog();
			}
		}





		public static void UpdateStat()
		{

		}


		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void PasswordRequest(string target)
		{

			passwordWindow passwordWindow = new passwordWindow();
			passwordWindow.targetWindow = target;
			passwordWindow.ShowDialog();


		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Application.Current.Shutdown();
		}

		public string GetCPU()
		{


			string cpu = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
			if (cpu.IndexOf("64") > 0)
			{
				cpu = "64 Bit";
			}
			else
			{
				cpu = "32 bit";
			}
			return cpu;

		}

		public void MaintenanceActivation(object sender, EventArgs e)
		{


			UserSettings userMaintenSett = new UserSettings();
			userMaintenSett.openSettings();



			if (userMaintenSett.Maintenance == "Yes")
			{
				MaintenanceLabel.Visibility = Visibility.Visible;
				MaintenanceLabel2.Visibility = Visibility.Visible;
				UpdateDatabaseButton.Visibility = Visibility.Hidden;
				UpdateDatabaseImage.Visibility = Visibility.Hidden;
				InsertDataButton.Visibility = Visibility.Hidden;
				InsertDataImage.Visibility = Visibility.Hidden;
				ViewDatabaseButton.Visibility = Visibility.Hidden;
				ViewDatabaseImage.Visibility = Visibility.Hidden;
				if (UserSettings.UserName != userName || UserSettings.SubAdmin1 != userName || UserSettings.SubAdmin2 != userName)
				{

					DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
					dispatcherTimer.Tick += new EventHandler(CloseApp);
					dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
					dispatcherTimer.Start();

				}
			}
			else
			{
				MaintenanceLabel.Visibility = Visibility.Hidden;
				MaintenanceLabel2.Visibility = Visibility.Hidden;
				UpdateDatabaseButton.Visibility = Visibility.Visible;
				UpdateDatabaseImage.Visibility = Visibility.Visible;
				InsertDataButton.Visibility = Visibility.Visible;
				InsertDataImage.Visibility = Visibility.Visible;
				ViewDatabaseButton.Visibility = Visibility.Visible;
				ViewDatabaseImage.Visibility = Visibility.Visible;
			}










		}
		public void OnStartupCheckMaintenance()
		{
			UserSettings userMaintenSett = new UserSettings();
			userMaintenSett.openSettings();
			if (userMaintenSett.Maintenance == "Yes")
			{
				MaintenanceLabel.Visibility = Visibility.Visible;
				MaintenanceLabel2.Visibility = Visibility.Visible;
				UpdateDatabaseButton.Visibility = Visibility.Hidden;
				UpdateDatabaseImage.Visibility = Visibility.Hidden;
				InsertDataButton.Visibility = Visibility.Hidden;
				InsertDataImage.Visibility = Visibility.Hidden;
				ViewDatabaseButton.Visibility = Visibility.Hidden;
				ViewDatabaseImage.Visibility = Visibility.Hidden;


			}
			else
			{
				MaintenanceLabel.Visibility = Visibility.Hidden;
				MaintenanceLabel2.Visibility = Visibility.Hidden;
				UpdateDatabaseButton.Visibility = Visibility.Visible;
				UpdateDatabaseImage.Visibility = Visibility.Visible;
				InsertDataButton.Visibility = Visibility.Visible;
				InsertDataImage.Visibility = Visibility.Visible;
				ViewDatabaseButton.Visibility = Visibility.Visible;
				ViewDatabaseImage.Visibility = Visibility.Visible;
			}
		}
		public void CloseApp(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ReportAppIssue_Click(object sender, RoutedEventArgs e)
		{
			ReportAppIssue repApp = new ReportAppIssue();
			repApp.Show();
		}
	}
}






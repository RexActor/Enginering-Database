using Engineering_Database;

using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;


//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
//[assembly: AssemblyVersion("2.10.*")]
namespace Enginering_Database
{


	public partial class MainWindow : Window
	{


		readonly string userName = WindowsIdentity.GetCurrent().Name;
		readonly UserSettings userSett = new UserSettings();
		UserSettings userMaintenSett = new UserSettings();
		public MainWindow()
		{





			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;



			DatabaseClass dtC = new DatabaseClass();


			Application currApp = Application.Current;
			currApp.ShutdownMode = ShutdownMode.OnLastWindowClose;


			dtC.ConnectDB();


			InitializeComponent();
			OnStartupCheckMaintenance();
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			DateTime buildDate = new DateTime(2000, 1, 1)
									.AddDays(version.Build).AddSeconds(version.Revision * 2);
			string displayableVersion = $"{version}";
			VersionData.Content = displayableVersion;




			//userSett.openSettings();


			DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(MaintenanceActivation);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
			dispatcherTimer.Start();


			if (dtC.DBStatus() == "DB connected")
			{
				fileExistsLabelData.Foreground = Brushes.Green;
			}
			else
			{
				fileExistsLabelData.Foreground = Brushes.Red;
			}
			fileExistsLabelData.Content = dtC.DBStatus();





			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

			DispatcherTimer clock = new DispatcherTimer();
			clock.Tick += new EventHandler(UpdateClock);
			clock.Interval = new TimeSpan(0, 0, 1);
			clock.Start();




			dtC.CloseDB();

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

			userSett.openSettings();
			if (userName == userSett.SubAdmin1 || userName == userSett.SubAdmin2 || userName == userSett.UserName)
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
			//UserSettings userSett = new UserSettings();
			userSett.openSettings();

			if (userName != userSett.SubAdmin1 || userName != userSett.SubAdmin2 || userName != userSett.UserName)
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



			userMaintenSett.openSettings();



			if (userMaintenSett.Maintenance == "Yes")
			{
				MaintenanceLabel.Visibility = Visibility.Visible;

				UpdateDatabaseButton.Visibility = Visibility.Hidden;
				UpdateDatabaseImage.Visibility = Visibility.Hidden;
				InsertDataButton.Visibility = Visibility.Hidden;
				InsertDataImage.Visibility = Visibility.Hidden;
				ViewDatabaseButton.Visibility = Visibility.Hidden;
				ViewDatabaseImage.Visibility = Visibility.Hidden;
				AssetListButton.Visibility = Visibility.Hidden;
				ShowReportButton.Visibility = Visibility.Hidden;
				ShowReportImage.Visibility = Visibility.Hidden;
				AssetListImage.Visibility = Visibility.Hidden;
				PasswordProtectImage1.Visibility = Visibility.Hidden;
				PasswordProtectImage2.Visibility = Visibility.Hidden;
				PasswordProtectImage3.Visibility = Visibility.Hidden;
				PasswordProtectImage4.Visibility = Visibility.Hidden;
				MeterReadingButton.Visibility = Visibility.Hidden;
				LineMaintenanceButton.Visibility = Visibility.Hidden;
				PasswordProtectImage5.Visibility = Visibility.Hidden;
				MaintenancePic.Visibility = Visibility.Hidden;

				if (userName != userMaintenSett.UserName || userName != userMaintenSett.SubAdmin1 || userName != userMaintenSett.SubAdmin2)
				{

					DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
					MaintenanceLabel2.Visibility = Visibility.Visible;
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
				AssetListButton.Visibility = Visibility.Visible;
				ShowReportButton.Visibility = Visibility.Visible;
				ShowReportImage.Visibility = Visibility.Visible;
				AssetListImage.Visibility = Visibility.Visible;
				PasswordProtectImage1.Visibility = Visibility.Visible;
				PasswordProtectImage2.Visibility = Visibility.Visible;
				PasswordProtectImage3.Visibility = Visibility.Visible;
				PasswordProtectImage4.Visibility = Visibility.Visible;
				MeterReadingButton.Visibility = Visibility.Visible;
				LineMaintenanceButton.Visibility = Visibility.Visible;
				PasswordProtectImage5.Visibility = Visibility.Visible;
				MaintenancePic.Visibility = Visibility.Visible;
			}










		}
		public void OnStartupCheckMaintenance()
		{
			//UserSettings userMaintenSett = new UserSettings();
			userMaintenSett.openSettings();
			//MessageBox.Show("*"+UserSettings.UserName.ToString() + "* ==> *" + userName+"*");
			if (userMaintenSett.Maintenance == "Yes")
			{
				MaintenanceLabel.Visibility = Visibility.Visible;
				//MaintenanceLabel2.Visibility = Visibility.Visible;
				UpdateDatabaseButton.Visibility = Visibility.Hidden;
				UpdateDatabaseImage.Visibility = Visibility.Hidden;
				InsertDataButton.Visibility = Visibility.Hidden;
				InsertDataImage.Visibility = Visibility.Hidden;
				ViewDatabaseButton.Visibility = Visibility.Hidden;
				ViewDatabaseImage.Visibility = Visibility.Hidden;
				AssetListButton.Visibility = Visibility.Hidden;
				ShowReportButton.Visibility = Visibility.Hidden;
				ShowReportImage.Visibility = Visibility.Hidden;
				AssetListImage.Visibility = Visibility.Hidden;
				PasswordProtectImage1.Visibility = Visibility.Hidden;
				PasswordProtectImage2.Visibility = Visibility.Hidden;
				PasswordProtectImage3.Visibility = Visibility.Hidden;
				PasswordProtectImage4.Visibility = Visibility.Hidden;
				MeterReadingButton.Visibility = Visibility.Hidden;
				LineMaintenanceButton.Visibility = Visibility.Hidden;
				PasswordProtectImage5.Visibility = Visibility.Hidden;
				MaintenancePic.Visibility = Visibility.Hidden;

				if (userName != userMaintenSett.UserName || userName != userMaintenSett.SubAdmin1 || userName != userMaintenSett.SubAdmin2)
				{

					DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
					MaintenanceLabel2.Visibility = Visibility.Visible;
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
				AssetListButton.Visibility = Visibility.Visible;
				ShowReportButton.Visibility = Visibility.Visible;
				ShowReportImage.Visibility = Visibility.Visible;
				AssetListImage.Visibility = Visibility.Visible;
				PasswordProtectImage1.Visibility = Visibility.Visible;
				PasswordProtectImage2.Visibility = Visibility.Visible;
				PasswordProtectImage3.Visibility = Visibility.Visible;
				PasswordProtectImage4.Visibility = Visibility.Visible;
				MeterReadingButton.Visibility = Visibility.Visible;
				LineMaintenanceButton.Visibility = Visibility.Visible;
				PasswordProtectImage5.Visibility = Visibility.Visible;
				MaintenancePic.Visibility = Visibility.Visible;
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

		private void UpdateClock(object sender, EventArgs e)
		{
			DateTime d;
			d = DateTime.Now;
			LocalTimeData.Content = d.ToString("HH:mm:ss");
		}

		private void ShowReportButtonClick(object sender, RoutedEventArgs e)
		{

			userSett.openSettings();
			if (userName == userSett.SubAdmin1 || userName == userSett.SubAdmin2 || userName == userSett.UserName)
			{

				ReportWindow repWin = new ReportWindow();
				repWin.Show();
			}
			else
			{

				PasswordRequest("Reports");

			}
			
		}

		private void AssetListButton_Click(object sender, RoutedEventArgs e)
		{

			userSett.openSettings();
			if (userName == userSett.SubAdmin1 || userName == userSett.SubAdmin2 || userName == userSett.UserName)
			{

				AssetList assetList = new AssetList();
				assetList.Show();
			}
			else
			{

				PasswordRequest("AssetList");

			}



		}

		private void MeterReadingButton_Click(object sender, RoutedEventArgs e)
		{
			userSett.openSettings();
			if (userName == userSett.SubAdmin1 || userName == userSett.SubAdmin2 || userName == userSett.UserName)
			{

				MeterReadings meterReadings = new MeterReadings();
				meterReadings.Show();
			}
			else
			{

				PasswordRequest("MeterReadings");

			}
		}

		private void LineMaintenanceButton_Click(object sender, RoutedEventArgs e)
		{
			userSett.openSettings();
			if (userName == userSett.SubAdmin1 || userName == userSett.SubAdmin2 || userName == userSett.UserName)
			{
				//LineMaintenance lineMaintenance = new LineMaintenance();
				//lineMaintenance.Show();
				DynamicMaintenanceTable dynamictable = new DynamicMaintenanceTable();
				
				dynamictable.Show();
			}
			else
			{

				PasswordRequest("LineMaintenance");

			}

		}
	}
}






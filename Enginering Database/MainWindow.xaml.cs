using Engineering_Database;

using System;
using System.Diagnostics;
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
		private LoginSystem loginSys = new LoginSystem();
		public int? loginSysID;
		private readonly string userName = WindowsIdentity.GetCurrent().Name;
		private readonly UserSettings userSett = new UserSettings();
		private UserSettings userMaintenSett = new UserSettings();
		private ErrorSystem err = new ErrorSystem();

		public MainWindow()
		{
			try
			{
				WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

				DatabaseClass dtC = new DatabaseClass();

				Application currApp = Application.Current;

				currApp.ShutdownMode = ShutdownMode.OnLastWindowClose;
				currApp.Exit += OnApplicationExit;

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

				//TODO:change
				loginSysID = loginSys.CreateUserLogin(userName);

				UserNameLabel.Content = userName;

				if (loginSysID != null && LoginIDLabel.Content.ToString() == "Not Assigned")
				{
					LoginIDLabel.Content = loginSysID.ToString();
				}
				UsersOnlineLabel.Content = loginSys.OnlineUserCount();

				dtC.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void OnApplicationExit(object sender, EventArgs e)
		{
			try
			{
				//TODO:change
				loginSys.UpdateUserLogin(Convert.ToInt32(loginSysID));
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		//TODO: remove ! not required

		private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			UserErrorWindow userError = new UserErrorWindow();
			userError.errorMessage = e.Message;
			userError.CallWindow();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				addData addnew = new addData();
				addnew.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				viewDatabase win2 = new viewDatabase();

				win2.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void SettingsShow(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
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
			try
			{
				passwordWindow passwordWindow = new passwordWindow();
				passwordWindow.targetWindow = target;
				passwordWindow.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			try
			{
				base.OnClosed(e);
				Application.Current.Shutdown();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		public string GetCPU()
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
				return null;
			}
		}

		public void MaintenanceActivation(object sender, EventArgs e)
		{
			try
			{
				userMaintenSett.openSettings();

				if (userMaintenSett.Maintenance == "Yes")
				{
					MaintenanceLabel.Visibility = Visibility.Visible;

					InsertDataButton.Visibility = Visibility.Hidden;
					InsertDataImage.Visibility = Visibility.Hidden;
					ViewDatabaseButton.Visibility = Visibility.Hidden;
					ViewDatabaseImage.Visibility = Visibility.Hidden;
					AdministratorButton.Visibility = Visibility.Hidden;
					AdministratorImage.Visibility = Visibility.Hidden;
					PasswordProtectImage2.Visibility = Visibility.Hidden;

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

					InsertDataButton.Visibility = Visibility.Visible;
					InsertDataImage.Visibility = Visibility.Visible;
					ViewDatabaseButton.Visibility = Visibility.Visible;
					ViewDatabaseImage.Visibility = Visibility.Visible;
					AdministratorButton.Visibility = Visibility.Visible;
					AdministratorImage.Visibility = Visibility.Visible;
					PasswordProtectImage2.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		public void OnStartupCheckMaintenance()
		{
			try
			{
				//UserSettings userMaintenSett = new UserSettings();
				userMaintenSett.openSettings();
				//MessageBox.Show("*"+UserSettings.UserName.ToString() + "* ==> *" + userName+"*");
				if (userMaintenSett.Maintenance == "Yes")
				{
					MaintenanceLabel.Visibility = Visibility.Visible;
					//MaintenanceLabel2.Visibility = Visibility.Visible;

					InsertDataButton.Visibility = Visibility.Hidden;
					InsertDataImage.Visibility = Visibility.Hidden;
					ViewDatabaseButton.Visibility = Visibility.Hidden;
					ViewDatabaseImage.Visibility = Visibility.Hidden;
					AdministratorButton.Visibility = Visibility.Hidden;
					AdministratorImage.Visibility = Visibility.Hidden;
					PasswordProtectImage2.Visibility = Visibility.Hidden;

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
					AdministratorButton.Visibility = Visibility.Visible;
					AdministratorImage.Visibility = Visibility.Visible;
					InsertDataButton.Visibility = Visibility.Visible;
					InsertDataImage.Visibility = Visibility.Visible;
					ViewDatabaseButton.Visibility = Visibility.Visible;
					ViewDatabaseImage.Visibility = Visibility.Visible;

					PasswordProtectImage2.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		public void CloseApp(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ReportAppIssue_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ReportAppIssue repApp = new ReportAppIssue();
				repApp.Show();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void UpdateClock(object sender, EventArgs e)
		{
			DateTime d;
			d = DateTime.Now;
			LocalTimeData.Content = d.ToString("HH:mm:ss");
		}

		private void AdministratorButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				userSett.openSettings();
				if (userName == userSett.SubAdmin1 || userName == userSett.SubAdmin2 || userName == userSett.UserName)
				{
					Admin admin = new Admin();
					admin.Show();
				}
				else
				{
					PasswordRequest("Admin");
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void Button_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
		}

		private void SecretButton_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				OnlineUsers onlineUsers = new OnlineUsers();
				onlineUsers.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void Window_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				if (SecretButton.IsEnabled == true)
				{
					SecretButton.IsEnabled = false;
				}
				else
				{
					SecretButton.IsEnabled = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}
	}
}
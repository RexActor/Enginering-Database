using Engineering_Database;
using System;
using System.Reflection;
//using System.IO;
using System.Windows;
//using System.Windows.Media;
//using System.Security.Permissions;
//using System.Windows.Threading;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
//[assembly: AssemblyVersion("2.10.*")]
namespace Enginering_Database
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	
	public partial class MainWindow : Window
	{

		//private static readonly log4net.ILog log = LogHelper.GetLogger();

		//readonly string userName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
		readonly string userName = Environment.UserName;
		//string userName = "Gatis";
		public MainWindow()
		{


			UserSettings userSett = new UserSettings();



			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;



			DatabaseClass dtC = new DatabaseClass();


			Application currApp = Application.Current;
			currApp.ShutdownMode = ShutdownMode.OnLastWindowClose;





			InitializeComponent();


			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			DateTime buildDate = new DateTime(2000, 1, 1)
									.AddDays(version.Build).AddSeconds(version.Revision * 2);
			string displayableVersion = $"{version}";
			VersionData.Content = displayableVersion;

			userSett.openSettings();
			dtC.ConnectDB();
			fileExistsLabelData.Content = dtC.DBStatus();


			//checkFile();

			/*
			if (userName == UserSettings.UserName || userName==UserSettings.SubAdmin1 || userName==UserSettings.SubAdmin2)
				{
				settingsOption.Visibility = Visibility.Visible;
				}
			else
				{

				settingsOption.Visibility = Visibility.Hidden;
				}
				*/

			//upLoadingData(viewDatabase.ldata);

			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

			//throw new Exception("2");

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
			//log.Debug("Test");
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
			//UserSettings userSett = new UserSettings();

			if (userName == UserSettings.SubAdmin1 || userName == UserSettings.SubAdmin2 || userName == UserSettings.UserName)
			{

				updateDatabase updateDB = new updateDatabase();

				updateDB.ShowDialog();
			}
			else
			{

				PasswordRequest("UpdateDB");


				/*else
				{


					UserErrorWindow userErrorMessage = new UserErrorWindow();
					userErrorMessage.errorMessage = "Damn... something went wrong. You don't have access to this...";
					userErrorMessage.Title = "User Access Error";
					userErrorMessage.CallWindow();
					userErrorMessage.ShowDialog();

				}*/


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

				//userSett.Owner = this;
				userSett.ShowDialog();
			}
		}




		//public void updateBar(int value)
		//{
		//	loadingData.Value = value;
		//}
		public static void UpdateStat()
		{

		}
		//public void upLoadingData(int value)
		//{
		//	loadingData.Value = loadingData.Value + value;
		//}

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
		//this.Close();
	}



}






using Engineering_Database;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Enginering_Database
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public partial class MainWindow : Window
	{

		//private static readonly log4net.ILog log = LogHelper.GetLogger();
		
		string userName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
		public MainWindow()
		{
			UserSettings userSett = new UserSettings();
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			DatabaseClass dtC = new DatabaseClass();

			dtC.ConnectDB();



			userSett.openSettings();


			InitializeComponent();

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

			upLoadingData(viewDatabase.ldata);



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
			UserSettings userSett = new UserSettings();
		
			if (userName == UserSettings.SubAdmin1 || userName == UserSettings.SubAdmin2 || userName == UserSettings.UserName)
			{

				updateDatabase updateDB = new updateDatabase();

				updateDB.ShowDialog();
			}
			else
			{
				UserErrorWindow userErrorMessage = new UserErrorWindow();
				userErrorMessage.errorMessage = "Damn... something went wrong. You don't have access to this...";
				userErrorMessage.Title = "User Access Error";
				userErrorMessage.CallWindow();
				userErrorMessage.ShowDialog();
			}
		}
		private void SettingsShow(object sender, RoutedEventArgs e)
		{
			UserSettings userSett = new UserSettings();

			if (userName != UserSettings.SubAdmin1 || userName != UserSettings.SubAdmin2 || userName != UserSettings.UserName)
			{

				PasswordRequest();
			}
			else
			{

				//userSett.Owner = this;
				userSett.ShowDialog();
			}
		}


		public void checkFile()
		{
			//database.xlsx

			String curfile = Directory.GetCurrentDirectory() + @"\database.xlsx";
			timeUpdatedLabelData.Content = System.IO.File.GetLastWriteTime(curfile).ToString("dd/MM/yy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

			timeUpdatedLabelData.Foreground = Brushes.Green;
			if (File.Exists(curfile))
			{
				fileExistsLabelData.Content = "Yes";
				fileExistsLabelData.Foreground = Brushes.Green;


			}
			else
			{
				fileExistsLabelData.Content = "NO";
				fileExistsLabelData.Foreground = Brushes.Red;
			}
			if (FileInfo(curfile) == true)
			{
				isReadOnlyLabelData.Content = "NO";
				isReadOnlyLabelData.Foreground = Brushes.Red;
			}
			else
			{
				isReadOnlyLabelData.Content = "Yes";
				isReadOnlyLabelData.Foreground = Brushes.Green;
			}

		}

		public static bool FileInfo(string fileName)
		{
			FileInfo fInfo = new FileInfo(fileName);
			return fInfo.IsReadOnly;
		}
		public void updateBar(int value)
		{
			loadingData.Value = value;
		}
		public static void UpdateStat()
		{

		}
		public void upLoadingData(int value)
		{
			loadingData.Value = loadingData.Value + value;
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void PasswordRequest()
		{

			passwordWindow passwordWindow = new passwordWindow();

			passwordWindow.ShowDialog();


		}

	}



}


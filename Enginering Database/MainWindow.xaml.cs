using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using Engineering_Database;


namespace Enginering_Database
	{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	
	public partial class MainWindow:Window
		{
		

		string userName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
		public MainWindow ( )
			{

			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			DatabaseClass dtC = new DatabaseClass();

			dtC.ConnectDB();
			

			UserSettings userSett = new UserSettings();
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

		private void Button_Click (object sender,RoutedEventArgs e)         
			{ 
			addData addnew = new addData();
			addnew.ShowDialog();
			}

		private void Button_Click_1 (object sender,RoutedEventArgs e)
			{
			viewDatabase win2 = new viewDatabase();
			
			win2.ShowDialog();
			}

		private void Button_Click_2 (object sender,RoutedEventArgs e)
			{
			updateDatabase updateDB = new updateDatabase();
			
			updateDB.ShowDialog();
			}
		private void SettingsShow (object sender,RoutedEventArgs e)
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
		

		public  void checkFile ( )
			{
			//database.xlsx
			
			String curfile = Directory.GetCurrentDirectory() +@"\database.xlsx";
			timeUpdatedLabelData.Content = System.IO.File.GetLastWriteTime(curfile).ToString("dd/MM/yy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

			timeUpdatedLabelData.Foreground = Brushes.Green;
			if(File.Exists(curfile)){
				fileExistsLabelData.Content = "Yes";
				fileExistsLabelData.Foreground = Brushes.Green;
				
			
				}
			else
				{
				fileExistsLabelData.Content = "NO";
				fileExistsLabelData.Foreground = Brushes.Red;
				}
			if(FileInfo(curfile) == true)
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

		public static bool FileInfo (string fileName)
			{
			FileInfo fInfo = new FileInfo(fileName);
			return fInfo.IsReadOnly;
			}
		public void updateBar (int value)
			{
			loadingData.Value = value;
			}
		public static void UpdateStat ( )
			{

			}
		public void upLoadingData (int value)
			{
			loadingData.Value = loadingData.Value + value;
			}

		private void MenuItem_Click (object sender,RoutedEventArgs e)
			{

			}

		private void PasswordRequest()
		{

			passwordWindow passwordWindow = new passwordWindow();
			
			passwordWindow.ShowDialog();


		}

		}

	

		}


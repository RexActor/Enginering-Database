using Enginering_Database;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Engineering_Database
{

	public partial class passwordWindow : Window
	{
		readonly UserSettings userSett = new UserSettings();
		readonly updateDatabase updateDatabase = new updateDatabase();
		readonly AssetList assetList = new AssetList();
		readonly Report reports = new Report();
		readonly MeterReadings meterReadings = new MeterReadings();
		readonly LineMaintenance lineMaintenance = new LineMaintenance();
		public string targetWindow;
		public passwordWindow()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			userSett.openSettings();
			InitializeComponent();
			passwordMessage.Visibility = Visibility.Hidden;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string pass = passwordWindowTextBox.Password.ToString();

			if (pass == UserSettings.password)
			{

				if (targetWindow == "UpdateDB")
				{
					this.Close();
					updateDatabase.Show();
				}
				else if (targetWindow == "Settings")
				{
					this.Close();
					userSett.Show();
				}
				else if (targetWindow == "AssetList")
				{
					this.Close();
					assetList.Show();
				}
				else if (targetWindow == "Reports")
				{
					this.Close();
					reports.Show();

				}
				else if (targetWindow == "MeterReadings")
				{
					this.Close();
					meterReadings.Show();

				}
				else if (targetWindow == "LineMaintenance")
				{
					this.Close();
					lineMaintenance.Show();
				}

			}
			else
			{
				passwordMessage.Foreground = Brushes.Red;

				passwordMessage.Visibility = Visibility.Visible;
				passwordWindowTextBox.Clear();

			}
		}

		private void passwordWindowTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				string pass = passwordWindowTextBox.Password.ToString();

				if (pass == UserSettings.password)
				{

					if (targetWindow == "UpdateDB")
					{
						this.Close();
						updateDatabase.ShowDialog();
					}
					else if (targetWindow == "Settings")
					{
						this.Close();
						userSett.ShowDialog();
					}
					else if (targetWindow == "AssetList")
					{
						this.Close();
						assetList.Show();
					}
					else if (targetWindow == "Reports")
					{
						this.Close();
						reports.Show();

					}
					else if (targetWindow == "MeterReadings")
					{
						this.Close();
						meterReadings.Show();

					}
					else if (targetWindow == "LineMaintenance")
					{
						this.Close();
						lineMaintenance.Show();
					}

				}
				else
				{
					passwordMessage.Foreground = Brushes.Red;

					passwordMessage.Visibility = Visibility.Visible;
					passwordWindowTextBox.Clear();

				}
			}

		}
	}
}

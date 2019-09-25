using Enginering_Database;
using System.Windows;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for passwordWindow.xaml
	/// </summary>
	public partial class passwordWindow : Window
	{
		readonly UserSettings userSett = new UserSettings();
		readonly updateDatabase updateDatabase = new updateDatabase();
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
					updateDatabase.ShowDialog();
				}
				else if (targetWindow == "Settings")
				{
					this.Close();
					userSett.ShowDialog();
				}

			}
			else
			{
				passwordMessage.Foreground = Brushes.Red;

				passwordMessage.Visibility = Visibility.Visible;
				passwordWindowTextBox.Clear();
				//MessageBox.Show("damn... password is wrong! Good try! But not this time");
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for passwordWindow.xaml
	/// </summary>
	public partial class passwordWindow : Window
	{
		UserSettings userSett = new UserSettings();
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

			if(pass == UserSettings.password)
			{
				this.Close();
				userSett.ShowDialog();
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

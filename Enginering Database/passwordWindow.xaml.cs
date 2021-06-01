using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Engineering_Database
{
	public partial class passwordWindow : Window
	{
		private readonly UserSettings userSett = new UserSettings();
		private readonly Admin admin = new Admin();
		private ErrorSystem err = new ErrorSystem();

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
			try
			{
				string pass = passwordWindowTextBox.Password.ToString();

				if (pass == UserSettings.password)
				{
					if (targetWindow == "Admin")
					{
						this.Close();
						admin.Show();
					}
					else if (targetWindow == "Settings")
					{
						this.Close();
						userSett.Show();
					}
				}
				else
				{
					passwordMessage.Foreground = Brushes.Red;

					passwordMessage.Visibility = Visibility.Visible;
					passwordWindowTextBox.Clear();
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void passwordWindowTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Return)
				{
					string pass = passwordWindowTextBox.Password.ToString();

					if (pass == UserSettings.password)
					{
						if (targetWindow == "Admin")
						{
							this.Close();
							admin.Show();
						}
						else if (targetWindow == "Settings")
						{
							this.Close();
							userSett.Show();
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
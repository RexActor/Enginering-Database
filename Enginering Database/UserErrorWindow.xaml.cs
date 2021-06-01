using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for UserErrorWindow.xaml
	/// </summary>
	public partial class UserErrorWindow : Window
	{
		//public string errorTitle;
		public string errorMessage;

		public bool shutDown = false;
		public string message;

		public UserErrorWindow()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

			InitializeComponent();
		}

		public void CallWindow()
		{
			ErrorTextBlock.Text = errorMessage;
		}

		public void CloseErrorMessage_click(object sender, RoutedEventArgs e)
		{
			switch (shutDown)
			{
				case true:

					Application.Current.Shutdown();
					break;

				case false:
					this.Hide();
					break;

				default:
					Application.Current.Shutdown();
					break;
			}
		}
	}
}
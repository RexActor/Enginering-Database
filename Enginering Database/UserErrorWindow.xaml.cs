using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for UserErrorWindow.xaml
	/// </summary>
	public partial class UserErrorWindow : Window
	{

		public string errorMessage;
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
			this.Close();
		}
	}
}

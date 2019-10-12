using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for SettingsForIssueCode.xaml
	/// </summary>
	public partial class SettingsForIssueCode : Window
	{
		public SettingsForIssueCode()
		{
			InitializeComponent();
		}

		private void SettingsAddNewArea_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SettingsRemoveArea_Click(object sender, RoutedEventArgs e)
		{

		}


		private void SettingsAddIssueTypeTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if(e.Key == System.Windows.Input.Key.Enter)
			{
				MessageBox.Show("Pressed Enter in Add Issue TextBox");
			}
		}

		private void SettingsAddFaultyAreaTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				MessageBox.Show("Pressed Enter in Add Faulty Area TextBox");
			}
		}

		private void SettingsAddIssueCodeTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				MessageBox.Show("Pressed Enter in Add Issue Code TextBox");
			}
		}
	}
}

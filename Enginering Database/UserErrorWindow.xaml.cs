using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

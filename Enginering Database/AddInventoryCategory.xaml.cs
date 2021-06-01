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
	/// Interaction logic for AddInventoryCategory.xaml
	/// </summary>
	public partial class AddInventoryCategory : Window
	{
		private DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();

		public AddInventoryCategory()
		{
			InitializeComponent();
		}

		private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (CategoryTextBox.Text != string.Empty)
				{
					db.ConnectDB();

					db.AddCategory("InventoryCategory", CategoryTextBox.Text);

					CategoryTextBox.Text = "";
					saveLabel.Visibility = Visibility.Visible;

					db.CloseDB();
				}
				else
				{
					errorLabel.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void CategoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (CategoryTextBox.Text != String.Empty)
				{
					saveLabel.Visibility = Visibility.Hidden;
				}
				errorLabel.Visibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
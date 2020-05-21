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
	/// Interaction logic for InventoryViewMeasureType.xaml
	/// </summary>
	public partial class InventoryViewMeasureType : Window
	{
		DatabaseClass db = new DatabaseClass();
		public InventoryViewMeasureType()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

			if (MeasureTypeTextBox.Text != string.Empty)
			{

				db.ConnectDB();

				db.AddMeasure("InventoryViewMeasureTypes", MeasureTypeTextBox.Text);
				db.CloseDB();

				MeasureTypeTextBox.Text = "";
				measureTypeSaved.Visibility = Visibility.Visible;

			}
			else
			{
				measureTypeError.Visibility = Visibility.Visible;
			}

		}

		private void MeasureTypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			measureTypeSaved.Visibility = Visibility.Hidden;
			measureTypeError.Visibility = Visibility.Hidden;
		}
	}
}

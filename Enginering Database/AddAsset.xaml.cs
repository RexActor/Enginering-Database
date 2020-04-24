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
	/// Interaction logic for AddAsset.xaml
	/// </summary>
	public partial class AddAsset : Window
	{
		public AddAsset()
		{
			InitializeComponent();
		}

		private void InsertAssetButton_Click(object sender, RoutedEventArgs e)
		{
			bool decomissioned;
			bool onSite;


			DatabaseClass db = new DatabaseClass();

			db.ConnectDB("Assets");

			if (decomissionedCheckBox.IsChecked==true)
			{
				decomissioned = true;
			}

			else
			{
				decomissioned = false;
			}
			if (onSiteCheckBox.IsChecked == true)
			{
				onSite = false;
			}
			else
			{
				onSite = true;
			}



			db.InsertAssetIntoDatabase("AssetList",AssetDescriptionTextBox.Text,AssetMakeTextBox.Text,AssetModelTextBox.Text,AssetNumberTextBox.Text,SerialNumberTextBox.Text,DateOfManufactureTextBox.Text,Convert.ToDateTime(DateOfInstallationDatePicker.SelectedDate.Value.Date),IssueLevelTextBox.Text,AssetInstalledOnTextBox.Text,decomissioned, onSite);

			db.CloseDB();


		}
	}
}

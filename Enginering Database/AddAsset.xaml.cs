using System;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AddAsset.xaml
	/// </summary>
	public partial class AddAsset : Window
	{
		private ErrorSystem err = new ErrorSystem();

		public AddAsset()
		{
			InitializeComponent();
		}

		private void InsertAssetButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool decomissioned;
				bool onSite;

				DatabaseClass db = new DatabaseClass();

				db.ConnectDB("Assets");

				if (decomissionedCheckBox.IsChecked == true)
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

				db.InsertAssetIntoDatabase("AssetList", AssetDescriptionTextBox.Text, AssetMakeTextBox.Text, AssetModelTextBox.Text, AssetNumberTextBox.Text, SerialNumberTextBox.Text, DateOfManufactureTextBox.Text, Convert.ToDateTime(DateOfInstallationDatePicker.SelectedDate.Value.Date), IssueLevelTextBox.Text, AssetInstalledOnTextBox.Text, decomissioned, onSite);

				db.CloseDB();
				this.Close();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
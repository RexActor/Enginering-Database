using Enginering_Database;

using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AssetDetail.xaml
	/// </summary>
	/// 

	public partial class AssetDetail : Window
	{
		public int assetDetailIDForDatabase { get; set; }
		DatabaseClass db = new DatabaseClass();
		public string foundAsset { get; set; }
		bool editMode = false;
		public AssetDetail()
		{
			InitializeComponent();
			AssetDetailEditModeLabel.Content = "{edit mode}";
			//UpdateAssetService();
		}

		private void AssetUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			if (!editMode)
			{
				editMode = true;
				AssetDetailEditModeLabel.Visibility = Visibility.Visible;
				AssetDetailMakeTextBox.IsReadOnly = false;
				AssetDetailModelTextBox.IsReadOnly = false;
				AssetDetailAssetNumberTextBox.IsReadOnly = true;
				AssetDetailSerialNumberTextBox.IsReadOnly = false;
				AssetDetailDateManufacturedTextBox.IsReadOnly = false;
				AssetDetailIssueLevelTextBox.IsReadOnly = false;
				AssetDetailInstalledOnTextBox.IsReadOnly = false;
				AssetDetailDescriptionTextBox.IsReadOnly = false;
				AssetDetailDatePicker.IsEnabled = true;
				AssetDetailMakeTextBox.Background = Brushes.PeachPuff;
				AssetDetailDescriptionTextBox.Background = Brushes.PeachPuff;
				AssetDetailModelTextBox.Background = Brushes.PeachPuff;
				AssetDetailAssetNumberTextBox.Background = Brushes.White;
				AssetDetailSerialNumberTextBox.Background = Brushes.PeachPuff;
				AssetDetailDateManufacturedTextBox.Background = Brushes.PeachPuff;
				AssetDetailIssueLevelTextBox.Background = Brushes.PeachPuff;
				AssetDetailInstalledOnTextBox.Background = Brushes.PeachPuff;
				AssetDetailDescriptionTextBox.Background = Brushes.PeachPuff;
				AssetDetailDatePicker.Background = Brushes.PeachPuff;
				AssetUpdateButton.Content = "Save Details";

			}
			else
			{
				editMode = false;
				AssetDetailEditModeLabel.Visibility = Visibility.Hidden;
				AssetDetailMakeTextBox.IsReadOnly = true;
				AssetDetailModelTextBox.IsReadOnly = true;
				AssetDetailAssetNumberTextBox.IsReadOnly = true;
				AssetDetailSerialNumberTextBox.IsReadOnly = true;
				AssetDetailDateManufacturedTextBox.IsReadOnly = true;
				AssetDetailIssueLevelTextBox.IsReadOnly = true;
				AssetDetailInstalledOnTextBox.IsReadOnly = true;
				AssetDetailDescriptionTextBox.IsReadOnly = true;
				AssetDetailDatePicker.IsEnabled = false;
				AssetDetailMakeTextBox.Background = Brushes.White;
				AssetDetailDescriptionTextBox.Background = Brushes.White;
				AssetDetailModelTextBox.Background = Brushes.White;
				AssetDetailAssetNumberTextBox.Background = Brushes.White;
				AssetDetailSerialNumberTextBox.Background = Brushes.White;
				AssetDetailDateManufacturedTextBox.Background = Brushes.White;
				AssetDetailIssueLevelTextBox.Background = Brushes.White;
				AssetDetailInstalledOnTextBox.Background = Brushes.White;
				AssetDetailDescriptionTextBox.Background = Brushes.White;
				AssetDetailDatePicker.Background = Brushes.White;
				AssetUpdateButton.Content = "Edit Details";
				db.ConnectDB("Assets");
				db.UpdateAsset("AssetList", "Description", assetDetailIDForDatabase, AssetDetailDescriptionTextBox.Text);
				db.UpdateAsset("AssetList", "Make", assetDetailIDForDatabase, AssetDetailMakeTextBox.Text);
				db.UpdateAsset("AssetList", "Model", assetDetailIDForDatabase, AssetDetailModelTextBox.Text);
				db.UpdateAsset("AssetList", "AssetNumber", assetDetailIDForDatabase, AssetDetailAssetNumberTextBox.Text);
				db.UpdateAsset("AssetList", "SerialNumber", assetDetailIDForDatabase, AssetDetailSerialNumberTextBox.Text);
				db.UpdateAsset("AssetList", "DateOfManufacture", assetDetailIDForDatabase, AssetDetailDateManufacturedTextBox.Text);
				db.UpdateAsset("AssetList", "DateOfInstallation", assetDetailIDForDatabase, AssetDetailDatePicker.SelectedDate.Value.Date);
				db.UpdateAsset("AssetList", "IssueLevel", assetDetailIDForDatabase, AssetDetailIssueLevelTextBox.Text);
				db.UpdateAsset("AssetList", "InstalledOn", assetDetailIDForDatabase, AssetDetailInstalledOnTextBox.Text);
			}
		}

		private void JobListForAssetDetails_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			updateDatabase update = new updateDatabase();
			IssueClass item = (IssueClass)JobListForAssetDetails.SelectedItem;
			update.canComplete = true;
			update.canSendEmail = true;
			update.canSubmit = true;
			update.canChangeDueDate = true;
			update.seperateWindow = true;

			if (item.Completed == true)
			{
				update.Frame3CompleteCheckBox.IsChecked = true;
			}
			else
			{
				update.Frame3CompleteCheckBox.IsChecked = false;
			}
			update.UpdateFrame2(item.JobNumber.ToString());
			update.AssignToDropDownBox.SelectedItem = item.AssignedTo;
			update.Show();
		}

		private void AssetServiceList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{

			ServiceClass serv = (ServiceClass)AssetServiceList.SelectedItem;

			ServiceReport _serviceReport = new ServiceReport();

			db.ConnectDB();
			byte[] buffer = null;
			string tempFile = System.IO.Path.GetTempFileName();

			var reader = db.GetPDFFileFromDatabase("LineMaintenance", "ID", serv.ID);
			while (reader.Read())
			{
				buffer = (byte[])reader["UploadedFile"];
			}
			using (FileStream fsStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				fsStream.Write(buffer, 0, buffer.Length);
			}

			_serviceReport.ServicePDFView.Navigate(tempFile);

			db.CloseDB();
			_serviceReport.ShowDialog();
		}






	}


}

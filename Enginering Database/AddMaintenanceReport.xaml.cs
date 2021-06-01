using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AddMaintenanceReport.xaml
	/// </summary>
	public partial class AddMaintenanceReport : System.Windows.Window
	{
		private DatabaseClass db = new DatabaseClass();
		private DatabaseClass db2 = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();
		private bool assetIsSelected = false;

		public AddMaintenanceReport()
		{
			InitializeComponent();
			//PDFBrowser.Navigate(new Uri("about:blank"));
			//InformationLabel.Content = "There are no preview file available";
			uploadDateDatePicker.SelectedDate = DateTime.Now.Date;
			uploadDateDatePicker.IsEnabled = false;
			dateErrorLabel.Visibility = Visibility.Hidden;
			uploadConfirmationLabel.Visibility = Visibility.Hidden;

			fileLocation.IsEnabled = false;
		}

		private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				uploadConfirmationLabel.Visibility = Visibility.Hidden;
				//InformationLabel.Visibility = Visibility.Visible;
				System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

				openFileDialog.Filter = "PDF Files (*.pdf) |*.pdf";
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				DialogResult dr = openFileDialog.ShowDialog();

				fileLocation.Text = openFileDialog.FileName;

				if (dr == System.Windows.Forms.DialogResult.OK)
				{
					//InformationLabel.Visibility = Visibility.Hidden;
					PDFBrowser.Navigate(openFileDialog.FileName);
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UploadFileToDatabase_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (assetIsSelected)
				{
					databaseFilePut(fileLocation.Text);
					PDFBrowser.Navigate(new Uri("about:blank"));
					LineOfMaintenance.Text = "";
					fileLocation.Text = "";
					assetListListView.Items.Clear();
					assetNumberTextBox.Text = "";

					AssetIDSelectedLabel.Content = " No Selection";
					AssetIDSelectedLabel.Foreground = Brushes.Red;

					AssetAssetNumberSelectedLabel.Content = " No Selection";
					AssetAssetNumberSelectedLabel.Foreground = Brushes.Red;

					//engineerCommentRichTextBox.Document.Blocks.Add(new Paragraph(new Run("")));
					engineerCommentRichTextBox.Document.Blocks.Clear();
					uploadConfirmationLabel.Visibility = Visibility.Visible;
				}
				else
				{
					assetSelectionError.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void databaseFilePut(string filePath)
		{
			try
			{
				string richText = new TextRange(engineerCommentRichTextBox.Document.ContentStart, engineerCommentRichTextBox.Document.ContentEnd).Text;
				db.ConnectDB();
				byte[] file;
				using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				{
					using (var reader = new BinaryReader(stream))
					{
						file = reader.ReadBytes((int)stream.Length);
						db.UploadFile("LineMaintenance", file, LineOfMaintenance.Text, Convert.ToDateTime(DateOfMaintenanceDatePicker.SelectedDate), Convert.ToDateTime(uploadDateDatePicker.SelectedDate), richText, AssetAssetNumberSelectedLabel.Content.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void DateOfMaintenanceDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			try
			{
				uploadConfirmationLabel.Visibility = Visibility.Hidden;
				if (DateOfMaintenanceDatePicker.SelectedDate.Value.Date > DateTime.Now.Date)
				{
					DateOfMaintenanceDatePicker.SelectedDate = DateTime.Now.Date;
					DateOfMaintenanceDatePicker.Background = Brushes.Red;
					dateErrorLabel.Visibility = Visibility.Visible;
				}
				else
				{
					DateOfMaintenanceDatePicker.Background = Brushes.Transparent;
					dateErrorLabel.Visibility = Visibility.Hidden;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void assetNumberTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			try
			{
				assetIsSelected = false;
				assetListListView.Items.Clear();
				AssetIDSelectedLabel.Content = " No Selection";
				AssetIDSelectedLabel.Foreground = Brushes.Red;

				AssetAssetNumberSelectedLabel.Content = " No Selection";
				AssetAssetNumberSelectedLabel.Foreground = Brushes.Red;

				db.ConnectDB("Assets");

				var reader = db.DBQueryForAssetsWithFilterOrderByID("AssetList", "AssetNumber", assetNumberTextBox.Text);

				while (reader.Read())
				{
					Assets retreivedAsset = new Assets();

					retreivedAsset.ID = Convert.ToInt32(reader["ID"]);
					retreivedAsset.Description = reader["Description"].ToString();
					retreivedAsset.AssetNumber = reader["AssetNumber"].ToString();

					assetListListView.Items.Add(retreivedAsset);
				}
				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void assetListListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				assetIsSelected = true;
				assetSelectionError.Visibility = Visibility.Hidden;

				Assets selectedAsset = (Assets)assetListListView.SelectedItem;
				if (assetIsSelected)
				{
					AssetIDSelectedLabel.Content = selectedAsset.ID.ToString();
					AssetIDSelectedLabel.Foreground = Brushes.Green;

					AssetAssetNumberSelectedLabel.Content = selectedAsset.AssetNumber.ToString();
					AssetAssetNumberSelectedLabel.Foreground = Brushes.Green;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
using Enginering_Database;
using System.Windows;


namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AssetDetail.xaml
	/// </summary>
	/// 

	public partial class AssetDetail : Window
	{
		public string assetDetailIDCont2 { get; set; }
		public AssetDetail()
		{
			InitializeComponent();

		}

		private void AssetUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Not Implemented Yet");
		}

		private void JobListForAssetDetails_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			updateDatabase update = new updateDatabase();
			IssueClass item = (IssueClass)JobListForAssetDetails.SelectedItem;
			update.UpdateFrame2(item.JobNumber.ToString());
			update.CollectSelectedData((int)item.JobNumber);
			update.canComplete = true;
			update.canSendEmail = true;
			update.canSubmit = true;
			update.canChangeDueDate = true;
			update.seperateWindow = true;
			update.Show();
		}
	}
}

using System;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Input;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AssetList.xaml
	/// </summary>
	public partial class AssetList : Window
	{
		private bool haveInstallationDate = false;
		public AssetList()
		{
			InitializeComponent();
			createListView();
		}

		public void createListView()
		{


			CollectAssetData();

		}
		public void CollectAssetData()
		{
			DatabaseClass assetDB = new DatabaseClass();
			assetDB.ConnectDB("Assets");


			OleDbDataReader reader = assetDB.DBQueryForAssets("AssetList");

			while (reader.Read())
			{
				Assets assets = new Assets();
				assets.AssetNumber = reader["AssetNumber"].ToString();
				assets.Description = reader["Description"].ToString();
				assets.Model = reader["Model"].ToString();
				assets.ID = (int)reader["ID"];
				assets.Make = reader["Make"].ToString();
				assets.SerialNumber = reader["SerialNumber"].ToString();
				assets.DateofManufacture = reader["DateOfManufacture"].ToString();
				if (reader["DateOfInstallation"] !=DBNull.Value)
				{
					
					assets.DateofInstallation = Convert.ToDateTime(reader["DateOfInstallation"]);
				
				}
				assets.IssueLevel = reader["IssueLevel"].ToString();
				assets.InstalledOn = reader["InstalledOn"].ToString();
				assets.Decomissioned = (bool)reader["Decomissioned"];
				assets.OnSite = (bool)reader["OnSite"];

				AssetListView.Items.Add(assets);

			}
			assetDB.CloseDB();

		}

		private void AssetListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//var selectedItem = AssetListView.SelectedItem as DataRowView;

			//System.Windows.MessageBox.Show(selectedItem["ID"].ToString());
			string decomissioned;
			string onSite;

			Assets item = (Assets)AssetListView.SelectedItem;

			AssetDetail assetDetail = new AssetDetail();

			if (item.Decomissioned.ToString() == "False")
			{
				decomissioned = "No";
			}
			else
			{
				decomissioned = "Yes";
			}

			if (item.OnSite.ToString() == "False")
			{
				onSite = "Yes";
			}
			else
			{
				onSite = "No";
			}

			//assetDetail.assetDetailIDCont2 = item.AssetNumber.ToString();


			assetDetail.AssetDetailId.Content = item.ID.ToString();
			assetDetail.AssetDetailDescription.Content = item.Description.ToString();
			assetDetail.AssetDetailMake.Content = item.Make.ToString();
			assetDetail.AssetDetailModel.Content = item.Model.ToString();
			assetDetail.AssetDetailAssetNumber.Content = item.AssetNumber.ToString();
			assetDetail.AssetDetailSerialNumber.Content = item.SerialNumber.ToString();
			assetDetail.AssetDetailDateOfManufacture.Content = item.DateofManufacture.ToString();

			if (item.DateofInstallation.ToString() =="01-Jan-01 12:00:00 AM")
			{
				assetDetail.AssetDetailDateOfInstalation.Content = "Date Not Set";
				
			}
			else
			{
				assetDetail.AssetDetailDateOfInstalation.Content = item.DateofInstallation.ToString();
			}
				assetDetail.AssetDetailIssueLevel.Content = item.IssueLevel.ToString();
			assetDetail.AssetDetailInstalledOn.Content = item.InstalledOn.ToString();
			assetDetail.AssetDetailDecomissioned.Content = decomissioned;
			assetDetail.AssetDetailOnSite.Content = onSite;


			DatabaseClass jobDB = new DatabaseClass();
			jobDB.ConnectDB();


			OleDbDataReader reader = jobDB.DBQueryForAssetsWithFilter("engineeringDatabaseTable", "AssetNumber", item.AssetNumber.ToString());


			while (reader.Read())
			{
				IssueClass issue = new IssueClass();


				issue.JobNumber = (int)reader["JobNumber"];
				issue.DetailedDescription = reader["DetailedDescription"].ToString();
				issue.ReportedDate = string.Format("{0:d}", reader["ReportedDate"].ToString());
				issue.CompletedByUsername = reader["CompletedBy"].ToString();
				issue.Action = reader["Action"].ToString();

				assetDetail.JobListForAssetDetails.Items.Add(issue);

			}



			jobDB.CloseDB();
			assetDetail.Show();


		}

		private void AddAssetButton_Click(object sender, RoutedEventArgs e)
		{
			AddAsset addAsset = new AddAsset();
			addAsset.Show();
		}
	}
}

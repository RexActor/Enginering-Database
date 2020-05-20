using CrystalDecisions.CrystalReports.Engine;
using DocumentFormat.OpenXml.Drawing.Charts;
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
#pragma warning disable CS0414 // The field 'AssetList.haveInstallationDate' is assigned but its value is never used
		private bool haveInstallationDate = false;
#pragma warning restore CS0414 // The field 'AssetList.haveInstallationDate' is assigned but its value is never used
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
				if (reader["DateOfInstallation"] != DBNull.Value)
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

			//refactored to use textbox for information showing.
			//when will need to edit asset details, texbot will be enabled, as by default texboxes are disabled

			assetDetail.assetDetailIDForDatabase = item.ID;
			assetDetail.foundAsset = item.AssetNumber;
			assetDetail.AssetDetailId.Content = item.ID.ToString();
			assetDetail.AssetDetailMakeTextBox.Text = item.Make.ToString();
			assetDetail.AssetDetailModelTextBox.Text = item.Model.ToString();
			assetDetail.AssetDetailAssetNumberTextBox.Text = item.AssetNumber.ToString();
			assetDetail.AssetDetailSerialNumberTextBox.Text = item.SerialNumber.ToString();
			assetDetail.AssetDetailDateManufacturedTextBox.Text = item.DateofManufacture.ToString();
			assetDetail.AssetDetailIssueLevelTextBox.Text = item.IssueLevel.ToString();
			assetDetail.AssetDetailInstalledOnTextBox.Text = item.InstalledOn.ToString();
			assetDetail.AssetDetailDescriptionTextBox.Text = item.Description.ToString();
			assetDetail.AssetDetailDecomissioned.Content = decomissioned;
			assetDetail.AssetDetailOnSite.Content = onSite;

			if (item.DateofInstallation.ToString() == "01-Jan-01 12:00:00 AM" || item.DateofInstallation.ToString() == "01-Jan-01 00:00:00 AM")
			{


			}
			else
			{
				assetDetail.AssetDetailDatePicker.SelectedDate = item.DateofInstallation;

			}

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
				issue.Completed = (bool)reader["Completed"];
				issue.AssignedTo = reader["AssignedTo"].ToString();
				assetDetail.JobListForAssetDetails.Items.Add(issue);

			}
			jobDB.CloseDB();

			ServiceClass service = new ServiceClass();
			//ServiceData service = new ServiceData();
			DatabaseClass serviceDatabase = new DatabaseClass();
			serviceDatabase.ConnectDB();


			var readerForService = serviceDatabase.GetPDFFileFromDatabase("LineMaintenance", "LinkedAsset", item.AssetNumber.ToString());
			while (readerForService.Read())
			{
				//serviceData.Add(new ServiceData(Convert.ToInt32(reader["ID"]), Convert.ToDateTime(reader["DateOfMaintenance"])));
				service.ID = Convert.ToInt32(readerForService["ID"]);
				service.ServiceDate =String.Format("{0:d}", readerForService["DateOfMaintenance"]);
				assetDetail.AssetServiceList.Items.Add(service);
			}

			serviceDatabase.CloseDB();

			assetDetail.Show();
		}

		public struct ServiceData
		{
			//public ServiceData(int _id, DateTime _serviceDate)
			//{
			//	ID = _id;
			//	ServiceDate = _serviceDate;
			//}

			public int ID { get; set; }
			public string ServiceDate { get; set; }

		}



		private void AddAssetButton_Click(object sender, RoutedEventArgs e)
		{
			AddAsset addAsset = new AddAsset();
			addAsset.Show();
		}
	}
}

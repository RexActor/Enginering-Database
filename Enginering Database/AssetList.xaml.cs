using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AssetList.xaml
	/// </summary>
	public partial class AssetList : Window
	{
		
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
				//assets.DateofInstallation = Convert.ToDateTime(reader["DateOfInstallation"]);
				assets.IssueLevel = reader["IssueLevel"].ToString();
				assets.InstalledOn = reader["InstalledOn"].ToString();
				assets.Decomissioned = (bool)reader["Decomissioned"];
				assets.OnSite = (bool)reader["OnSite"];

				AssetListView.Items.Add(assets);

			}


		}

		private void AssetListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//var selectedItem = AssetListView.SelectedItem as DataRowView;

			//System.Windows.MessageBox.Show(selectedItem["ID"].ToString());


			Assets item = (Assets)AssetListView.SelectedItem;

			System.Windows.MessageBox.Show(item.ID.ToString());

		}
	}
}


using DocumentFormat.OpenXml.Office.CustomUI;

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for InventoryOverView.xaml
	/// </summary>
	public partial class InventoryOverView : Window
	{

		DatabaseClass db = new DatabaseClass();
		public InventoryOverView()
		{
			InitializeComponent();
			LoadInvnetoryListView();
		}

		private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}


		public void LoadInvnetoryListView()
		{
			db.ConnectDB();

			Console.WriteLine(db.DBStatus());

			var reader = db.GetAllPDFIds("InventoryView");


			while (reader.Read())
			{

				Inventory inv = new Inventory();


				inv.ID = Convert.ToInt32(reader["ID"]);
				inv.Product = reader["Product"].ToString();
				inv.Qty = Convert.ToInt32(reader["Qty"]);
				inv.MeasureType = reader["MeasureType"].ToString();
				inv.ProductCategory = reader["ProductCategory"].ToString();
				inv.Comment = reader["Comment"].ToString();

				if (inv.Qty >0)
				{
					ListviewInventory.Items.Add(inv);
				}
				else
				{
					ListviewInventoryNotOnStock.Items.Add(inv);
				}

			}


			



			db.CloseDB();


		}

	

	}
}

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
		private DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();

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
			try
			{
				db.ConnectDB();

				Console.WriteLine(db.DBStatus());

				var reader = db.GetAllPDFIds("InventoryViewProducts");

				while (reader.Read())
				{
					Inventory inv = new Inventory();

					inv.ID = Convert.ToInt32(reader["ID"]);
					inv.Product = reader["ProductName"].ToString();
					inv.MeasureType = reader["MeasureType"].ToString();
					inv.ProductCategory = reader["ProductCategory"].ToString();

					var getProduct = db.GetInventoryProduct("InventoryView", "Product", inv.Product);

					while (getProduct.Read())
					{
						inv.Qty = Convert.ToInt32(getProduct["Qty"]);
					}

					if (inv.Qty > 0)
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
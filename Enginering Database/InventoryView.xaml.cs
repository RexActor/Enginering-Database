using System;
using System.IO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for InventoryView.xaml
	/// </summary>
	public partial class InventoryView : Window
	{
		DatabaseClass db = new DatabaseClass();
		int foundId = 0;
		bool havePic = false;
		public InventoryView()
		{
			InitializeComponent();

			UpdateCategoryList();
		}

		private void InventoryViewListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UploadPicBorder.Visibility = Visibility.Hidden;
			qtyOnStockTextBox.Text = "";
			InventoryViewComment.Document.Blocks.Clear();
			checkboxChange();
			UpdateError.Visibility = Visibility.Hidden;


			if (InventoryViewListBox.SelectedItem != null)
			{



				db.ConnectDB();


				byte[] buffer = null;
				string tempFile = Path.GetTempFileName();
				var getProdductInfo = db.GetPDFFileFromDatabase("InventoryViewProducts", "ProductName", ((Inventory)InventoryViewListBox.SelectedItem).Product);

				while (getProdductInfo.Read())
				{

					if (getProdductInfo["ProductImage"] == DBNull.Value)
					{
						havePic = false;
					}
					else
					{
						havePic = true;
						buffer = (byte[])getProdductInfo["ProductImage"];
					}
					MeasureTypeLabelContent.Content = getProdductInfo["MeasureType"].ToString();






				}

				if (havePic)
				{
					using (FileStream fsStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
					{
						fsStream.Write(buffer, 0, buffer.Length);
					}

					ProductImage.Source = new BitmapImage(new Uri(tempFile));
				}
				else
				{
					//MessageBox.Show("Don't have pic");

					string path = AppDomain.CurrentDomain.BaseDirectory;


					Console.WriteLine(Path.Combine(path, "defaultImage.png"));

					ProductImage.Source = new BitmapImage(new Uri(Path.Combine(path, "defaultImage.png")));
				}

				var getInventoryViewForProduct = db.GetInventoryProduct2Fields("InventoryView", "Product", ((Inventory)InventoryViewListBox.SelectedItem).Product, "ProductCategory", InventoryCategoryComboBox.SelectedItem.ToString());

				while (getInventoryViewForProduct.Read())
				{
					qtyOnStockTextBox.Text = getInventoryViewForProduct["Qty"].ToString();
					InventoryViewComment.Document.Blocks.Add(new Paragraph(new Run(getInventoryViewForProduct["Comment"].ToString())));
				}



				db.CloseDB();
			}
		}

		private void addProductButton_Click(object sender, RoutedEventArgs e)
		{
			AddInventoryViewProduct addProduct = new AddInventoryViewProduct();
			addProduct.ShowDialog();
		}

		private void UpdateInventory_Click(object sender, RoutedEventArgs e)
		{

			if (InventoryViewListBox.SelectedItem != null)
			{
				if (qtyTakenTextBox.Text != string.Empty)
				{
					if (Convert.ToInt32(qtyLeftTextBox.Text) > 0)
					{
						int id = findProduct(((Inventory)InventoryViewListBox.SelectedItem).Product);

						db.ConnectDB();

						string textToUpdate = new TextRange(InventoryViewComment.Document.ContentStart, InventoryViewComment.Document.ContentEnd).Text;
						MessageBox.Show(textToUpdate);

						db.UpdateInventoryView("InventoryView", "Comment", id, textToUpdate);
						db.UpdateInventoryView("InventoryView", "Qty", id, Convert.ToInt32(qtyLeftTextBox.Text));

						var reader = db.GetInventoryProduct2Fields("InventoryView", "Product", ((Inventory)InventoryViewListBox.SelectedItem).Product.ToString(), "ProductCategory", InventoryCategoryComboBox.SelectedItem.ToString());

						while (reader.Read())
						{
							qtyOnStockTextBox.Text = reader["Qty"].ToString();
						}

						qtyLeftTextBox.Text = "";
						qtyTakenTextBox.Text = "";
						InventoryViewComment.Document.Blocks.Clear();

						UpdateError.Content = "Database updated";
						UpdateError.Foreground = Brushes.Green;
						UpdateError.Visibility = Visibility.Visible;

						db.CloseDB();
					}
					else
					{
						UpdateError.Content = "Can't leave negative stock levels";
						UpdateError.Foreground = Brushes.Red;
						UpdateError.Visibility = Visibility.Visible;
					}
				}
				else
				{
					UpdateError.Content = "Please type taken quantity";
					UpdateError.Foreground = Brushes.Red;
					UpdateError.Visibility = Visibility.Visible;
				}
			}
			else
			{

				UpdateError.Content = "Please Select product";
				UpdateError.Foreground = Brushes.Red;
				UpdateError.Visibility = Visibility.Visible;
			}

		}

		private void UpdateInventoryList()

		{


			if (InventoryCategoryComboBox.SelectedIndex > 0)
			{
				InventoryViewListBox.Items.Clear();
				db.ConnectDB();

				var reader = db.GetInventoryProduct("InventoryViewProducts", "ProductCategory", InventoryCategoryComboBox.SelectedItem.ToString());

				while (reader.Read())
				{
					Inventory _inventory = new Inventory();
					_inventory.ID = Convert.ToInt32(reader["ID"]);
					_inventory.Product = reader["ProductName"].ToString();

					InventoryViewListBox.Items.Add(_inventory);

				}





				db.CloseDB();
			}
			else
			{
				InventoryViewListBox.Items.Clear();
			}


		}

		private void addQuantityCheckBox_Click(object sender, RoutedEventArgs e)
		{


			if (addQuantityCheckBox.IsChecked == true)
			{
				if (InventoryViewListBox.SelectedItem != null)
				{
					addQuantityCheckBox.Foreground = Brushes.Green;
					addQuantityCheckBox.FontSize = 15;
					addQuantityCheckBox.FontWeight = FontWeights.Bold;
					addQuantityCheckBox.Content = "Adding Quantity in stock";
					qtyOnStockTextBox.IsEnabled = true;
					qtyOnStockTextBox.Background = Brushes.PeachPuff;
					qtyOnStockTextBox.BorderBrush = Brushes.Red;
					qtyOnStockTextBox.BorderThickness = new Thickness(2, 2, 2, 2);

				}
				else
				{

					if (addQuantityCheckBox.IsChecked == true)
					{
						addQuantityCheckBox.IsChecked = false;
					}

					addQuantityCheckBox.Foreground = Brushes.Red;
					addQuantityCheckBox.FontSize = 12;
					addQuantityCheckBox.FontWeight = FontWeights.Bold;
					addQuantityCheckBox.Content = "Please select product";



				}
			}
			else
			{
				addQuantityCheckBox.Foreground = Brushes.Black;
				addQuantityCheckBox.FontSize = 12;
				addQuantityCheckBox.FontWeight = FontWeights.Normal;
				addQuantityCheckBox.Content = "Add quantity in stock";
				qtyOnStockTextBox.IsEnabled = false;
				qtyOnStockTextBox.Background = Brushes.Transparent;
				qtyOnStockTextBox.BorderBrush = Brushes.Gray;
				qtyOnStockTextBox.BorderThickness = new Thickness(1, 1, 1, 1);
			}


		}

		private void AddQtyButton_Click(object sender, RoutedEventArgs e)
		{
			if (addQuantityCheckBox.IsChecked == true)
			{

				int alreadyExistingQty = 0;


				if (findProduct(((Inventory)InventoryViewListBox.SelectedItem).Product) != 0)
				{
					db.ConnectDB();
					var getAlreadyExistingQty = db.GetInventoryProduct2Fields("InventoryView", "Product", ((Inventory)InventoryViewListBox.SelectedItem).Product.ToString(), "ProductCategory", InventoryCategoryComboBox.SelectedItem.ToString());


					while (getAlreadyExistingQty.Read())
					{
						alreadyExistingQty = Convert.ToInt32(getAlreadyExistingQty["Qty"].ToString());
					}


					int value = Convert.ToInt32(qtyOnStockTextBox.Text) + alreadyExistingQty;
					db.UpdateInventoryView("InventoryView", "Qty", foundId, value);

					db.CloseDB();

				}
				else
				{
					int value = Convert.ToInt32(qtyOnStockTextBox.Text);

					addProductToInventory(((Inventory)InventoryViewListBox.SelectedItem).Product, value);
				}
				db.ConnectDB();


				var reader = db.GetInventoryProduct2Fields("InventoryView", "Product", ((Inventory)InventoryViewListBox.SelectedItem).Product.ToString(), "ProductCategory", InventoryCategoryComboBox.SelectedItem.ToString());

				while (reader.Read())
				{
					qtyOnStockTextBox.Text = reader["Qty"].ToString();
				}

				addQuantityCheckBox.IsChecked = false;
				qtyOnStockTextBox.IsEnabled = false;
				qtyOnStockTextBox.Background = Brushes.Transparent;
				qtyOnStockTextBox.BorderBrush = Brushes.Gray;
				qtyOnStockTextBox.BorderThickness = new Thickness(1, 1, 1, 1);
				db.CloseDB();

				addQuantityCheckBox.IsChecked = false;
				addQuantityCheckBox.Foreground = Brushes.Black;
				addQuantityCheckBox.FontSize = 12;
				addQuantityCheckBox.FontWeight = FontWeights.Normal;
				addQuantityCheckBox.Content = "Add quantity in stock";

			}
		}

		private int findProduct(string productName)
		{



			db.ConnectDB();

			var reader = db.GetAllPDFIds("InventoryView");

			while (reader.Read())
			{
				if (productName == reader["Product"].ToString())
				{
					foundId = Convert.ToInt32(reader["ID"]);
				}
			}

			db.CloseDB();

			return foundId;
		}

		private void UpdateCategoryList()
		{
			db.ConnectDB();

			var reader = db.GetAllPDFIds("InventoryCategory");
			InventoryCategoryComboBox.Items.Add("Please Select");

			while (reader.Read())
			{
				InventoryCategoryComboBox.Items.Add(reader["Category"].ToString());

			}
			InventoryCategoryComboBox.SelectedIndex = 0;




			db.CloseDB();

		}

		private void addProductToInventory(string Product, int Qty)
		{
			db.ConnectDB();

			db.AddProduct("InventoryView", Product, Qty, MeasureTypeLabelContent.Content.ToString(), InventoryCategoryComboBox.SelectedItem.ToString());



			db.CloseDB();
		}

		private void InventoryCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateInventoryList();
			checkboxChange();
		}

		private void qtyOnStockTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void qtyTakenTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			int value = 0;
			int stockQty = 0;
			int takenQty = 0;

			if (qtyOnStockTextBox.Text != string.Empty && qtyTakenTextBox.Text != string.Empty)
			{
				takenQty = Convert.ToInt32(qtyTakenTextBox.Text);
				stockQty = Convert.ToInt32(qtyOnStockTextBox.Text);
				value = stockQty - takenQty;
				qtyLeftTextBox.Text = value.ToString();

				if (value < 0)
				{
					qtyLeftTextBox.BorderBrush = Brushes.Red;
					qtyLeftTextBox.Foreground = Brushes.Red;
					qtyLeftTextBox.FontWeight = FontWeights.Black;
					qtyLeftTextBox.BorderThickness = new Thickness(2, 2, 2, 2);
				}
				else
				{
					qtyLeftTextBox.BorderBrush = Brushes.Green;
					qtyLeftTextBox.Foreground = Brushes.Green;
					qtyLeftTextBox.FontWeight = FontWeights.Black;
					qtyLeftTextBox.BorderThickness = new Thickness(2, 2, 2, 2);
				}


			}
			else
			{
				qtyLeftTextBox.Text = "";
			}
		}

		private void checkboxChange()
		{
			if (addQuantityCheckBox.IsChecked == true)
			{
				addQuantityCheckBox.IsChecked = false;
				addQuantityCheckBox.Foreground = Brushes.Black;
				addQuantityCheckBox.FontSize = 12;
				addQuantityCheckBox.FontWeight = FontWeights.Normal;
				addQuantityCheckBox.Content = "Add quantity in stock";
				qtyOnStockTextBox.IsEnabled = false;
				qtyOnStockTextBox.Background = Brushes.Transparent;
				qtyOnStockTextBox.BorderBrush = Brushes.Gray;
				qtyOnStockTextBox.BorderThickness = new Thickness(1, 1, 1, 1);
			}
		}

		private void InventoryOverView_Click(object sender, RoutedEventArgs e)
		{
			InventoryOverView inventOverView = new InventoryOverView();
			inventOverView.ShowDialog();
		}

		private void ProductImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{

			if (InventoryViewListBox.SelectedItem != null)
			{
				if (UploadPicBorder.Visibility == Visibility.Hidden)
				{
					UploadPicBorder.Visibility = Visibility.Visible;
				}
				else
				{
					UploadPicBorder.Visibility = Visibility.Hidden;
				}
			}
		}

		private void UploadPicBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (InventoryViewListBox.SelectedItem != null)
			{
				UploadPicBorder.Visibility = Visibility.Visible;
			}
		}

		private void ImageButtonClick_Click(object sender, RoutedEventArgs e)
		{
			if (InventoryViewListBox.SelectedItem != null)
			{

				UpdatePicture updatePic = new UpdatePicture();
				updatePic.ProductIDLabel.Content = ((Inventory)InventoryViewListBox.SelectedItem).ID.ToString();
				updatePic.ProductCategory.Content = InventoryCategoryComboBox.SelectedItem.ToString();
				updatePic.ProductMeasureType.Content = MeasureTypeLabelContent.Content.ToString();
				updatePic.ProductNameLabel.Content = ((Inventory)InventoryViewListBox.SelectedItem).Product;
				updatePic.newImage.Source = ProductImage.Source;

				updatePic.ShowDialog();


			}
		}

		private void RequestProductButton_Click(object sender, RoutedEventArgs e)
		{

			if (InventoryViewListBox.SelectedItem != null)
			{

				if (int.TryParse(requestQtyTextBox.Text, out int c))
				{

					try
					{
						EmailClass email = new EmailClass();
						string dateNow = String.Format("{0:d}",DateTime.Now.Date);
						 string userName = WindowsIdentity.GetCurrent().Name;
						email.RequestProduct(dateNow, userName, ((Inventory)InventoryViewListBox.SelectedItem).Product.ToString(),requestQtyTextBox.Text,MeasureTypeLabelContent.Content.ToString());

						informationLabel.Content = "Request has been sent";
						informationLabel.Foreground = Brushes.Green;
						informationLabel.FontWeight = FontWeights.Bold;
						informationLabel.Visibility = Visibility.Visible;



					}
					catch
					{
						informationLabel.Content = "Request hasn't been sent";
						informationLabel.Foreground = Brushes.Red;
						informationLabel.FontWeight = FontWeights.Bold;
						informationLabel.Visibility = Visibility.Visible;
					}
				}

				else
				{
					informationLabel.Content = "Can't send request because it's not number";
					informationLabel.Foreground = Brushes.Red;
					informationLabel.FontWeight = FontWeights.Bold;
					informationLabel.Visibility = Visibility.Visible;
				}
			}
			else
			{
				informationLabel.Content = "Please select product";
				informationLabel.Foreground = Brushes.Red;
				informationLabel.FontWeight = FontWeights.Bold;
				informationLabel.Visibility = Visibility.Visible;
			}

		}

		private void requestQtyTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!int.TryParse(requestQtyTextBox.Text, out int c))
			{
				informationLabel.Content = "Please enter number";
				informationLabel.Foreground = Brushes.Red;
				informationLabel.FontWeight = FontWeights.Bold;
				informationLabel.Visibility = Visibility.Visible;
			}
			else
			{
				informationLabel.Visibility = Visibility.Hidden;
			}

		}
	}
}

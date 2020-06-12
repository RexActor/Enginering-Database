using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AddInventoryViewProduct.xaml
	/// </summary>
	public partial class AddInventoryViewProduct : Window
	{
		DatabaseClass db = new DatabaseClass();
		private bool fileChosen = false;
		private bool productNameAssigned = false;
		private bool measureTypeChosen = false;
		private bool categoryChosed = false;
		public AddInventoryViewProduct()
		{
			InitializeComponent();
			UpdateMeasureTypeComboBox();
			UpdateCategory();
		}

		private void AddProductButton_Click(object sender, RoutedEventArgs e)
		{

			if (productNameAssigned == true && measureTypeChosen == true && categoryChosed == true)
			{

				//uploading data into database




				db.ConnectDB();

				if (fileChosen == true)
				{
					byte[] file;

					using (var stream = new FileStream(ImageLocation.Text, FileMode.Open, FileAccess.Read))
					{
						using (var reader = new BinaryReader(stream))
						{

							file = reader.ReadBytes((int)stream.Length);
							db.UploadInventoryProduct("InventoryViewProducts", file, ProdoductNameTextBox.Text, MeasureTypeComboBox.SelectedItem.ToString(), Category.SelectedItem.ToString());


						}

					}
				}
				else
				{
					db.UploadInventoryProductWithoutPic("InventoryViewProducts", ProdoductNameTextBox.Text, MeasureTypeComboBox.SelectedItem.ToString(), Category.SelectedItem.ToString());
				}

				infoErrorMessage.Visibility = Visibility.Visible;
				infoErrorMessage.Content = "File Uploaded";
				infoErrorMessage.Foreground = Brushes.Green;

				ProdoductNameTextBox.Text = "";
				MeasureTypeComboBox.SelectedIndex = 0;
				ImageLocation.Text = "";
				ProductImage.Source = null;

				db.CloseDB();

				//once uploaded showing that it's completed 
			}
			else
			{
				//showing error message
				infoErrorMessage.Visibility = Visibility.Visible;
				infoErrorMessage.Content = "Please Fill all fields";
				infoErrorMessage.Foreground = Brushes.Red;


			}

		}

		private void AddMeasureTypeButton_Click(object sender, RoutedEventArgs e)
		{
			InventoryViewMeasureType addMeasure = new InventoryViewMeasureType();
			addMeasure.ShowDialog();

		}
		private void UpdateMeasureTypeComboBox()
		{
			MeasureTypeComboBox.Items.Clear();


			db.ConnectDB();

			var reader = db.GetAllPDFIds("InventoryViewMeasureTypes");
			MeasureTypeComboBox.Items.Add("Please Select");
			while (reader.Read())
			{

				MeasureTypeComboBox.Items.Add(reader["MeasureType"].ToString());
			}
			MeasureTypeComboBox.SelectedIndex = 0;

			db.CloseDB();

		}

		private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
		{
			//	informationLabel.Visibility = Visibility.Hidden;
			infoErrorMessage.Visibility = Visibility.Hidden;
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

			openFileDialog.Filter = "Image Files (*.jpg;*.png) |*.jpg;*.png";
			openFileDialog.FilterIndex = 1;
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			DialogResult dr = openFileDialog.ShowDialog();

			if (dr == System.Windows.Forms.DialogResult.OK)
			{
				fileChosen = true;
				ImageLocation.Text = openFileDialog.FileName;
				ProductImage.Source = new BitmapImage(new Uri(ImageLocation.Text));
			}
			else
			{
				fileChosen = false;
			}
		}

		private void ProdoductNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{

			if (ProdoductNameTextBox.Text != String.Empty)
			{
				productNameAssigned = true;
			}
			else
			{
				productNameAssigned = false;
			}
		}

		private void MeasureTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (MeasureTypeComboBox.SelectedIndex > 0)
			{
				measureTypeChosen = true;
			}
			else
			{
				measureTypeChosen = false;
			}
		}

		private void AddCategory_Click(object sender, RoutedEventArgs e)
		{
			AddInventoryCategory addCategory = new AddInventoryCategory();
			addCategory.ShowDialog();
		}

		private void UpdateCategory()
		{
			db.ConnectDB();

			var reader = db.GetAllPDFIds("InventoryCategory");
			Category.Items.Add("Please Select");

			while (reader.Read())
			{
				Category.Items.Add(reader["Category"].ToString());

			}
			Category.SelectedIndex = 0;

			db.CloseDB();
		}

		private void Category_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (Category.SelectedIndex > 0)
			{
				categoryChosed = true;
			}
			else
			{
				categoryChosed = false;
			}
		}
	}
}

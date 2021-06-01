using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Vml.Spreadsheet;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for UpdatePicture.xaml
	/// </summary>
	public partial class UpdatePicture : Window
	{
		private bool newFileChosen = false;
		private DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();

		public UpdatePicture()
		{
			InitializeComponent();
			ProductIDLabel.Content = "asd";
		}

		private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//	informationLabel.Visibility = Visibility.Hidden;

				System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

				openFileDialog.Filter = "Image Files (*.jpg;*.png) |*.jpg;*.png";
				openFileDialog.FilterIndex = 1;
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				DialogResult dr = openFileDialog.ShowDialog();

				if (dr == System.Windows.Forms.DialogResult.OK)
				{
					newFileChosen = true;
					chosenImageLocation.Text = openFileDialog.FileName;
					newImage.Source = new BitmapImage(new Uri(chosenImageLocation.Text));
				}
				else
				{
					newFileChosen = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (newFileChosen)
				{
					db.ConnectDB();

					byte[] file;

					using (var stream = new FileStream(chosenImageLocation.Text, FileMode.Open, FileAccess.Read))
					{
						using (var reader = new BinaryReader(stream))
						{
							file = reader.ReadBytes((int)stream.Length);
							db.UpdateProductPicture("InventoryViewProducts", "ProductImage", file, Convert.ToInt32(ProductIDLabel.Content));
						}
					}

					InfoLabel.Foreground = Brushes.Green;
					InfoLabel.Content = "Picture Updated";
					InfoLabel.Visibility = Visibility.Visible;

					db.CloseDB();
				}
				else
				{
					InfoLabel.Foreground = Brushes.Red;
					InfoLabel.Content = "New Picture not chosen. Still the same image";
					InfoLabel.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
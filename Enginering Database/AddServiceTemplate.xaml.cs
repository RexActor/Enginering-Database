using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for AddServiceTemplate.xaml
	/// </summary>
	public partial class AddServiceTemplate : Window
	{
		private bool fileChosen = false;
		private bool templateNameProvided = false;
		DatabaseClass db = new DatabaseClass();
		public AddServiceTemplate()
		{
			InitializeComponent();
		}

		private void chooseTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			informationLabel.Visibility = Visibility.Hidden;

			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

			openFileDialog.Filter = "PDF Files (*.pdf) |*.pdf";
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			DialogResult dr = openFileDialog.ShowDialog();

			if (dr == System.Windows.Forms.DialogResult.OK)
			{
				fileChosen = true;
				templateLocation.Text = openFileDialog.FileName;
				ServicePDFView.Navigate(openFileDialog.FileName);
			}
			else
			{
				fileChosen = false;
			}
		}

		private void nameOfTemplate_TextChanged(object sender, TextChangedEventArgs e)
		{

			informationLabel.Visibility = Visibility.Hidden;

			if (nameOfTemplate.Text == string.Empty)
			{
				templateNameProvided = false;
			}
			else
			{
				templateNameProvided = true;
			}
		}

		private void saveTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			if (templateNameProvided && fileChosen)
			{
				db.ConnectDB();
				byte[] file;
				using (var stream = new FileStream(templateLocation.Text, FileMode.Open, FileAccess.Read))
				{
					using (var reader = new BinaryReader(stream))
					{
						file = reader.ReadBytes((int)stream.Length);
						db.uploadTemplateFile("ServiceTemplate", file, nameOfTemplate.Text);
					}
				}

				informationLabel.Foreground = Brushes.Green;
				informationLabel.Content = "File uploaded.";
				informationLabel.Visibility = Visibility.Visible;

				ServicePDFView.Navigate(new Uri("about:blank"));
				nameOfTemplate.Text = "";
				templateLocation.Text = "";
				fileChosen = false;
				templateNameProvided = false;

			}
			else
			{
				informationLabel.Foreground = Brushes.Red;
				informationLabel.Content = "File wasn't uploaded.";
				informationLabel.Visibility = Visibility.Visible;
			}
		}
	}
}

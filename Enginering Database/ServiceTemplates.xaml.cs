using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for ServiceTemplates.xaml
	/// </summary>
	public partial class ServiceTemplates : Window
	{
		DatabaseClass db = new DatabaseClass();
		public ServiceTemplates()
		{
			InitializeComponent();
			loadtemplates();
		}

		private void templateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (templateList.SelectedItem != null)
			{
				tempDeleteErrorLabel.Visibility = Visibility.Hidden;
				db.ConnectDB();
				byte[] buffer = null;
				string tempFile = System.IO.Path.GetTempFileName();

				var reader = db.GetPDFFileFromDatabase("ServiceTemplate", "TemplateName", templateList.SelectedItem.ToString());
				while (reader.Read())
				{
					buffer = (byte[])reader["TemplateFile"];

				}
				using (FileStream fsStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
				{
					fsStream.Write(buffer, 0, buffer.Length);
				}


				ServicePDFView.Navigate(tempFile);
				db.CloseDB();
			}

		}

		private void addNewTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			AddServiceTemplate addTemplate = new AddServiceTemplate();

			addTemplate.ShowDialog();
		}


		private void loadtemplates()
		{
			templateList.Items.Clear();
			db.ConnectDB();

			var reader = db.GetAllPDFIds("ServiceTemplate");

			while (reader.Read())
			{
				templateList.Items.Add(reader["TemplateName"].ToString());
			}


			db.CloseDB();

		}

		private void refreshTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			loadtemplates();

		}

		private void DeleteTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			if (templateList.SelectedItem != null)
			{
				db.ConnectDB();
				db.DeleteTemplate("ServiceTemplate", templateList.SelectedItem.ToString());
				ServicePDFView.Navigate(new Uri("about:blank"));
				loadtemplates();
			}
			else
			{
				tempDeleteErrorLabel.Visibility = Visibility.Visible;
			}
		}
	}
}

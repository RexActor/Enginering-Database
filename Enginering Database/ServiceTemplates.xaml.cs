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
		private DatabaseClass db = new DatabaseClass();

		private ErrorSystem err = new ErrorSystem();

		public ServiceTemplates()

		{
			InitializeComponent();
			loadtemplates();
		}

		private void templateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void addNewTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddServiceTemplate addTemplate = new AddServiceTemplate();

				addTemplate.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void loadtemplates()
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void refreshTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				loadtemplates();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void DeleteTemplateButton_Click(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
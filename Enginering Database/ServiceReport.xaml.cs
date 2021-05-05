using System.IO;
using System.Windows;
using System;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for ServiceReport.xaml
	/// </summary>
	public partial class ServiceReport : Window
	{
		public int serviceID { get; set; }
		private DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();

		public ServiceReport()
		{
			InitializeComponent();
			//UpdatePDFViewer();
		}

		private void UpdatePDFViewer()
		{
			try
			{
				db.ConnectDB();
				byte[] buffer = null;
				string tempFile = System.IO.Path.GetTempFileName();

				var reader = db.GetPDFFileFromDatabase("LineMaintenance", "ID", serviceID);
				while (reader.Read())
				{
					buffer = (byte[])reader["UploadedFile"];
				}
				using (FileStream fsStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
				{
					fsStream.Write(buffer, 0, buffer.Length);
				}

				ServicePDFView.Navigate(tempFile);

				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}
	}
}
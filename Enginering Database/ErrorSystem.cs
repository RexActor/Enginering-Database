using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace Engineering_Database
{
	internal class ErrorSystem
	{
		private string fileName;
		private UserErrorWindow userError = new UserErrorWindow();
		private readonly string userName = WindowsIdentity.GetCurrent().Name;

		public int loginID { get; set; }

		public void ReadClassData(LoginSystem log)
		{
		}

		public void RecordError(string message, string stackTrace)
		{
			string sourceDirectory = Directory.GetCurrentDirectory();
			string destinationDirectory = $"{ sourceDirectory}\\Backup";

			fileName = $"{sourceDirectory}\\Log.txt";
			using (StreamWriter w = File.AppendText(fileName))
			{
				w.WriteLine($"------ Log for User Login ID: {loginID} START-----\n");
				w.WriteLine($"Date/Time: {DateTime.Now}");
				w.WriteLine($"User: {userName}");
				w.WriteLine($"Error Message: {message}");
				w.WriteLine($"Stack Trace: {stackTrace}");
				w.WriteLine("\n---------------- LOG END -------------------\n");
				//w.Close();
			}
			//MessageBox.Show("Error recorded", "Error/Information", MessageBoxButton.OK, MessageBoxImage.Warning);
			userError.errorMessage = $"There was something wrong!" +
				$"\nIt has been recorded and will be checked!" +
				$"\nPlease avoid this step/action (which caused error) until further notice." +
				$"\nThanks for your support and sorry for inconvenience caused.";
			userError.CallWindow();
			userError.Topmost = true;
			userError.Show();
		}
	}
}
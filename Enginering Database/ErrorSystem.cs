using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
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

		public void RecordError(string message, string stackTrace, string source)
		{
			string sourceDirectory = Directory.GetCurrentDirectory();
			string destinationDirectory = $"{ sourceDirectory}\\Backup";
			string messageToPass;

			fileName = $"{sourceDirectory}\\Log.txt";
			using (StreamWriter w = File.AppendText(fileName))
			{
				w.WriteLine($"------ Log for User Login ID: {loginID} START-----\n");
				w.WriteLine($"Date/Time: {DateTime.Now}");
				w.WriteLine($"User: {userName}");
				w.WriteLine($"Error Message: {message}");
				w.WriteLine($"Stack Trace: {stackTrace}");
				w.WriteLine($"Source: {source}");
				w.WriteLine("\n---------------- LOG END -------------------\n");
				//w.Close();
			}
			//MessageBox.Show("Error recorded", "Error/Information", MessageBoxButton.OK, MessageBoxImage.Warning);

			switch (message)
			{
				case "The Microsoft Access database engine cannot find the input table or query 'GlobalSettings'. Make sure it exists and that its name is spelled correctly.":
				case "Cannot set Visibility or call Show, ShowDialog, or WindowInteropHelper.EnsureHandle after a Window has closed.":
					userError.shutDown = true;

					messageToPass = $"There was something wrong!" +
				$"\nIt has been recorded and will be checked!" +
				$"\nDue this Application will shutdown once \"ok\" is pressed." +
				$"\nThanks for your support and sorry for inconvenience caused.";

					break;

				default:

					userError.shutDown = false;

					messageToPass = $"There was something wrong!" +
				$"\nIt has been recorded and will be checked!" +
				$"\nPlease avoid this step/action (which caused error) until further notice." +
				$"\nThanks for your support and sorry for inconvenience caused.";
					break;
			}

			userError.errorMessage = messageToPass;
			userError.CallWindow();
			userError.message = message;

			userError.Topmost = true;
			userError.Show();
		}
	}
}
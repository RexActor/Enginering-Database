using System.Diagnostics;
namespace Engineering_Database
{
	class EmailClass
	{

		UserSettings userSett = new UserSettings();
		UserErrorWindow userErr = new UserErrorWindow();
		private bool checkOutlook()
		{
			int procCount = 0;
			Process[] processlist = Process.GetProcessesByName("OUTLOOK");
			foreach (Process theprocess in processlist)
			{
				procCount++;
			}
			if (procCount > 0)
			{
				
				return true;
			}
			else
			{
				System.Diagnostics.Process.Start("OUTLOOK.EXE");

				return true;
			}
		}

		public void SendEmail()
		{

			if (checkOutlook() == true)
			{


				string emailAddress = userSett.Email;
				//var url = "mailto:gatis.jansons@ipl-ltd.com";
				//System.Diagnostics.Process.Start(url);
				Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
				
				Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
				//Microsoft.Office.Interop.Outlook.Accounts accounts = app.Session.Accounts;
				//Microsoft.Office.Interop.Outlook.Account acc = null;


				/*
				foreach (Microsoft.Office.Interop.Outlook.Account account in accounts)
				{
					if (account.SmtpAddress.Equals("bobsters.lol@gmail.com", StringComparison.CurrentCultureIgnoreCase))
					{
						acc = account;
						break;
					}

				}
				*/
				//mailItem.SendUsingAccount = acc;
				mailItem.Subject = "Test";
				mailItem.To = emailAddress;
				mailItem.Body = "Testing email";

				mailItem.Display(true);

				//mailItem.Send();

			}
			else
			{
				userErr.errorMessage = "Outlook is not open.Please open outlook aplication and try again";
				userErr.Title = "Outlook error";
				userErr.CallWindow();
				userErr.ShowDialog();
			}

		}


	}
}

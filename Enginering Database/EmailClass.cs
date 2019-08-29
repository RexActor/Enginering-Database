using System;

namespace Engineering_Database
{
	class EmailClass
	{

		public void SendEmail()
		{

			//var url = "mailto:gatis.jansons@ipl-ltd.com";
			//System.Diagnostics.Process.Start(url);
			Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
			Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
			Microsoft.Office.Interop.Outlook.Accounts accounts = app.Session.Accounts;
			Microsoft.Office.Interop.Outlook.Account acc = null;



			foreach (Microsoft.Office.Interop.Outlook.Account account in accounts)
			{
				if (account.SmtpAddress.Equals("bobsters.lol@gmail.com", StringComparison.CurrentCultureIgnoreCase))
				{
					acc = account;
					break;
				}

			}

			mailItem.SendUsingAccount = acc;
			mailItem.Subject = "Test";
			mailItem.To = "bobsters.lol@gmail.com";
			mailItem.Body = "Testing email";

			//mailItem.Display(true);

			mailItem.Send();



		}


	}
}

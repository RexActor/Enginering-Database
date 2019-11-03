using Microsoft.Office.Interop.Outlook;
using System.Diagnostics;

namespace Engineering_Database
{
	class EmailClass
	{

		readonly UserSettings userSett = new UserSettings();
		readonly UserErrorWindow userErr = new UserErrorWindow();
		readonly DatabaseClass db = new DatabaseClass();


		string htmlString;
		public string sender = null;
		//IssueClass iss = new IssueClass();
		//data
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

		public void SendEmail(string jobStatus = null, IssueClass issue = null, string reply = null)
		{



			if (checkOutlook() == true)
			{


				string emailAddress = userSett.Email;
				//var url = "mailto:gatis.jansons@ipl-ltd.com";


				//System.Diagnostics.Process.Start(url);
				Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();

				Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
				//sender = mailItem.SenderEmailAddress;

				//sender = getSenderEmailAddress(mailItem);
				//sender = mailItem.SenderEmailType;
				//sender = mailItem.SendUsingAccount.ToString();
				//sender = GetSenderSMTPAddress(mailItem);
				///sender = "gatis.jansons@ipl-ltd.com";

				sender = GetSenderEmailAddress(mailItem);

				//sender = mailItem.UserProperties.Session.CurrentUser.Address;




				issue.ReporterEmail = sender;







				if (reply == "Complete")
				{


					mailItem.To = issue.ReportedEmail;
					mailItem.CC = emailAddress;
					mailItem.Subject = "Your reported issue : " + issue.Type + " with " + issue.Priority + " priority is completed and closed";


					htmlString = "<html><body><h3> " + issue.ReportedUserName + " reported <b>" + issue.Type + " issue for:</b></h3>" +
				"<b>Priority:</b> " + issue.Priority + "<br>" +
"<b>Job Number:</b> " + issue.JobNumber + "<br>" +
"<b> Area: </b>" + issue.Area + "<br>" +
"<b>Building:</b> " + issue.Building + "<br>" +
"<b>Issue Code:</b> " + issue.Code + "<br>" +
"<b> Asset Number: </b>" + issue.AssetNumber + "<br>" +
"	<b>Faulty Area:</b> " + issue.FaulyArea + "<br>" +
"					<b>Your report:</b> " + issue.DetailedDescription + "<br>" +
"<b> Was Assigned To : </b>" + issue.AssignedTo + "<br>" +
" <b> Engineer Comment: </b>" + issue.CommentsForActionsTaken + "" +
"					</body>" +
"</html>";
				}
				else if (reply == "Report")
				{
					mailItem.To = emailAddress;
					mailItem.CC = sender;
					mailItem.Subject = "Engineering Attention required for: " + issue.Type + " with " + issue.Priority + " priority";
					//issue.JobNumber;
					htmlString = "<html><body><h3> " + issue.ReportedUserName + " reported <b>" + issue.Type + " issue for:</b></h3>" +
						"<b>Priority:</b> " + issue.Priority + "<br>" +
	  "<b>Job Number:</b> " + issue.JobNumber + "<br>" +
	  "<b> Area: </b>" + issue.Area + "<br>" +
   "<b>Building:</b> " + issue.Building + "<br>" +
   "<b>Issue Code:</b> " + issue.Code + "<br>" +
   "<b> Asset Number: </b>" + issue.AssetNumber + "<br>" +
   "	<b>Faulty Area:</b> " + issue.FaulyArea + "<br>" +
   "					<b>Your report:</b> " + issue.DetailedDescription + "" +
   "<br><h3>Please action reported issue ASAP. As issue needs to be completed by " + issue.DueDate + "</h3>	" +
   "					</body>" +
   "</html>";



					//body = issue.JobNumber.ToString();
				}
				else if (reply == "ReOpen")
				{
					mailItem.To = issue.ReportedEmail;
					mailItem.CC = emailAddress;
					mailItem.Subject = "Your reported issue : " + issue.Type + " with " + issue.Priority + " priority was reopened";
					htmlString = "<html><body><h3> " + issue.ReportedUserName + " reported <b>" + issue.Type + " issue for:</b></h3>" +
				"<b>Priority:</b> " + issue.Priority + "<br>" +
"<b>Job Number:</b> " + issue.JobNumber + "<br>" +
"<b> Area: </b>" + issue.Area + "<br>" +
"<b>Building:</b> " + issue.Building + "<br>" +
"<b>Issue Code:</b> " + issue.Code + "<br>" +
"<b> Asset Number: </b>" + issue.AssetNumber + "<br>" +
"	<b>Faulty Area:</b> " + issue.FaulyArea + "<br>" +
"					<b>Your report:</b> " + issue.DetailedDescription + "<br>" +
"<b> Is Assigned To : </b>" + issue.AssignedTo + "<br>" +
"<b> Due Date: </b>" + issue.DueDate + "<br>" +
" <b> Engineer Comment: </b>" + issue.CommentsForActionsTaken + "" +
"					</body>" +
"</html>";
				}

				else if (reply == null)
				{
					mailItem.To = issue.ReportedEmail;
					mailItem.CC = emailAddress;
					mailItem.Subject = "Your reported issue : " + issue.Type + " with " + issue.Priority + " priority was updated";
					htmlString = "<html><body><h3> " + issue.ReportedUserName + " reported <b>" + issue.Type + " issue for:</b></h3>" +
				"<b>Priority:</b> " + issue.Priority + "<br>" +
"<b>Job Number:</b> " + issue.JobNumber + "<br>" +
"<b> Area: </b>" + issue.Area + "<br>" +
"<b>Building:</b> " + issue.Building + "<br>" +
"<b>Issue Code:</b> " + issue.Code + "<br>" +
"<b> Asset Number: </b>" + issue.AssetNumber + "<br>" +
"	<b>Faulty Area:</b> " + issue.FaulyArea + "<br>" +
"					<b>Your report:</b> " + issue.DetailedDescription + "<br>" +
"<b> Is Assigned To : </b>" + issue.AssignedTo + "<br>" +
"<b> Due Date: </b>" + issue.DueDate + "<br>" +
" <b> Engineer Comment: </b>" + issue.CommentsForActionsTaken + "" +
"					</body>" +
"</html>";

				}

				else
				{
					userErr.errorMessage = "There was something wrong when trying to send email. [Object] {Email Class.cs} [Line] {167}";
					userErr.Title = "Outlook error";
					userErr.CallWindow();
					userErr.ShowDialog();

				}








				mailItem.HTMLBody = htmlString;

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

		private string GetSenderEmailAddress(MailItem mailItem)
		{


			string SenderEmailAddress;

			if (mailItem.Session.CurrentUser.AddressEntry.Type == "EX")
			{
				SenderEmailAddress = mailItem.Session.CurrentUser.AddressEntry.GetExchangeUser().PrimarySmtpAddress;
			}
			else
			{
				SenderEmailAddress = mailItem.UserProperties.Session.CurrentUser.Address;

			}


			return SenderEmailAddress;
		}
	}
}

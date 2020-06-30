using Microsoft.Office.Interop.Outlook;

using System;
using System.Diagnostics;
using System.Windows.Media;

namespace Engineering_Database
{
	class EmailClass
	{

		readonly UserSettings userSett = new UserSettings();
		readonly UserErrorWindow userErr = new UserErrorWindow();
		readonly DatabaseClass db = new DatabaseClass();


		string htmlString;
		public string sender = null;

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

				userSett.openSettings();
				string emailAddress = userSett.Email;

				Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();

				Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);


				sender = GetSenderEmailAddress(mailItem);






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

				if (userSett.EmailPreview == "Yes")
				{
					mailItem.Display(true);
				}
				else
				{
					mailItem.Send();
				}





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

		public void ReportAppISsueEmail(string Date, string Time, string Username, string report)
		{

			userSett.openSettings();


			if (checkOutlook() == true)
			{


				string emailAddress = userSett.ReportEmail;

				Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();

				Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);




				sender = GetSenderEmailAddress(mailItem);




				try
				{
					mailItem.To = emailAddress;
					mailItem.CC = sender;
					mailItem.Subject = "Report for application has been sent from: " + Username;


					htmlString = "<html><body><h4> " + Username + " reported  issue below for application:</b></h4>" +
				report + "</body>" +
				"</html>";

					mailItem.HTMLBody = htmlString;
					if (userSett.EmailPreview == "Yes")
					{
						mailItem.Display(true);
					}
					else
					{
						mailItem.Send();
					}


				}




				catch
				{
					userErr.errorMessage = "There was something wrong when trying to send email. [Object] {Email Class.cs} [Line] {167}";
					userErr.Title = "Outlook error";
					userErr.CallWindow();
					userErr.ShowDialog();

				}












			}
			else
			{
				userErr.errorMessage = "Outlook is not open.Please open outlook aplication and try again";
				userErr.Title = "Outlook error";
				userErr.CallWindow();
				userErr.ShowDialog();
			}


		}


		public void RequestProduct(string Date, string Username, string product, string qty, string measureType, ImageSource fname)
		{
			userSett.openSettings();


			if (checkOutlook() == true)
			{


				string emailAddress = userSett.productRequests;

				Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();

				Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);




				sender = GetSenderEmailAddress(mailItem);




				try
				{
					mailItem.To = emailAddress;
					mailItem.CC = sender;
					mailItem.Subject = product + " request from: " + Username;


					htmlString = " <h2>Product Requests</h2>" +
						" <br>" +
						" <b><u>Requester </u>:</b>&nbsp;" + Username +
						" <br>" +
						" <b><u>Product Requested </u>:</b> &nbsp;" + product +
						" <br>" +
						"<b><u>Measure Type </u>:</b>&nbsp;" + measureType +
						"<br>" +
						"<b><u>Qty Requested </u>:</b> &nbsp;" + qty +
						"<br>" +
						"<br>" +
						"<b><u>Product Image</b></u>" +
						"<br>" +
						"<br>" +
						@"<img width=""200px"" height =""7000px"" style=""position:relative; top: 70px; left: 320px;"" src='" + fname + "'>" +
						"</body>" +

						"</html>"
						;

					mailItem.HTMLBody = htmlString;
					if (userSett.EmailPreview == "Yes")
					{
						mailItem.Display(true);
					}
					else
					{
						mailItem.Send();
					}


				}




				catch
				{
					userErr.errorMessage = "There was something wrong when trying to send email. [Object] {Email Class.cs} [Line] {167}";
					userErr.Title = "Outlook error";
					userErr.CallWindow();
					userErr.ShowDialog();

				}












			}
			else
			{
				userErr.errorMessage = "Outlook is not open.Please open outlook aplication and try again";
				userErr.Title = "Outlook error";
				userErr.CallWindow();
				userErr.ShowDialog();
			}



		}



		#region meeting setup
		public void SetUpMeetingRequest(string subject, string emailbody, DateTime meetingDate, int timeInterval)
		{

			userSett.openSettings();


			if (checkOutlook() == true)
			{


				

				Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();

				//Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

				Microsoft.Office.Interop.Outlook.AppointmentItem meetingItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olAppointmentItem);

				Microsoft.Office.Interop.Outlook.Recipients recipients = null;
				Microsoft.Office.Interop.Outlook.Recipient recipient = null;


				//sender = GetSenderEmailAddress(mailItem);


				//DateTime newDate = DateTime.Now;

				//each meeting have gap  1 hour between each other
				TimeSpan meetingStartTime = new TimeSpan(8 + timeInterval, 0, 0);


				//newDate = newDate.AddDays(meetingDaysForward);

				//checks if new date is saturday or Sunday. If so then days are being changed Applies only when in settings not using weekends
				if (userSett.UseWeekendsForMeeting == "No")
				{
					if (meetingDate.Date.DayOfWeek == DayOfWeek.Saturday)
					{
						meetingDate = meetingDate.AddDays(-1);
					}
					else if (meetingDate.Date.DayOfWeek == DayOfWeek.Sunday)
					{
						meetingDate = meetingDate.AddDays(-2);
					}
				}

				//checks if meeting is being set up for after work hours.if so then meeting is being pushed forward by one day and set up in morning
				if (meetingStartTime.Hours >= 17)
				{
					meetingDate = meetingDate.AddDays(1);
					meetingStartTime = new TimeSpan(8, 0, 0);
				}


				//once all is done. Meeting date is being populated with new date and time.
				meetingDate = meetingDate.Date + meetingStartTime;

				if (meetingItem != null)
				{

					try
					{
						//MessageBox.Show("Setting Up meeting");
						meetingItem.MeetingStatus = Microsoft.Office.Interop.Outlook.OlMeetingStatus.olMeeting;
						meetingItem.Location = "Whittlesey";
						meetingItem.Subject = subject;
						meetingItem.StartInStartTimeZone = DateTime.Now.AddHours(timeInterval);
						meetingItem.Body = emailbody;
					
						meetingItem.Start = meetingDate;
						meetingItem.Duration = 10;
						meetingItem.BusyStatus = OlBusyStatus.olFree;


						//TODO:switch back on when deploying update

						//meeting request to be sent for specific persons - during development switched off

						if (userSett.DebugMode == "No")
						{
							string emailAddress = userSett.MeetingSetupEmail;

							recipients = meetingItem.Recipients;
							recipient = recipients.Add(emailAddress);
							recipient.Type = (int)Microsoft.Office.Interop.Outlook.OlMeetingRecipientType.olRequired;
							if (recipient.Resolve())
							{
								meetingItem.Send();

							}
							else
							{
								userErr.errorMessage = $"Couldn`t resolve email address {emailAddress}";
								userErr.Title = "Outlook error";
								userErr.CallWindow();
								userErr.ShowDialog();
							}

						}


						meetingItem.Save();

					}

					catch
					{
						userErr.errorMessage = "There was something wrong when trying to send email. [Object] {Email Class.cs} [Line] {167}";
						userErr.Title = "Outlook error";
						userErr.CallWindow();
						userErr.ShowDialog();

					}


				}
				else
				{
					userErr.errorMessage = "Meeting Item was with {null} value";
					userErr.Title = "Outlook meeting Item error";
					userErr.CallWindow();
					userErr.ShowDialog();
				}


			}
			else
			{
				userErr.errorMessage = "Outlook is not open.Please open outlook aplication and try again";
				userErr.Title = "Outlook error";
				userErr.CallWindow();
				userErr.ShowDialog();
			}
		}
		#endregion
	}

}
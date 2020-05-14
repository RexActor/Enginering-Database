using Engineering_Database;

using System;
using System.Data;
using System.Data.OleDb;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Enginering_Database
{
	/// <summary>
	/// Interaction logic for viewDatabase.xaml
	/// </summary>
	public partial class viewDatabase : Window
	{

		readonly IssueClass issueClass = new IssueClass();

		int jobList;
		int convJobNumber;
		readonly string LogedInUser;
		readonly DatabaseClass db = new DatabaseClass();
		System.Windows.Controls.Button textTestLabel;
		public string contentForTextTestLabel;


		public viewDatabase()


		{
			db.ConnectDB();
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			InitializeComponent();
			LogedInUser = WindowsIdentity.GetCurrent().Name;
			LogedInUserLabel.Content = LogedInUser;


			OutstandingReports("Outstanding");
			ResolvedReports("Resolved");

		}


		public void viewdataGrid()
		{



			//throw new NotImplementedException();



		}


		private void OutstandingReports(string filter)
		{
			issueClass.updateIssueDataList();



			OustandingUserStackPanel.Children.Clear();


			jobList = 100;



			if (jobList > 0)
			{

				OleDbDataAdapter da = new OleDbDataAdapter(db.DBQueryforUsers(filter, LogedInUser));
				DataTable dt = new DataTable();
				int i = 0;
				da.Fill(dt);
				da.Dispose();




				foreach (DataRow dr in dt.Rows)
				{
					if (i <= dt.Rows.Count && i < jobList)
					{


						textTestLabel = new System.Windows.Controls.Button();
						textTestLabel.BorderThickness = new Thickness(0);
						textTestLabel.Background = new SolidColorBrush(Color.FromArgb(195, 195, 195, 0));

						textTestLabel.Height = 30;
						textTestLabel.Padding = new Thickness(3);
						textTestLabel.Margin = new Thickness(2);
						contentForTextTestLabel = dr["JobNumber"].ToString();
						textTestLabel.Content = contentForTextTestLabel;




						textTestLabel.Click += (sender, e) => { TextTestLabel_Click(sender, e); };

						OustandingUserStackPanel.Children.Add(textTestLabel);

					}
					i++;
				}

			}





		}
		private void TextTestLabel_Click(object sender, RoutedEventArgs e)
		{
			//canChangeDueDate = true;
			//canSendEmail = true;
			//canSubmit = true;
			/*
			if (ChangeDueDateCheckBox.IsChecked == true)
			{
				ChangeDueDateCheckBox.IsChecked = false;
			}

			if (ConfirmEmailCheckBox.IsChecked == true)
			{
				ConfirmEmailCheckBox.IsChecked = false;
			}
			*/

			//log.Debug(p);
			Button btn = sender as Button;



			//log.Debug(btn.Content.ToString());


			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}


			//Frame2StackPanel.Children.Clear();
			UserFrameStackPanel.Children.Clear();

			System.Windows.Controls.TextBlock frame2TextBlock = new System.Windows.Controls.TextBlock();


			//UpdateFrame2(btn.Content.ToString());
			UpdateUserFrame(btn.Content.ToString());

			UserFrameStackPanel.Children.Add(frame2TextBlock);
			//Frame2StackPanel.Children.Add(frame2TextBlock);



		}
		private void TextTestLabel_Leave(object sender, MouseEventArgs e)
		{

			//previewStackPanel.Visibility = Visibility.Hidden;
		}

		private void TextTestLabel_Hover(object sender, EventArgs e)
		{
			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}


			Button lbl = (Button)sender;

			if (lbl == null)
			{


			}
			else
			{

				int convertSearch;
				int p;
				if (Int32.TryParse(lbl.Content.ToString(), out p))
				{
					convertSearch = p;

				}
				else
				{
					convertSearch = 1;
				}




			}



		}

		private void ResolvedReports(string filter)
		{
			issueClass.updateIssueDataList();


			ResolvedUserstackPanel.Children.Clear();


			jobList = 100;



			if (jobList > 0)
			{

				OleDbDataAdapter da = new OleDbDataAdapter(db.DBQueryforUsers(filter, LogedInUser));
				DataTable dt = new DataTable();
				int i = 0;
				da.Fill(dt);
				da.Dispose();




				foreach (DataRow dr in dt.Rows)
				{
					if (i <= dt.Rows.Count && i < jobList)
					{


						textTestLabel = new System.Windows.Controls.Button();
						textTestLabel.BorderThickness = new Thickness(0);
						textTestLabel.Background = new SolidColorBrush(Color.FromArgb(195, 195, 195, 0));

						textTestLabel.Height = 30;
						textTestLabel.Padding = new Thickness(3);
						textTestLabel.Margin = new Thickness(2);
						contentForTextTestLabel = dr["JobNumber"].ToString();
						textTestLabel.Content = contentForTextTestLabel;




						textTestLabel.Click += (sender, e) => { TextTestLabel_Click(sender, e); };


						ResolvedUserstackPanel.Children.Add(textTestLabel);

					}
					i++;
				}

			}




		}

		private void UpdateUserFrame(string jobNumber)
		{
			int c;


			if (Int32.TryParse(jobNumber, out c))
			{
				convJobNumber = c;

				//log.Debug(c);
			}
			string[] words = jobNumber.Split(null);

			//UserJobNumberLabelData
			UserJobNumberLabelData.Content = words[words.Length - 1];
			UserReportedDateLabelData.Content = Convert.ToDateTime(db.DBQuery("ReportedDate", convJobNumber)).ToShortDateString().ToString();
			//Frame2ReportedUserData.Content = db.DBQuery("ReportedUsername", convJobNumber);
			UserAreaLabelData.Content = db.DBQuery("Area", convJobNumber);
			UserIssueTypeLabelData.Content = db.DBQuery("Type", convJobNumber);
			UserBuildingLabelData.Content = db.DBQuery("Building", convJobNumber);
			UserReportedTimeLabelData.Content = Convert.ToDateTime(db.DBQuery("ReportedTime", convJobNumber)).ToShortTimeString();
			UserDueDateLabelData.Content = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber)).ToShortDateString().ToString();
			UserFaultyAreaLabelData.Content = db.DBQuery("FaultyArea", convJobNumber);
			UserIssueCodeLabelData.Content = db.DBQuery("IssueCode", convJobNumber);
			UserAssetNumberLabelData.Content = db.DBQuery("AssetNumber", convJobNumber);
			UserPriorityLabelData.Content = db.DBQuery("Priority", convJobNumber);

			//Frame2ReportedDescription.Text = db.DBQuery("DetailedDescription", convJobNumber);
			UserDescTextBox.Text = db.DBQuery("DetailedDescription", convJobNumber);

			//Frame3DueDateTextBox.Text = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber)).ToShortDateString().ToString();
			//Frame3StartTimeTextBox.Text = Convert.ToDateTime(db.DBQuery("StartTime", convJobNumber)).ToShortTimeString();
			//Frame3FinishTimeTextBox.Text = Convert.ToDateTime(db.DBQuery("FinishTime", convJobNumber)).ToShortTimeString();

			if (db.DBQuery("AssignedTo", convJobNumber) == "")
			{
				UserAssignedToLabelData.Content = "Job is not assigned";

			}
			else
			{

				UserAssignedToLabelData.Content = db.DBQuery("AssignedTo", convJobNumber);
			}


			if (db.DBQuery("Contractor", convJobNumber) != "")
			{

				UserContractorLabelData.Content = db.DBQuery("Contractor", convJobNumber);
			}
			else
			{

				UserContractorLabelData.Content = "No Contractors Assigned";
			}

			if (db.DBQuery("CommentsForActionTaken", convJobNumber) != "")
			{

				UserEngineerTextBox.Text = db.DBQuery("CommentsForActionTaken", convJobNumber);
			}
			else
			{

				UserEngineerTextBox.Text = "There are no comment from Enginner";
			}




		}
		private void RefreshUserView(object sender, RoutedEventArgs e)
		{

			ResolvedUserstackPanel.Children.Clear();
			OustandingUserStackPanel.Children.Clear();

			OutstandingReports("Outstanding");
			ResolvedReports("Resolved");


			UserJobNumberLabelData.Content = "";
			UserReportedDateLabelData.Content = "";
			UserAreaLabelData.Content = "";
			UserBuildingLabelData.Content = "";
			UserIssueCodeLabelData.Content = "";
			UserPriorityLabelData.Content = "";
			UserAssetNumberLabelData.Content = "";
			UserReportedTimeLabelData.Content = "";
			UserContractorLabelData.Content = "";
			UserAssetNumberLabelData.Content = "";
			UserFaultyAreaLabelData.Content = "";
			UserIssueTypeLabelData.Content = "";
			UserDueDateLabelData.Content = "";
			UserDescTextBox.Text = "";
			UserEngineerTextBox.Text = "";
			UserAssignedToLabelData.Content = "";


		}

	}
}

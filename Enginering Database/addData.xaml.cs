using System;
using System.Windows;
using System.Windows.Controls;
using System.Security.Principal;
using System.IO;
using Engineering_Database;
using System.Windows.Documents;

namespace Enginering_Database
	{
	/// <summary>
	/// Interaction logic for addData.xaml
	/// </summary>
	public partial class addData:Window
		{
		DatabaseClass db = new DatabaseClass();
		public addData ( )
			{
			InitializeComponent();
			updateTimeAndDate();
			//usernameLabelValue.Content = WindowsIdentity.GetCurrent().Name;
			usernameLabelValue.Content = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;

			//area combo box values 
			areaComboBox.Text = "Please Choose";
			areaComboBox.Items.Insert(0,"Please Choose");
			areaComboBox.Items.Add("Production");
			areaComboBox.Items.Add("Warehouse");
			areaComboBox.Items.Add("Facilities");
			areaComboBox.Items.Add("Projects");
			areaComboBox.SelectedIndex = 0;

			PriorityComboBox.Text="Please Choose";
			PriorityComboBox.Items.Insert(0, "Please Choose");
			PriorityComboBox.Items.Add("Normal");
			PriorityComboBox.Items.Add("High");
			PriorityComboBox.Items.Add("Urgent");
			PriorityComboBox.SelectedIndex = 0;

			//disabling all comboboxes which ones not needed on start to be enabled before choosing area code
			buildingComboBox.IsEnabled = false;
			faultyAreaComboBox.IsEnabled = false;
			issueComboBox.IsEnabled = false;
			issueTypeComboBox.IsEnabled = false;

			getJobNumber();
			}
		public void updateTimeAndDate ( )
			{
			DateTime time = DateTime.Now;
			

			timeLabelAddData.Content = time.ToString("HH:mm");
			dateLabelAddData.Content = time.ToString("dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture);
			}


		#region comboboxes set up

		private void ComboBox_SelectionChanged (object sender,SelectionChangedEventArgs e)
			{
			
			switch(areaComboBox.SelectedItem)
				{
				case "Production":

					issueTypeComboBox.IsEnabled = true;
					//update faulty area drop down list

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"waiting for Issue Type");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0,"Please Choose");
					issueTypeComboBox.Items.Add("Lines Mechanical");
					issueTypeComboBox.Items.Add("Boxing");
					issueTypeComboBox.SelectedIndex = 0;

					//update building drop down list
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Waiting for Issue Type");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = false;

					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Waiting for Issue Type");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
				
					break;

				case "Warehouse":
					issueTypeComboBox.IsEnabled = true;
					//update faulty area drop down list

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"waiting for Issue Type");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0,"Please Choose");
					issueTypeComboBox.Items.Add("MHE Equipment");
					issueTypeComboBox.Items.Add("Rackings");
					issueTypeComboBox.SelectedIndex = 0;

					//update building drop down list
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Waiting for Issue Type");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = false;

					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Waiting for Issue Type");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				case "Facilities":
					issueTypeComboBox.IsEnabled = true;
					//update faulty area drop down list

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Waiting for Issue Type");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;


					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0,"Please Choose");
					issueTypeComboBox.Items.Add("Security");
					issueTypeComboBox.Items.Add("Drains");
					issueTypeComboBox.Items.Add("Office Equipment");
					issueTypeComboBox.SelectedIndex = 0;

					//update building drop down list
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Waiting for Issue Type");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = false;


					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Waiting for Issue Type");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
					break;

					


				case "Projects":
					issueTypeComboBox.IsEnabled = false;
					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0,"Projects");
					issueTypeComboBox.SelectedIndex = 0;


					//faulty area drop down list
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Projects");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;


				

					//update building drop down list
					buildingComboBox.Items.Clear();



					buildingComboBox.Items.Insert(0,"Please Choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = true;
					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Projects");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				default:
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Waiting for issue area");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = false;

					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0,"Waiting for issue area");
					issueTypeComboBox.SelectedIndex = 0;
					issueTypeComboBox.IsEnabled = false;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Waiting for issue area");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Waiting for issue area");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
				
					

					break;


				}


			}

		private void IssueTypeComboBox_SelectionChanged (object sender,SelectionChangedEventArgs e)
			{

			switch(issueTypeComboBox.SelectedItem)
				{
			
				case "Lines Mechanical":
					buildingComboBox.IsEnabled = true;

					
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Please Choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;
					//update faulty area combobox

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Please choose");
					faultyAreaComboBox.Items.Add("Line 1");
					faultyAreaComboBox.Items.Add("Line 2");
					faultyAreaComboBox.Items.Add("Line 3");
					faultyAreaComboBox.Items.Add("Line 4");
					faultyAreaComboBox.Items.Add("Line 5");
					faultyAreaComboBox.Items.Add("Line 6");
					faultyAreaComboBox.Items.Add("Line 7");
					faultyAreaComboBox.Items.Add("Line 8");
					
					
					faultyAreaComboBox.SelectedIndex = 0;

					//update issue code combobox

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Please choose");
					issueComboBox.Items.Add("Bucket filler - Bercomex");
					issueComboBox.Items.Add("Carousel LHS conveyer");
					issueComboBox.Items.Add("Carousel RHS conveyer");
					issueComboBox.Items.Add("Boxing modular belt");
					issueComboBox.Items.Add("Blade");
					issueComboBox.Items.Add("Strimmer");
					issueComboBox.Items.Add("Take off belt");
					issueComboBox.Items.Add("Pull cord");
					issueComboBox.Items.Add("E-Stop overhead");
					issueComboBox.Items.Add("Waste conveyer belt");
					issueComboBox.Items.Add("Binder");
					issueComboBox.Items.Add("Robotag");
					issueComboBox.Items.Add("Terra Machine - Binder");
					issueComboBox.Items.Add("Terra Machine - Taper");
					issueComboBox.Items.Add("Terra Machine - Sleeving Section");
					issueComboBox.Items.Add("Green Compactor");
					issueComboBox.Items.Add("Water supply");
					issueComboBox.Items.Add("Air pipe supply");
					issueComboBox.Items.Add("Power supply");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
					break;

				case "Boxing":

					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please Choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					//update faulty area combobox
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Other");
					faultyAreaComboBox.SelectedIndex = 0;

					//update issue code combobox values

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Please choose");
					issueComboBox.Items.Add("Conveyer belt");
					issueComboBox.Items.Add("Power sockets");
					
					issueComboBox.SelectedIndex = 0;
					

					break;

				case "MHE Equipment":

					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Please Choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;


					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Choose option");
					faultyAreaComboBox.Items.Add("Flexi");
					faultyAreaComboBox.Items.Add("Scissor Lift");
					faultyAreaComboBox.Items.Add("PPT");
					faultyAreaComboBox.Items.Add("Electric FLT");
					faultyAreaComboBox.Items.Add("Gas FLT");
					faultyAreaComboBox.Items.Add("Pump truck");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;



					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Waiting for faulty area");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				case "Rackings":
					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Other");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Other");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				case "Security":

					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Please choose");
					faultyAreaComboBox.Items.Add("Loading Bays");
					faultyAreaComboBox.Items.Add("Roller Doors");
					faultyAreaComboBox.Items.Add("Alarm");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;

					issueComboBox.Items.Clear();

					issueComboBox.Items.Insert(0,"Waiting for faulty area");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;


					break;

				case "Drains":

					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Please choose");
					faultyAreaComboBox.Items.Add("Indoor");
					faultyAreaComboBox.Items.Add("Outdoor");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Other");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				case "Office Equipment":


					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Other");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Other");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

 
					

				default:

					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0,"Waiting for issue type");
					buildingComboBox.SelectedIndex = 0;

					

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0,"Waiting for issue type");
					faultyAreaComboBox.SelectedIndex = 0;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Waiting for issue type");
					faultyAreaComboBox.SelectedIndex = 0;


					break;






				}



			}

		private void BuildingComboBox_SelectionChanged (object sender,SelectionChangedEventArgs e)
			{
			if(issueTypeComboBox.SelectedItem=="Lines Mechanical")
				{
				faultyAreaComboBox.Items.Clear();
				faultyAreaComboBox.Items.Insert(0,"Please choose");
				faultyAreaComboBox.SelectedIndex = 0;

				switch(buildingComboBox.SelectedItem)
					{

					case "Building 1":
						faultyAreaComboBox.IsEnabled = true;
						faultyAreaComboBox.Items.Clear();
						faultyAreaComboBox.Items.Insert(0,"Choose option");
						faultyAreaComboBox.Items.Add("Line 1");
						faultyAreaComboBox.Items.Add("Line 2");
						faultyAreaComboBox.Items.Add("Line 3");
						faultyAreaComboBox.Items.Add("Line 4");
						faultyAreaComboBox.Items.Add("Line 5");
						faultyAreaComboBox.Items.Add("Line 6");
						faultyAreaComboBox.Items.Add("Line 7");
						faultyAreaComboBox.Items.Add("Line 8");
						faultyAreaComboBox.Items.Add("Condition line");
						faultyAreaComboBox.SelectedIndex = 0;

						issueComboBox.Items.Clear();
						issueComboBox.Items.Insert(0,"Waiting for faulty area");
						issueComboBox.SelectedIndex = 0;
						issueComboBox.IsEnabled = false;


						break;

					case "Building 2":
						faultyAreaComboBox.IsEnabled = true;
						faultyAreaComboBox.Items.Clear();
						faultyAreaComboBox.Items.Insert(0,"Choose option");
						faultyAreaComboBox.Items.Add("Line 1");
						faultyAreaComboBox.Items.Add("Line 2");
						faultyAreaComboBox.Items.Add("Line 3");
						faultyAreaComboBox.Items.Add("Line 4");
						faultyAreaComboBox.Items.Add("Line 5");
						faultyAreaComboBox.Items.Add("Line 6");
						faultyAreaComboBox.Items.Add("Line 7");
						faultyAreaComboBox.Items.Add("Line 8");
						faultyAreaComboBox.SelectedIndex = 0;

						issueComboBox.Items.Clear();
						issueComboBox.Items.Insert(0,"Waiting for faulty area");
						issueComboBox.SelectedIndex = 0;
						issueComboBox.IsEnabled = false;
						break;

					default:
						

						break;


					}




				}
			
			}

		private void FaultyAreaComboBox_SelectionChanged (object sender,SelectionChangedEventArgs e)
			{
			issueComboBox.IsEnabled = true;

			switch(faultyAreaComboBox.SelectedItem)
				{

				case "Flexi":
				case "Scissor Lift":
				case "PPT":
				case "Electric FLT":
				case "Gas FLT":
				case "Pump truck":


					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Please choose");
					issueComboBox.Items.Add("Issue 1");
					issueComboBox.Items.Add("Issue 2");
					issueComboBox.Items.Add("Issue 3");
					issueComboBox.Items.Add("Issue 4");
					issueComboBox.Items.Add("Issue 5");
					issueComboBox.Items.Add("Issue 6");
					issueComboBox.Items.Add("Issue 7");
					issueComboBox.Items.Add("Issue 8");
					issueComboBox.SelectedIndex = 0;

					break;

				case "Line 1":
				case "Line 2":
				case "Line 3":
				case "Line 4":
				case "Line 5":
				case "Line 6":
				case "Line 7":
				case "Line 8":
				case "Condition line":

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Please choose");
					issueComboBox.Items.Add("Bucket filler - Bercomex");
					issueComboBox.Items.Add("Carousel LHS conveyer");
					issueComboBox.Items.Add("Carousel RHS conveyer");
					issueComboBox.Items.Add("Boxing modular belt");
					issueComboBox.Items.Add("Blade");
					issueComboBox.Items.Add("Strimmer");
					issueComboBox.Items.Add("Take off belt");
					issueComboBox.Items.Add("Pull cord");
					issueComboBox.Items.Add("E-Stop overhead");
					issueComboBox.Items.Add("Waste conveyer belt");
					issueComboBox.Items.Add("Binder");
					issueComboBox.Items.Add("Robotag");
					issueComboBox.Items.Add("Terra Machine - Binder");
					issueComboBox.Items.Add("Terra Machine - Taper");
					issueComboBox.Items.Add("Terra Machine - Sleeving Section");
					issueComboBox.Items.Add("Green Compactor");
					issueComboBox.Items.Add("Water supply");
					issueComboBox.Items.Add("Air pipe supply");
					issueComboBox.Items.Add("Power supply");
					issueComboBox.SelectedIndex = 0;


					break;
				case "Loading Bays":

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Please choose");
					issueComboBox.Items.Add("Bay 1");
					issueComboBox.Items.Add("Bay 2");
					issueComboBox.Items.Add("Bay 3");
					issueComboBox.Items.Add("Bay 4");
					issueComboBox.SelectedIndex = 0;

					break;

				case "Alarm":

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Please choose");
					issueComboBox.Items.Add("Intruder alarm");
					issueComboBox.Items.Add("Fire Alarm");
					issueComboBox.SelectedIndex = 0;

					break;

				case "Roller Doors":
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Please choose");
					issueComboBox.Items.Add("Chiller 1");
					issueComboBox.Items.Add("Chiller 2");
					issueComboBox.Items.Add("Chiller 3");
					issueComboBox.Items.Add("Chiller 4");
					issueComboBox.Items.Add("Warehouse 1");
					issueComboBox.Items.Add("Warehouse 2");
					issueComboBox.SelectedIndex = 0;

					break;

				case "Indoor":
				case "Outdoor":
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Other");
					issueComboBox.SelectedIndex = 0;
					break;


					

				default:

					

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0,"Waiting for faulty area");
					faultyAreaComboBox.SelectedIndex = 0;
					break;
				}

			}
		#endregion

		public void getJobNumber ( )
			{

			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}
			//Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
			//Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(Directory.GetCurrentDirectory() + @"\database.xlsx");
			//Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1);
			//Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

			//int lastUsedRow = excelRange.Row + excelRange.Rows.Count - 1;
			//Double jobNumber = (excelSheet.Cells[lastUsedRow,2] as Microsoft.Office.Interop.Excel.Range).Value2 + 1;


			int lastJob = db.DBQueryLastJobNumber("JobNumber")+1;
			jobNumberDataLabel.Content = lastJob.ToString();




			}

		private void InsertDataIntoDatabase_Click(object sender, RoutedEventArgs e)
		{

			

			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}


			/*
			 * areaComboBox
				issueTypeComboBox
				dateLabelAddData
				timeLabelAddData
				usernameLabelValue
				buildingComboBox
				faultyAreaComboBox
				issueComboBox
				jobNumberDataLabel
				AssetNumberTextBoxData
				PriorityComboBox
				DetailedDescriptionRichTextBox
				*/

			
			
			int getLastJobNumber = db.DBQueryLastJobNumber("JobNumber");

			string dtNow = DateTime.Now.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
			string dtTmNow = DateTime.Now.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

			TextRange textRange = new TextRange(DetailedDescriptionRichTextBox.Document.ContentStart, DetailedDescriptionRichTextBox.Document.ContentEnd);


			//setting up new issue object for uploading to database

			IssueClass issue = new IssueClass();

			issue.Area = areaComboBox.Text;
			issue.AssetNumber = AssetNumberTextBox.Text;
			issue.Building = buildingComboBox.Text;
			issue.Code = issueComboBox.Text;
			issue.DetailedDescription = textRange.Text;
			issue.DueDate = dtNow;
			issue.FaulyArea = faultyAreaComboBox.Text;

			//get last job number and increment by 1
			issue.JobNumber = getLastJobNumber+1;

			issue.ReportedDate = dtNow;
			issue.ReportedTime = dtTmNow;
			issue.Type = issueTypeComboBox.Text;
			issue.Priority = PriorityComboBox.Text;
			issue.ReportedUserName = usernameLabelValue.Content.ToString();




			db.InsertDataIntoDatabase(issue.JobNumber, issue.ReportedDate, issue.ReportedTime, issue.ReportedUserName, issue.AssetNumber, issue.FaulyArea, issue.Building, issue.Code, issue.Priority, issue.Type, issue.DetailedDescription, issue.DueDate, issue.Area);

			//db.InsertDataIntoDatabase(getLastJobNumber+1, dtNow, dtTmNow, usernameLabelValue.Content.ToString(),AssetNumberTextBox.Text,faultyAreaComboBox.Text,buildingComboBox.Text,issueComboBox.Text,PriorityComboBox.Text,issueTypeComboBox.Text,textRange.Text,dtNow,areaComboBox.Text);
		}
	}



	

	}

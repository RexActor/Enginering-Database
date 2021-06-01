using DocumentFormat.OpenXml.Bibliography;

using Engineering_Database;

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Enginering_Database
{
	/// <summary>
	/// Interaction logic for addData.xaml
	/// </summary>
	public partial class addData : Window
	{
		private readonly DatabaseClass db = new DatabaseClass();
		private readonly EmailClass email = new EmailClass();
		private ErrorSystem err = new ErrorSystem();

		private ExistingIssues existingIssuesWindow;

		private List<IssueClass> alreadyReportedIssues = new List<IssueClass>();

		//readonly UserSettings userrSet = new UserSettings();
		private bool richTextBoxTextChanged = false;

		public addData()
		{
			try
			{
				InitializeComponent();
				AlreadyReportedLabel.Visibility = Visibility.Hidden;
				AlreadyReportedButton.Visibility = Visibility.Hidden;
				//hiding error message by default
				ErrorMessageLabel.Visibility = Visibility.Hidden;

				//update time and date for correct labels
				UpdateTimeAndDate();

				//getting curent username from system
				usernameLabelValue.Content = WindowsIdentity.GetCurrent().Name;

				SetUpComboBox();

				GetJobNumber();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void UpdateTimeAndDate()
		{
			try
			{
				DateTime time = DateTime.Now;

				timeLabelAddData.Content = time.ToString("HH:mm");
				dateLabelAddData.Content = time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		#region comboboxes set up

		//need to add option to able to add/remove/edit issue options through settings

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				db.ConnectDB();
				issueTypeComboBox.Items.Clear();

				//checks if area combox box have selected value other than default
				//if there is not selected value --> issuetype combo box is being disabled and default value is being selected for this combo box
				if (areaComboBox.SelectedIndex == 0)
				{
					issueTypeComboBox.IsEnabled = false;
					issueComboBox.Items.Add("Waiting for data");
					issueTypeComboBox.Items.Add("Waiting for data");
					faultyAreaComboBox.Items.Add("Waiting for data");
					issueComboBox.SelectedIndex = 0;
					faultyAreaComboBox.SelectedIndex = 0;
					issueTypeComboBox.SelectedIndex = 0;
				}
				else
				{
					issueTypeComboBox.IsEnabled = true;
				}

				//areacombobox - static --> still dependant on settings window
				if (areaComboBox.SelectedIndex > 0)
				{
					db.ConnectDB();
					var setIssueTypeComboBox = db.SetUpComboBoxBasedonParrent("IssueTypeComboBox", areaComboBox.SelectedItem.ToString());
					issueTypeComboBox.Items.Add("Please Select");
					//gets all values from database where UID is linked with area combo box ID
					while (setIssueTypeComboBox.Read())
					{
						issueTypeComboBox.Items.Add(setIssueTypeComboBox[1]);
					}
					//select value by default -- > "Please Select"
					issueTypeComboBox.SelectedIndex = 0;
					faultyAreaComboBox.Items.Add("Waiting for data");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;
					issueComboBox.Items.Add("Waiting for data");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void IssueTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				db.ConnectDB();
				faultyAreaComboBox.Items.Clear();

				if (areaComboBox.SelectedIndex == 0)
				{
					faultyAreaComboBox.IsEnabled = false;
					faultyAreaComboBox.Items.Add("Waiting for data");
					issueComboBox.Items.Add("Waiting for data");
					faultyAreaComboBox.SelectedIndex = 0;
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
				}
				else
				{
					if (issueTypeComboBox.SelectedIndex > 0)
					{
						db.ConnectDB();
						var setFaultyAreaComboBox = db.SetUpComboBoxBasedonParrent("FaultyAreaComboBox", issueTypeComboBox.SelectedItem.ToString());

						faultyAreaComboBox.Items.Add("Please Select");
						//gets all values from database where UID is linked with area combo box ID
						while (setFaultyAreaComboBox.Read())
						{
							faultyAreaComboBox.Items.Add(setFaultyAreaComboBox[1]);
						}
						//select value by default -- > "Please Select"
						faultyAreaComboBox.SelectedIndex = 0;
					}
					faultyAreaComboBox.IsEnabled = true;
					issueComboBox.Items.Add("Waiting for data");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void BuildingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void FaultyAreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				db.ConnectDB();
				issueComboBox.Items.Clear();

				if (faultyAreaComboBox.SelectedIndex == 0)
				{
					issueComboBox.IsEnabled = false;
					issueComboBox.Items.Add("Waiting for data");
					issueComboBox.SelectedIndex = 0;
				}
				else
				{
					if (issueTypeComboBox.SelectedIndex > 0)
					{
						db.ConnectDB();
						var setIssueComboBox = db.SetUpComboBoxBasedonParrent("IssueComboBox", issueTypeComboBox.SelectedItem.ToString());

						issueComboBox.Items.Add("Please Select");
						//gets all values from database where UID is linked with area combo box ID
						while (setIssueComboBox.Read())
						{
							issueComboBox.Items.Add(setIssueComboBox[1]);
						}
						//select value by default -- > "Please Select"
						issueComboBox.SelectedIndex = 0;
					}
					issueComboBox.IsEnabled = true;
				}
				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		#endregion comboboxes set up

		#region dynamic ComboBoxSetup - initialization at launch of application

		public void SetUpComboBox()
		{
			try
			{
				//refactoring ComboBox Setup

				//set up comboboxes based on previous data selected
				//there are 2 objects which ones don't have dynamic values. Still needs to be able to change on settings window

				//set up default comboboxes with default values

				db.ConnectDB();

				//areacombobox - static --> still dependant on settings window
				var getAreaComboBox = db.SetUpComboBox("AreaComboBox");
				areaComboBox.Items.Add("Please Select");
				while (getAreaComboBox.Read())
				{
					areaComboBox.Items.Add(getAreaComboBox[1]);
				}
				areaComboBox.SelectedIndex = 0;
				db.ConnectDB();
				//PriorityComboBox - static --> still dependant on settings window
				var getPriorityComboBox = db.SetUpComboBox("PriorityComboBox");
				PriorityComboBox.Items.Add("Please Select");
				while (getPriorityComboBox.Read())
				{
					PriorityComboBox.Items.Add(getPriorityComboBox[1]);
				}
				PriorityComboBox.SelectedIndex = 0;

				//buildingComboBox - static --> still dependant on settings window
				db.ConnectDB();
				var getBuildingComboBox = db.SetUpComboBox("BuildingComboBox");
				buildingComboBox.Items.Add("Please Select");
				while (getBuildingComboBox.Read())
				{
					buildingComboBox.Items.Add(getBuildingComboBox[1]);
				}
				buildingComboBox.SelectedIndex = 0;
				//buildingComboBox.IsEnabled = true;

				db.CloseDB();

				//issueTypeComboBox - dynamic
				issueTypeComboBox.Items.Add("Waiting for data");
				issueTypeComboBox.SelectedIndex = 0;
				issueTypeComboBox.IsEnabled = false;

				//faultyAreaComboBo - dynamic
				faultyAreaComboBox.Items.Add("Waiting for data");
				faultyAreaComboBox.SelectedIndex = 0;
				faultyAreaComboBox.IsEnabled = false;

				//issueComboBox - dynamic
				issueComboBox.Items.Add("Waiting for data");
				issueComboBox.SelectedIndex = 0;
				issueComboBox.IsEnabled = false;

				//end of part for refactoring
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		#endregion dynamic ComboBoxSetup - initialization at launch of application

		public void GetJobNumber()
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}

				int lastJob = db.DBQueryLastJobNumber("JobNumber") + 1;
				jobNumberDataLabel.Content = lastJob.ToString();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void InsertDataIntoDatabase_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}

				UserSettings userrSet = new UserSettings();
				userrSet.openSettings();

				int getLastJobNumber = db.DBQueryLastJobNumber("JobNumber");

				DateTime dueDate = DateTime.Now.AddDays(float.Parse(userrSet.DueDateGap));

				string dtNow = dueDate.ToString("dd/MM/yyyy");

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
				issue.JobNumber = getLastJobNumber + 1;

				DateTime time = DateTime.Now;

				issue.ReportedDate = time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

				issue.ReportedTime = time.ToString("HH:mm");
				issue.Type = issueTypeComboBox.Text;
				issue.Priority = PriorityComboBox.Text;
				issue.ReportedUserName = usernameLabelValue.Content.ToString();

				if (CheckFilledData())
				{
					ErrorMessageLabel.Visibility = Visibility.Hidden;
					email.SendEmail(null, issue, "Report");
					if (issue.ReporterEmail == null)
					{
						issue.ReporterEmail = "";
					}
					//checks one more time for correct job number to be updated into database.
					//this is to avoid issue of double job numbers due the delay on email application (outlook) opening

					issue.JobNumber = Convert.ToInt32(db.DBQueryLastJobNumber("JobNumber")) + 1;

					db.InsertDataIntoDatabase(issue.JobNumber, issue.ReportedDate, issue.ReportedTime, issue.ReportedUserName, issue.AssetNumber, issue.FaulyArea, issue.Building, issue.Code, issue.Priority, issue.Type, issue.DetailedDescription, issue.DueDate, issue.Area, issue.ReporterEmail);

					this.Close();
				}
				else
				{
					ErrorMessageLabel.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ClearRichTextBox(object sender, RoutedEventArgs e)
		{
			try
			{
				((RichTextBox)sender).Document = new FlowDocument();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void CheckRichTextBoxchanges(object sender, TextChangedEventArgs e)
		{
			try
			{
				richTextBoxTextChanged = true;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public bool CheckFilledData()
		{
			try
			{
				#region set up label colours if 0 index selected

				if (areaComboBox.SelectedIndex == 0)
				{
					AreaLabel.Background = Brushes.Red;
				}
				else
				{
					//AreaLabel.Background = new SolidColorBrush(Color.FromArgb(100, 0, 195, 0));
					AreaLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
				}

				if (issueTypeComboBox.SelectedIndex == 0)
				{
					issueTypeLabel.Background = Brushes.Red;
				}
				else
				{
					issueTypeLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
				}

				if (buildingComboBox.SelectedIndex == 0)
				{
					buildingLabel.Background = Brushes.Red;
				}
				else
				{
					buildingLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
				}
				if (faultyAreaComboBox.SelectedIndex == 0)
				{
					faultyAreaLabel.Background = Brushes.Red;
				}
				else
				{
					faultyAreaLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
				}
				if (issueComboBox.SelectedIndex == 0)
				{
					IssueCodeLabel.Background = Brushes.Red;
				}
				else
				{
					IssueCodeLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
				}
				if (PriorityComboBox.SelectedIndex == 0)
				{
					PriorityLabel.Background = Brushes.Red;
				}
				else
				{
					PriorityLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
				}
				if (AssetNumberTextBox.Text == "")
				{
					AssetNumberLabel.Background = Brushes.Red;
				}
				else
				{
					AssetNumberLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
				}

				#endregion set up label colours if 0 index selected

				if (faultyAreaComboBox.SelectedIndex == 0 || issueComboBox.SelectedIndex == 0 || buildingComboBox.SelectedIndex == 0 || issueTypeComboBox.SelectedIndex == 0 || areaComboBox.SelectedIndex == 0 || PriorityComboBox.SelectedIndex == 0)

				{
					return false;
				}
				else
				{
					if (AssetNumberTextBox.Text == "")
					{
						return false;
					}
					else if (richTextBoxTextChanged == false)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
				return false;
			}
		}

		private void AlreadyReportedButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				existingIssuesWindow.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void CheckAlreadyReported(string value)
		{
			try
			{
				existingIssuesWindow = new ExistingIssues();
				db.ConnectDB();
				bool valuefound = false;

				//alreadyReportedIssues.Clear();

				var reader = db.DBQueryForExistingAssets("engineeringDatabaseTable");

				while (reader.Read())
				{
					IssueClass issue = new IssueClass();

					if (reader["AssetNumber"].ToString().ToLower() == value)
					{
						issue.JobNumber = Convert.ToInt32(reader["JobNumber"]);
						issue.DetailedDescription = reader["DetailedDescription"].ToString();
						issue.ReportedUserName = reader["ReportedUsername"].ToString();
						issue.Action = reader["Action"].ToString();
						issue.ReportedDate = String.Format("{0:d/MMM/yyyy}", reader["ReportedDate"]);

						existingIssuesWindow.existingIssuesListView.Items.Add(issue);

						valuefound = true;
					}
				}

				if (valuefound)
				{
					AlreadyReportedLabel.Content = "There are already reported Issues for this Asset Number";
					AlreadyReportedLabel.Foreground = Brushes.Red;

					AlreadyReportedLabel.Visibility = Visibility.Visible;
					AlreadyReportedButton.Visibility = Visibility.Visible;
				}
				else
				{
					AlreadyReportedLabel.Content = "Nothing found for this asset";
					AlreadyReportedLabel.Foreground = Brushes.Green;

					AlreadyReportedLabel.Visibility = Visibility.Visible;
					AlreadyReportedButton.Visibility = Visibility.Hidden;
				}
				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void AssetNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				if (AssetNumberTextBox.Text.ToLower() == "n/a" || AssetNumberTextBox.Text.ToLower() == "na")
				{
					AlreadyReportedLabel.Visibility = Visibility.Hidden;
					AlreadyReportedButton.Visibility = Visibility.Hidden;
				}
				else
				{
					CheckAlreadyReported(AssetNumberTextBox.Text.ToString().ToLower());
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void AssetNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				AlreadyReportedLabel.Visibility = Visibility.Hidden;
				AlreadyReportedButton.Visibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
using Engineering_Database;

using System;
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
		readonly DatabaseClass db = new DatabaseClass();
		readonly EmailClass email = new EmailClass();
		readonly UserSettings userrSet = new UserSettings();
		bool richTextBoxTextChanged = false;
		public addData()
		{
			InitializeComponent();

			//hiding error message by default
			ErrorMessageLabel.Visibility = Visibility.Hidden;

			//update time and date for correct labels
			updateTimeAndDate();

			//getting curent username from system
			usernameLabelValue.Content = WindowsIdentity.GetCurrent().Name;

			SetUpComboBox();


			#region not required anymore - refactored part
			//area combo box values 
			//areaComboBox.Text = "Please Choose";
			//areaComboBox.Items.Insert(0, "Please Choose");
			//areaComboBox.Items.Add("Production");
			//areaComboBox.Items.Add("Warehouse");
			//areaComboBox.Items.Add("Facilities");
			//areaComboBox.Items.Add("Projects");
			//areaComboBox.SelectedIndex = 0;

			//PriorityComboBox.Text = "Please Choose";
			//PriorityComboBox.Items.Insert(0, "Please Choose");
			//PriorityComboBox.Items.Add("Normal");
			//PriorityComboBox.Items.Add("High");
			//PriorityComboBox.Items.Add("Urgent");
			//PriorityComboBox.SelectedIndex = 0;

			//disabling all comboboxes which ones not needed on start to be enabled before choosing area code

			//faultyAreaComboBox.IsEnabled = false;
			//issueComboBox.IsEnabled = false;
			//issueTypeComboBox.IsEnabled = false;

			//issueTypeComboBox.Text = "Please Choose";
			//issueTypeComboBox.Items.Insert(0, "Please Choose");
			//issueTypeComboBox.SelectedIndex = 0;
			#endregion

			GetJobNumber();
		}
		public void updateTimeAndDate()
		{
			DateTime time = DateTime.Now;


			timeLabelAddData.Content = time.ToString("HH:mm");
			dateLabelAddData.Content = time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
		}


		#region comboboxes set up

		//TODO: need to refactore this part. 
		//need to add option to able to add/remove/edit issue options through settings

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			#region not required - in progress refactoring
			/*
			switch (areaComboBox.SelectedItem)
			{
				case "Production":

					issueTypeComboBox.IsEnabled = true;
					//update faulty area drop down list

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "waiting for Issue Type");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0, "Please Choose");
					issueTypeComboBox.Items.Add("Lines Mechanical");
					issueTypeComboBox.Items.Add("Boxing");
					issueTypeComboBox.SelectedIndex = 0;

					//update building drop down list
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Waiting for Issue Type");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = false;

					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Waiting for Issue Type");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				case "Warehouse":
					issueTypeComboBox.IsEnabled = true;
					//update faulty area drop down list

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "waiting for Issue Type");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0, "Please Choose");
					issueTypeComboBox.Items.Add("MHE Equipment");
					issueTypeComboBox.Items.Add("Rackings");
					issueTypeComboBox.SelectedIndex = 0;

					//update building drop down list
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Waiting for Issue Type");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = false;

					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Waiting for Issue Type");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				case "Facilities":
					issueTypeComboBox.IsEnabled = true;
					//update faulty area drop down list

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Waiting for Issue Type");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;


					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0, "Please Choose");
					issueTypeComboBox.Items.Add("Security");
					issueTypeComboBox.Items.Add("Drains");
					issueTypeComboBox.Items.Add("Office Equipment");
					issueTypeComboBox.Items.Add("Cleaning Equipment");
					issueTypeComboBox.SelectedIndex = 0;

					//update building drop down list
					//buildingComboBox.Items.Clear();
					//buildingComboBox.Items.Insert(0, "Waiting for Issue Type");
					//buildingComboBox.SelectedIndex = 0;
					//buildingComboBox.IsEnabled = false;


					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Waiting for Issue Type");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;
					break;




				case "Projects":
					issueTypeComboBox.IsEnabled = false;
					//update issue type  drop down list
					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0, "Please Choose");
					issueTypeComboBox.Items.Add("Projects");
					issueTypeComboBox.SelectedIndex = 1;


					//faulty area drop down list
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please Choose");
					faultyAreaComboBox.Items.Add("Projects");
					faultyAreaComboBox.SelectedIndex = 1;
					faultyAreaComboBox.IsEnabled = false;




					//update building drop down list
					buildingComboBox.Items.Clear();



					buildingComboBox.Items.Insert(0, "Please Choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;
					buildingComboBox.IsEnabled = true;
					//update issue drop down list
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Projects");
					issueComboBox.SelectedIndex = 1;
					issueComboBox.IsEnabled = false;

					break;

				default:
					//buildingComboBox.Items.Clear();
					//buildingComboBox.Items.Insert(0, "Waiting for issue area");
					//buildingComboBox.SelectedIndex = 0;
					//buildingComboBox.IsEnabled = false;

					issueTypeComboBox.Items.Clear();
					issueTypeComboBox.Items.Insert(0, "Waiting for issue area");
					issueTypeComboBox.SelectedIndex = 0;
					issueTypeComboBox.IsEnabled = false;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Waiting for issue area");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = false;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Waiting for issue area");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;



					break;


			}
			*/
			#endregion

			db.ConnectDB();
			issueTypeComboBox.Items.Clear();


			//checks if area combox box have selected value other than default
			//if there is not selected value --> issuetype combo box is being disabled and default value is being selected for this combo box
			if (areaComboBox.SelectedIndex == 0)
			{

				issueTypeComboBox.IsEnabled = false;
				issueTypeComboBox.SelectedIndex = 0;
			}
			else
			{
				issueTypeComboBox.IsEnabled = true;
			}


			//areacombobox - static --> still dependant on settings window
			var setIssueTypeComboBox = db.SetUpComboBoxBasedonUID("IssueTypeComboBox", areaComboBox.SelectedIndex);
			issueTypeComboBox.Items.Add("Please Select");
			//gets all values from database where UID is linked with area combo box ID
			while (setIssueTypeComboBox.Read())
			{
				issueTypeComboBox.Items.Add(setIssueTypeComboBox[1]);
			}
			//select value by default -- > "Please Select"
			issueTypeComboBox.SelectedIndex = 0;



		}

		private void IssueTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			#region not required --> being refactored
			/*
			switch (issueTypeComboBox.SelectedItem)
			{

				case "Lines Mechanical":
					buildingComboBox.IsEnabled = true;


					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please Choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;
					//update faulty area combobox

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please choose");
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
					issueComboBox.Items.Insert(0, "Please choose");
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
					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;

					//update issue code combobox values

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Conveyer belt");
					issueComboBox.Items.Add("Power sockets");

					issueComboBox.SelectedIndex = 0;


					break;

				case "MHE Equipment":

					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please Choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;


					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Choose option");
					faultyAreaComboBox.Items.Add("Flexi");
					faultyAreaComboBox.Items.Add("Scissor Lift");
					faultyAreaComboBox.Items.Add("PPT");
					faultyAreaComboBox.Items.Add("Electric FLT");
					faultyAreaComboBox.Items.Add("Gas FLT");
					faultyAreaComboBox.Items.Add("Pump truck");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;



					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Waiting for faulty area");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;

					break;

				case "Rackings":
					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;
					faultyAreaComboBox.IsEnabled = false;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Other");
					issueComboBox.SelectedIndex = 1;
					issueComboBox.IsEnabled = false;

					break;

				case "Security":

					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Loading Bays");
					faultyAreaComboBox.Items.Add("Roller Doors");
					faultyAreaComboBox.Items.Add("Alarm");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;

					issueComboBox.Items.Clear();

					issueComboBox.Items.Insert(0, "Waiting for faulty area");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;


					break;

				case "Drains":

					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Indoor");
					faultyAreaComboBox.Items.Add("Outdoor");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please Choose");
					issueComboBox.Items.Add("Other");
					issueComboBox.SelectedIndex = 1;
					issueComboBox.IsEnabled = false;

					break;

				case "Office Equipment":


					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();

					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;
					faultyAreaComboBox.IsEnabled = false;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Other");
					issueComboBox.SelectedIndex = 1;
					issueComboBox.IsEnabled = false;

					break;

				case "Cleaning Equipment":
					buildingComboBox.IsEnabled = true;
					buildingComboBox.Items.Clear();
					buildingComboBox.Items.Insert(0, "Please choose");
					buildingComboBox.Items.Add("Building 1");
					buildingComboBox.Items.Add("Building 2");
					buildingComboBox.SelectedIndex = 0;

					faultyAreaComboBox.Items.Clear();

					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;
					faultyAreaComboBox.IsEnabled = false;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Floor scrubber");
					issueComboBox.Items.Add("Ride On floor scrubber");
					issueComboBox.SelectedIndex = 0;
					issueComboBox.IsEnabled = false;


					break;


				default:

					//buildingComboBox.Items.Clear();
					//buildingComboBox.Items.Insert(0, "Waiting for issue type");
					//buildingComboBox.SelectedIndex = 0;



					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Waiting for issue type");
					faultyAreaComboBox.SelectedIndex = 0;

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Waiting for issue type");
					faultyAreaComboBox.SelectedIndex = 0;


					break;
									   					 				  				  
			}


	*/
			#endregion


		}

		private void BuildingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			if (issueTypeComboBox.SelectedItem != null)
			{


				if (issueTypeComboBox.SelectedItem.ToString() == "Lines Mechanical")
				{
					faultyAreaComboBox.Items.Clear();


					switch (buildingComboBox.SelectedItem)
					{

						case "Building 1":
							faultyAreaComboBox.IsEnabled = true;
							faultyAreaComboBox.Items.Clear();
							faultyAreaComboBox.Items.Insert(0, "Choose option");
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
							issueComboBox.Items.Insert(0, "Waiting for faulty area");
							issueComboBox.SelectedIndex = 0;
							issueComboBox.IsEnabled = false;


							break;

						case "Building 2":
							faultyAreaComboBox.IsEnabled = true;
							faultyAreaComboBox.Items.Clear();
							faultyAreaComboBox.Items.Insert(0, "Choose option");
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
							issueComboBox.Items.Insert(0, "Waiting for faulty area");
							issueComboBox.SelectedIndex = 0;
							issueComboBox.IsEnabled = false;
							break;

						default:
							faultyAreaComboBox.IsEnabled = true;

							break;


					}




				}
				else if (issueTypeComboBox.SelectedItem.ToString() == "MHE Equipment")
				{
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Choose option");
					faultyAreaComboBox.Items.Add("Flexi");
					faultyAreaComboBox.Items.Add("Scissor Lift");
					faultyAreaComboBox.Items.Add("PPT");
					faultyAreaComboBox.Items.Add("Electric FLT");
					faultyAreaComboBox.Items.Add("Gas FLT");
					faultyAreaComboBox.Items.Add("Pump truck");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;
				}
				else if (issueTypeComboBox.SelectedItem.ToString() == "Rackings")
				{
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please Choose12");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;
					faultyAreaComboBox.IsEnabled = false;
					issueComboBox.IsEnabled = false;
				}
				else if (issueTypeComboBox.SelectedItem.ToString() == "Projects")
				{
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please Choose");
					faultyAreaComboBox.Items.Add("Projects");
					faultyAreaComboBox.SelectedIndex = 1;
					issueComboBox.IsEnabled = false;
				}
				else if (issueTypeComboBox.SelectedItem.ToString() == "Security")
				{
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Loading Bays");
					faultyAreaComboBox.Items.Add("Roller Doors");
					faultyAreaComboBox.Items.Add("Alarm");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;

				}

				else if (issueTypeComboBox.SelectedItem.ToString() == "Drains")
				{
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please choose");
					faultyAreaComboBox.Items.Add("Indoor");
					faultyAreaComboBox.Items.Add("Outdoor");
					faultyAreaComboBox.SelectedIndex = 0;
					faultyAreaComboBox.IsEnabled = true;
				}

				else if (issueTypeComboBox.SelectedItem.ToString() == "Office Equipment")
				{

					faultyAreaComboBox.Items.Clear();

					faultyAreaComboBox.Items.Insert(0, "Please Choose");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;
					faultyAreaComboBox.IsEnabled = false;
					issueComboBox.IsEnabled = false;
				}

				else if (issueTypeComboBox.SelectedItem.ToString() == "Boxing")
				{

					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please Choose");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;

				}
				else if (issueTypeComboBox.SelectedItem.ToString() == "Cleaning Equipment")
				{
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please Choose");
					faultyAreaComboBox.Items.Add("Other");
					faultyAreaComboBox.SelectedIndex = 1;
				}
				else
				{
					faultyAreaComboBox.Items.Clear();
					faultyAreaComboBox.Items.Insert(0, "Please choose it");
					faultyAreaComboBox.SelectedIndex = 0;
				}


			}
		}

		private void FaultyAreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			issueComboBox.IsEnabled = true;

			switch (faultyAreaComboBox.SelectedItem)
			{

				case "Flexi":
				case "Scissor Lift":
				case "PPT":
				case "Electric FLT":
				case "Gas FLT":
				case "Pump truck":


					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Hydraulics");
					issueComboBox.Items.Add("Load Wheels");
					issueComboBox.Items.Add("Caster Wheels");
					issueComboBox.Items.Add("Drive Wheels");
					issueComboBox.Items.Add("Battery");
					issueComboBox.Items.Add("Forks");
					issueComboBox.Items.Add("Pins");
					issueComboBox.Items.Add("Access Code");
					issueComboBox.Items.Add("Electrical Issue");
					issueComboBox.Items.Add("Hose Pipe");
					issueComboBox.Items.Add("Oil leak");
					issueComboBox.Items.Add("Gas leak");
					issueComboBox.Items.Add("Other");
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
					issueComboBox.Items.Insert(0, "Please choose");
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
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Bay 1");
					issueComboBox.Items.Add("Bay 2");
					issueComboBox.Items.Add("Bay 3");
					issueComboBox.Items.Add("Bay 4");
					issueComboBox.SelectedIndex = 0;

					break;

				case "Alarm":

					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
					issueComboBox.Items.Add("Intruder alarm");
					issueComboBox.Items.Add("Fire Alarm");
					issueComboBox.SelectedIndex = 0;

					break;

				case "Roller Doors":
					issueComboBox.Items.Clear();
					issueComboBox.Items.Insert(0, "Please choose");
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
					issueComboBox.Items.Insert(0, "Please Choose");
					issueComboBox.Items.Add("Other");
					issueComboBox.SelectedIndex = 1;
					break;




				default:




					break;
			}

		}
		#endregion



		#region dynamic ComboBoxSetup - initialization at launch of application
		public void SetUpComboBox()
		{
			//refactoring ComboBox Setup


			//TODO: set up database connection for getting issue codes etc.

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

			//PriorityComboBox - static --> still dependant on settings window
			var getPriorityComboBox = db.SetUpComboBox("PriorityComboBox");
			PriorityComboBox.Items.Add("Please Select");
			while (getPriorityComboBox.Read())
			{
				PriorityComboBox.Items.Add(getPriorityComboBox[1]);
			}
			PriorityComboBox.SelectedIndex = 0;

			//buildingComboBox - static --> still dependant on settings window

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

			//faultyAreaComboBo - dynamic
			faultyAreaComboBox.Items.Add("Waiting for data");
			faultyAreaComboBox.SelectedIndex = 0;


			//issueComboBox - dynamic
			issueComboBox.Items.Add("Waiting for data");
			issueComboBox.SelectedIndex = 0;


			//end of part for refactoring
		}
		#endregion

		public void GetJobNumber()
		{

			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}


			int lastJob = db.DBQueryLastJobNumber("JobNumber") + 1;
			jobNumberDataLabel.Content = lastJob.ToString();




		}

		private void InsertDataIntoDatabase_Click(object sender, RoutedEventArgs e)
		{





			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}





			int getLastJobNumber = db.DBQueryLastJobNumber("JobNumber");

			DateTime dueDate = DateTime.Now.AddDays(float.Parse(userrSet.DueDateGap));


			string dtNow = dueDate.ToString("dd/MM/yyyy");

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

				db.InsertDataIntoDatabase(issue.JobNumber, issue.ReportedDate, issue.ReportedTime, issue.ReportedUserName, issue.AssetNumber, issue.FaulyArea, issue.Building, issue.Code, issue.Priority, issue.Type, issue.DetailedDescription, issue.DueDate, issue.Area, issue.ReporterEmail);




				this.Close();

			}
			else
			{
				ErrorMessageLabel.Visibility = Visibility.Visible;


			}





		}
		private void ClearRichTextBox(object sender, RoutedEventArgs e)
		{
			((RichTextBox)sender).Document = new FlowDocument();
		}
		private void CheckRichTextBoxchanges(object sender, TextChangedEventArgs e)
		{
			richTextBoxTextChanged = true;
		}
		public bool CheckFilledData()
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
			#endregion

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


	}





}

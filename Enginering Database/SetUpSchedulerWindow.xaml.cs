using System;
using System.Collections.Generic;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for SetUpSchedulerWindow.xaml
	/// </summary>
	public partial class SetUpSchedulerWindow : Window
	{
		private DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();

		public SetUpSchedulerWindow()
		{
			InitializeComponent();
			setUpWasteStreams();
		}

		private void setUpWasteStreams()
		{
			try
			{
				db.ConnectDB();

				WasteStreamComboBox.Items.Add("Please select");
				var reader = db.GetAllPDFIds("WasteStreams");

				while (reader.Read())
				{
					WasteStreamComboBox.Items.Add(reader["WasteStreamDescription"]);
				}
				WasteStreamComboBox.SelectedIndex = 0;

				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void SetUpButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				DateTime selectedDate;

				if (DayOfTheWeekComboBox.SelectedIndex != 0 && WasteStreamComboBox.SelectedIndex != 0 && CollectionFrequencyTextBox.Text != string.Empty && WeeklyMonthlyComboBox.SelectedIndex != 0)
				{
					if (int.TryParse(CollectionsTextBox.Text, out int p))
					{
						List<CollectionSchedulerClass> collectionList = new List<CollectionSchedulerClass>();

						for (int i = 0; i < p; i++)
						{
							CollectionSchedulerClass Collectionclass = new CollectionSchedulerClass();

							Collectionclass.WasteStream = WasteStreamComboBox.SelectedItem.ToString();
							Collectionclass.DayOfWeekCollection = DayOfTheWeekComboBox.SelectedItem.ToString();
							Collectionclass.CollectionFrequency = $"{CollectionFrequencyTextBox.Text} {i}";
							Collectionclass.WeeklyMonthly = WeeklyMonthlyComboBox.SelectedItem.ToString();
							if (OnRequestCheckBox.IsChecked == true)
							{
								Collectionclass.OnRequest = "Yes";
							}
							else
							{
								Collectionclass.OnRequest = "No";
							}
							if (i > 0)
							{
								selectedDate = collectionList[i - 1].CollectionDate;
								selectedDate = selectedDate.AddDays(7);
							}
							else
							{
								selectedDate = FirstCollectionDateDatePicker.SelectedDate.Value.Date;
							}
							//get closest required date

							Collectionclass.CollectionDate = selectedDate;

							collectionList.Add(Collectionclass);
						}

						foreach (var item in collectionList)
						{
							Console.WriteLine(String.Format("{0:d}", item.CollectionDate) + $" --> {item.CollectionFrequency.ToString()}");
						}
					}
					else
					{
						System.Windows.MessageBox.Show("Not Number");
					}
				}
				else
				{
					System.Windows.MessageBox.Show("Please fill all required fields");
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
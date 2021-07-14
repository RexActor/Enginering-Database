using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for StatutoryItem.xaml
	/// </summary>
	public partial class StatutoryItem : Window
	{
		private bool correctReportDate = false;
		private bool correctRenewDate = false;
		private bool ReportDateWasChanged = false;
		private bool RenewDateWasChanged = false;
		private bool bookedStatusChanged = false;
		private bool editMode = false;
		private byte[] _fileToUpload;

		// readonly private bool uploaded = false;
		private string _statutoryReportSubFolder = "21 - Service Reports";

		private string _saveLocationDir;

		private bool manufacturedChanged = false;
		private bool companyInsurerChanged = false;
		private bool serialNumberChanged = false;
		private bool monthlyWeeklyChanged = false;
		private DateTime currentReportDate;

		private readonly DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();

		public StatutoryItem()
		{
			InitializeComponent();

			RenewDateInfoLabel.Visibility = Visibility.Hidden;
			ReportIssuedInfoLabel.Visibility = Visibility.Hidden;
			SerialNumberUploadStatus.Visibility = Visibility.Hidden;
			ManufacturerCompanyUploadStatus.Visibility = Visibility.Hidden;
			CompanyInsurerUploadStatus.Visibility = Visibility.Hidden;
			MonthlyWeeklyUploadStatus.Visibility = Visibility.Hidden;
			MonthlyWeeklyComboBoxUploadStatus.Visibility = Visibility.Hidden;
			BookedInfoLabel.Visibility = Visibility.Hidden;
			MonthlyWeeklyChangeComboBox.Visibility = Visibility.Hidden;
			UploadFilePath.Visibility = Visibility.Hidden;
			SelectUploadFile.Visibility = Visibility.Hidden;
			FileSaveLocation.Visibility = Visibility.Hidden;

			//not needed to be shown. required just for pulling data to update database
			hiddenID.Visibility = Visibility.Hidden;
			hiddenInspectionCount.Visibility = Visibility.Hidden;
		}

		private void StatutoryItemUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				currentReportDate = DateReportIssuedDatePicker.SelectedDate.Value.Date;
				if (!editMode)
				{
					editMode = true;
					StatutoryItemUpdateButton.Content = "Save";

					//activating textboxes for edit mode
					DateReportIssuedDatePicker.IsEnabled = true;
					RenewDateDatePicker.IsEnabled = true;
					ManufacturerTextBox.IsEnabled = true;
					InsurerTextBox.IsEnabled = true;
					SerialNumberTextBox.IsEnabled = true;
					MonthlyWeeklyTextBox.IsEnabled = true;
					BookedCheckBox.IsEnabled = true;

					ManufacturerCompanyUploadStatus.Visibility = Visibility.Hidden;
					CompanyInsurerUploadStatus.Visibility = Visibility.Hidden;
					SerialNumberUploadStatus.Visibility = Visibility.Hidden;
					MonthlyWeeklyUploadStatus.Visibility = Visibility.Hidden;
					ReportIssuedInfoLabel.Visibility = Visibility.Hidden;
					RenewDateInfoLabel.Visibility = Visibility.Hidden;

					MonthlyWeeklyChangeComboBox.Visibility = Visibility.Visible;
					MonthlyWeeklyRangeLabelContent.Visibility = Visibility.Hidden;

					UpdateMonthlyWeeklyComboBox();
					MonthlyWeeklyChangeComboBox.SelectedItem = MonthlyWeeklyRangeLabelContent.Content;

					//marking which text boxes are active for change
					DateReportIssuedDatePicker.Background = Brushes.PeachPuff;
					RenewDateDatePicker.Background = Brushes.PeachPuff;
					ManufacturerTextBox.Background = Brushes.PeachPuff;
					InsurerTextBox.Background = Brushes.PeachPuff;
					SerialNumberTextBox.Background = Brushes.PeachPuff;
					MonthlyWeeklyTextBox.Background = Brushes.PeachPuff;
				}
				else
				{
					//update database with changes

					//before updating Statutory item. check if file is being uploaded
					//if file being uploaded - let trhrough!
					//TODO - maybe check on file content and search for specific words?

					db.ConnectDB();

					if (manufacturedChanged)
					{
						string value = ManufacturerTextBox.Text.ToString();
						db.UpdateStatutoryCompliance("StatutoryCompliance", "ManufacturerCompany", Convert.ToInt32(hiddenID.Content), value);

						ManufacturerCompanyUploadStatus.Foreground = Brushes.Green;
						ManufacturerCompanyUploadStatus.FontWeight = FontWeights.Bold;
						ManufacturerCompanyUploadStatus.Content = "Uploaded";
						ManufacturerCompanyUploadStatus.Visibility = Visibility.Visible;
					}
					else
					{
						ManufacturerCompanyUploadStatus.Foreground = Brushes.Red;
						ManufacturerCompanyUploadStatus.FontWeight = FontWeights.Bold;
						ManufacturerCompanyUploadStatus.Content = "Not Uploaded";
						ManufacturerCompanyUploadStatus.Visibility = Visibility.Visible;
					}

					if (companyInsurerChanged)
					{
						string value = InsurerTextBox.Text.ToString();
						db.UpdateStatutoryCompliance("StatutoryCompliance", "CompanyInsurer", Convert.ToInt32(hiddenID.Content), value);

						CompanyInsurerUploadStatus.Foreground = Brushes.Green;
						CompanyInsurerUploadStatus.FontWeight = FontWeights.Bold;
						CompanyInsurerUploadStatus.Content = "Uploaded";
						CompanyInsurerUploadStatus.Visibility = Visibility.Visible;
					}
					else
					{
						CompanyInsurerUploadStatus.Foreground = Brushes.Red;
						CompanyInsurerUploadStatus.FontWeight = FontWeights.Bold;
						CompanyInsurerUploadStatus.Content = "Not Uploaded";
						CompanyInsurerUploadStatus.Visibility = Visibility.Visible;
					}

					if (serialNumberChanged)
					{
						string value = SerialNumberTextBox.Text.ToString();
						db.UpdateStatutoryCompliance("StatutoryCompliance", "SerialNumber", Convert.ToInt32(hiddenID.Content), value);

						SerialNumberUploadStatus.Foreground = Brushes.Green;
						SerialNumberUploadStatus.FontWeight = FontWeights.Bold;
						SerialNumberUploadStatus.Content = "Uploaded";
						SerialNumberUploadStatus.Visibility = Visibility.Visible;
					}
					else
					{
						SerialNumberUploadStatus.Foreground = Brushes.Red;
						SerialNumberUploadStatus.FontWeight = FontWeights.Bold;
						SerialNumberUploadStatus.Content = "Not Uploaded";
						SerialNumberUploadStatus.Visibility = Visibility.Visible;
					}

					if (MonthlyWeeklyChangeComboBox.SelectedIndex > 0 && MonthlyWeeklyChangeComboBox.SelectedItem.ToString() != MonthlyWeeklyRangeLabelContent.Content.ToString())
					{
						db.UpdateStatutoryCompliance("StatutoryCompliance", "MonthlyWeeklyRange", Convert.ToInt32(hiddenID.Content), MonthlyWeeklyChangeComboBox.SelectedItem.ToString());

						MonthlyWeeklyComboBoxUploadStatus.Foreground = Brushes.Green;
						MonthlyWeeklyComboBoxUploadStatus.FontWeight = FontWeights.Bold;
						MonthlyWeeklyComboBoxUploadStatus.Content = "Uploaded";
						MonthlyWeeklyComboBoxUploadStatus.Visibility = Visibility.Visible;
					}
					else
					{
						MonthlyWeeklyComboBoxUploadStatus.Foreground = Brushes.Red;
						MonthlyWeeklyComboBoxUploadStatus.FontWeight = FontWeights.Bold;
						MonthlyWeeklyComboBoxUploadStatus.Content = "Not Uploaded";
						MonthlyWeeklyComboBoxUploadStatus.Visibility = Visibility.Visible;
					}

					if (monthlyWeeklyChanged)
					{
						string value = MonthlyWeeklyTextBox.Text.ToString();
						db.UpdateStatutoryCompliance("StatutoryCompliance", "MonthlyWeekly", Convert.ToInt32(hiddenID.Content), value);

						MonthlyWeeklyUploadStatus.Foreground = Brushes.Green;
						MonthlyWeeklyUploadStatus.FontWeight = FontWeights.Bold;
						MonthlyWeeklyUploadStatus.Content = "Uploaded";
						MonthlyWeeklyUploadStatus.Visibility = Visibility.Visible;
					}
					else
					{
						MonthlyWeeklyUploadStatus.Foreground = Brushes.Red;
						MonthlyWeeklyUploadStatus.FontWeight = FontWeights.Bold;
						MonthlyWeeklyUploadStatus.Content = "Not Uploaded";
						MonthlyWeeklyUploadStatus.Visibility = Visibility.Visible;
					}

					if (RenewDateWasChanged)
					{
						if (correctRenewDate)
						{
							//check for file upload here
							//File upload should be checked only when change renew date
							//no renew date accepted if there are no file uploaded
							//possible check of file content to avoid blank file uploads

							if (_fileToUpload != null && _fileToUpload.Length > 0)
							{
								db.UpdateStatutoryCompliance("StatutoryCompliance", "UploadedFile", Convert.ToInt32(hiddenID.Content), _fileToUpload);

								string cwd = Directory.GetCurrentDirectory();
								string combined_path = Path.Combine(cwd, _statutoryReportSubFolder);
								string[] _fileName = UploadFilePath.Text.Split('\\');
								string desired_location = Path.Combine(combined_path, _saveLocationDir, _fileName[_fileName.Length - 1]);

								File.Copy(UploadFilePath.Text, desired_location, true);
							}
							else
							{
								return;
							}

							int updateInspectionCount = Convert.ToInt32(hiddenInspectionCount.Content) + 1;
							db.UpdateStatutoryCompliance("StatutoryCompliance", "RenewDate", Convert.ToInt32(hiddenID.Content), Convert.ToDateTime(RenewDateDatePicker.SelectedDate.Value.Date));
							db.UpdateStatutoryCompliance("StatutoryCompliance", "InspectionCount", Convert.ToInt32(hiddenID.Content), updateInspectionCount);

							RenewDateInfoLabel.Foreground = Brushes.Green;
							RenewDateInfoLabel.FontWeight = FontWeights.Bold;
							RenewDateInfoLabel.Content = "Uploaded";
							RenewDateInfoLabel.Visibility = Visibility.Visible;
							db.UpdateStatutoryCompliance("StatutoryCompliance", "MeetingSet", Convert.ToInt32(hiddenID.Content), false);

							UploadFilePath.Visibility = Visibility.Hidden;
							SelectUploadFile.Visibility = Visibility.Hidden;
						}
						else
						{
							RenewDateInfoLabel.Foreground = Brushes.Red;
							RenewDateInfoLabel.FontWeight = FontWeights.Bold;
							RenewDateInfoLabel.Content = "Not Uploaded";
							RenewDateInfoLabel.Visibility = Visibility.Visible;
						}
					}
					else
					{
						RenewDateInfoLabel.Foreground = Brushes.Red;
						RenewDateInfoLabel.Content = "Not Uploaded";
						RenewDateInfoLabel.Visibility = Visibility.Visible;
					}

					if (ReportDateWasChanged)
					{
						if (correctReportDate)
						{
							db.UpdateStatutoryCompliance("StatutoryCompliance", "DateReportIssued", Convert.ToInt32(hiddenID.Content), Convert.ToDateTime(DateReportIssuedDatePicker.SelectedDate.Value.Date));
							if (BookedCheckBox.IsChecked == true)
							{
								db.UpdateStatutoryCompliance("StatutoryCompliance", "Booked", Convert.ToInt32(hiddenID.Content), "No");
								BookedCheckBox.IsChecked = false;
								bookedStatusChanged = true;
							}

							ReportIssuedInfoLabel.Foreground = Brushes.Green;
							ReportIssuedInfoLabel.Content = "Uploaded";
							ReportIssuedInfoLabel.Visibility = Visibility.Visible;
						}
						else
						{
							ReportIssuedInfoLabel.Foreground = Brushes.Red;
							ReportIssuedInfoLabel.Content = "Not Uploaded";
							ReportIssuedInfoLabel.Visibility = Visibility.Visible;
						}
					}
					else
					{
						ReportIssuedInfoLabel.Foreground = Brushes.Red;
						ReportIssuedInfoLabel.Content = "Not Uploaded";
						ReportIssuedInfoLabel.Visibility = Visibility.Visible;
					}

					if (bookedStatusChanged)
					{
						if (BookedCheckBox.IsChecked == true)
						{
							db.UpdateStatutoryCompliance("StatutoryCompliance", "Booked", Convert.ToInt32(hiddenID.Content), "Yes");
						}
						else
						{
							db.UpdateStatutoryCompliance("StatutoryCompliance", "Booked", Convert.ToInt32(hiddenID.Content), "No");
						}
						BookedInfoLabel.Foreground = Brushes.Green;
						BookedInfoLabel.Content = "Uploaded";
						BookedInfoLabel.Visibility = Visibility.Visible;
					}
					else
					{
						BookedInfoLabel.Foreground = Brushes.Red;
						BookedInfoLabel.Content = "Not Uploaded";
						BookedInfoLabel.Visibility = Visibility.Visible;
					}

					db.CloseDB();

					//deactivating textBoxes

					DateReportIssuedDatePicker.IsEnabled = false;
					RenewDateDatePicker.IsEnabled = false;
					ManufacturerTextBox.IsEnabled = false;
					InsurerTextBox.IsEnabled = false;
					SerialNumberTextBox.IsEnabled = false;
					MonthlyWeeklyTextBox.IsEnabled = false;
					BookedCheckBox.IsEnabled = false;

					MonthlyWeeklyChangeComboBox.Visibility = Visibility.Hidden;
					MonthlyWeeklyRangeLabelContent.Content = MonthlyWeeklyChangeComboBox.SelectedItem.ToString();

					MonthlyWeeklyRangeLabelContent.Visibility = Visibility.Visible;

					//changing back background colour to default
					DateReportIssuedDatePicker.Background = Brushes.Transparent;
					RenewDateDatePicker.Background = Brushes.Transparent;
					ManufacturerTextBox.Background = Brushes.Transparent;
					InsurerTextBox.Background = Brushes.Transparent;
					SerialNumberTextBox.Background = Brushes.Transparent;
					MonthlyWeeklyTextBox.Background = Brushes.Transparent;

					editMode = false;

					StatutoryItemUpdateButton.Content = "Edit Mode";
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateMonthlyWeeklyComboBox()
		{
			try
			{
				db.ConnectDB();
				var reader = db.GetAllPDFIds("WeeklyMonthlyRange");
				MonthlyWeeklyChangeComboBox.Items.Add("Please Select");
				while (reader.Read())
				{
					MonthlyWeeklyChangeComboBox.Items.Add(reader["WeeklyMonthlyRange"].ToString());
				}

				MonthlyWeeklyChangeComboBox.SelectedIndex = 0;
				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void RenewDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				RenewDateInfoLabel.Visibility = Visibility.Hidden;
				if (editMode)
				{
					UploadFilePath.Visibility = Visibility.Visible;
					SelectUploadFile.Visibility = Visibility.Visible;

					RenewDateWasChanged = true;

					TimeSpan daysTillNewRenew = RenewDateDatePicker.SelectedDate.Value.Date - DateTime.Now.Date;
					RenewDateInfoLabel.Foreground = Brushes.Green;
					RenewDateInfoLabel.FontWeight = FontWeights.Bold;
					RenewDateInfoLabel.Content = $"{daysTillNewRenew.Days} day/s left till next inspection";
					RenewDateInfoLabel.Visibility = Visibility.Visible;
					correctRenewDate = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void DateReportIssuedDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				ReportIssuedInfoLabel.Visibility = Visibility.Hidden;

				if (editMode)
				{
					ReportDateWasChanged = true;

					if (DateReportIssuedDatePicker.SelectedDate.Value.Date == currentReportDate)
					{
						correctReportDate = false;
					}
					else
					{
						switch (MonthlyWeeklyChangeComboBox.SelectedItem.ToString())
						{
							case "Yearly":

								//Do calculation by adding specific amount of Years
								RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddYears(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

								break;

							case "Monthly":

								//Do calculation by adding specific amount of Months

								RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddMonths(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

								break;

							case "Weekly":
								//Do calculation by adding specific amount of Weeks

								RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text) * 7);

								break;

							case "Daily":

								//Do calculation by adding specific amount of Days
								RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

								break;
						}

						correctReportDate = true;
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ManufacturerTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (editMode)
				{
					manufacturedChanged = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void InsurerTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (editMode)
				{
					companyInsurerChanged = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void SerialNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (editMode)
				{
					serialNumberChanged = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void MonthlyWeeklyTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (editMode)
				{
					monthlyWeeklyChanged = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void MonthlyWeeklyChangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (editMode)
				{
					switch (MonthlyWeeklyChangeComboBox.SelectedItem.ToString())
					{
						case "Yearly":

							//Do calculation by adding specific amount of Years
							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddYears(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

							break;

						case "Monthly":

							//Do calculation by adding specific amount of Months

							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddMonths(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

							break;

						case "Weekly":
							//Do calculation by adding specific amount of Weeks

							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text) * 7);

							break;

						case "Daily":

							//Do calculation by adding specific amount of Days
							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

							break;
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void BookedCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				bookedStatusChanged = true;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void SelectUploadFile_Click(object sender, RoutedEventArgs e)
		{
			// _uploadFile =
			_fileToUpload = null;
			OpenFileDialog _fileDialog = new OpenFileDialog();
			_fileDialog.DefaultExt = ".pdf"; //required extension
			_fileDialog.Filter = "PDF Documents (.pdf)|*.pdf|All Files (*.*)|*.*";
			_fileDialog.RestoreDirectory = true;

			if (_fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				UploadFilePath.Text = _fileDialog.FileName;

				_fileToUpload = File.ReadAllBytes(UploadFilePath.Text);
				GetDirectoryContent();
			}
		}

		private void GetDirectoryContent()
		{
			FileSaveLocation.Visibility = Visibility.Visible;

			if (FileSaveLocation.Items.Count > 0)
			{
				FileSaveLocation.Items.Clear();
			}
			string cwd = Directory.GetCurrentDirectory();
			string combined_path = Path.Combine(cwd, _statutoryReportSubFolder);
			string[] cwdcontent = Directory.GetDirectories(combined_path);

			foreach (var item in cwdcontent)
			{
				string[] _breakdown = item.Split('\\');

				FileSaveLocation.Items.Add(_breakdown[_breakdown.Length - 1].ToString());
			}
		}

		private void UploadFilePath_Drop(object sender, System.Windows.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
			{
				string[] _files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);

				_fileToUpload = File.ReadAllBytes(_files[0]);
				//System.Windows.MessageBox.Show(_fileToUpload.Length.ToString());

				UploadFilePath.Text = _files[0];
			}
			else if (e.Data.GetDataPresent("FileGroupDescriptor"))
			{
				Stream _stream = (Stream)e.Data.GetData("FileGroupDescriptor");
				byte[] _fileGroupDescriptor = new byte[512];
				_stream.Read(_fileGroupDescriptor, 0, 512);
				StringBuilder fileName = new StringBuilder("");
				for (int i = 76; _fileGroupDescriptor[i] != 0; i++)
				{
					fileName.Append(Convert.ToChar(_fileGroupDescriptor[i]));
				}
				_stream.Close();

				//string _path = string.Concat(Directory.GetCurrentDirectory(), "/Temp/");
				string _path = Path.GetTempPath();
				//string _path = Path.Combine(Directory.GetCurrentDirectory, "Temp");
				string _theFile = _path + fileName.ToString();

				MemoryStream ms = (MemoryStream)e.Data.GetData("FileContents", true);
				byte[] fileBytes = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(fileBytes, 0, (int)ms.Length);
				FileStream fs = new FileStream(_theFile, FileMode.Create);
				fs.Write(fileBytes, 0, (int)fileBytes.Length);
				fs.Close();

				FileInfo tempFile = new FileInfo(_theFile);
				if (tempFile.Exists == true)
				{
					UploadFilePath.Text = _theFile;
					_fileToUpload = File.ReadAllBytes(_theFile);
				}
			}

			GetDirectoryContent();
		}

		private void UploadFilePath_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
		{
			e.Handled = true;
		}

		private void FileSaveLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (FileSaveLocation.SelectedItem.ToString() != string.Empty)
			{
				_saveLocationDir = FileSaveLocation.SelectedItem.ToString();
			}
		}
	}
}
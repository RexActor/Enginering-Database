using System;
using System.Security.Principal;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for ReportAppIssue.xaml
	/// </summary>
	///

	public partial class ReportAppIssue : Window
	{
		private ErrorSystem err = new ErrorSystem();

		public ReportAppIssue()
		{
			InitializeComponent();
			DateTime timeNow = DateTime.Now;
			TimeData.Content = timeNow.ToString("HH:mm");
			DateLabelData.Content = timeNow.ToString("dd/mm/yyyy");
			UserNameReporting.Content = WindowsIdentity.GetCurrent().Name;
		}

		private void ReportAppIssueSubmitButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EmailClass email = new EmailClass();
				email.ReportAppISsueEmail(DateLabelData.Content.ToString(), TimeData.Content.ToString(), UserNameReporting.Content.ToString(), AppIssueReportTextBox.Text);
				this.Close();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}
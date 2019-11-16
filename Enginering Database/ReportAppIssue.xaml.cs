using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for ReportAppIssue.xaml
	/// </summary>
	/// 

	


	public partial class ReportAppIssue : Window
	{

	


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
		

			EmailClass email = new EmailClass();
			email.ReportAppISsueEmail(DateLabelData.Content.ToString(), TimeData.Content.ToString(), UserNameReporting.Content.ToString(), AppIssueReportTextBox.Text);
		}
	}
}

using System;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for LineMaintenance.xaml
	/// </summary>
	public partial class LineMaintenance : Window
	{
#pragma warning disable CS0414 // The field 'LineMaintenance.test' is assigned but its value is never used
		string test = "test";
#pragma warning restore CS0414 // The field 'LineMaintenance.test' is assigned but its value is never used
		string currentMonth = string.Empty;
		string currentYear = string.Empty;
		string lineNumber = string.Empty;
		public LineMaintenance()
		{

			InitializeComponent();

			Random rnd = new Random();
			int randomNumber = rnd.Next(0, 8);
			//testTextBlock.Text = $"test {randomNumber}";
			DateTime dt = DateTime.Now;
			LineNumberTextBlock.Text = $"Line {randomNumber}";
			currentMonth = dt.ToString("MMMM");
			currentYear = dt.ToString("yyy");
			MonthTextBlock.Text = currentMonth;
			YearTextBlock.Text = currentYear;
		}
	}
}

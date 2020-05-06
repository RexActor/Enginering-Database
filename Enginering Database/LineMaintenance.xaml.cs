using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for LineMaintenance.xaml
	/// </summary>
	public partial class LineMaintenance : Window
	{
		string test = "test";
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

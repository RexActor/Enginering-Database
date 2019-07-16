using System;
using System.Data;
using System.IO;
using System.Windows;

namespace Enginering_Database
{
	/// <summary>
	/// Interaction logic for viewDatabase.xaml
	/// </summary>
	public partial class viewDatabase : Window
	{
		public static int ldata = 0;
		public viewDatabase()


		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			InitializeComponent();
			viewdataGrid();
			this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.9;
			this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
			this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		}


		public void viewdataGrid()
		{
			Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
			Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(Directory.GetCurrentDirectory() + @"\database.xlsx");
			Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1);
			Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

			string strCellData = "";
			double douCellData;
			int rowCnt = 0;
			int colCnt = 0;


			DataTable dt = new DataTable();
			for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
			{
				string strColumn = "";
				strColumn = (string)(excelRange.Cells[1, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
				dt.Columns.Add(strColumn, typeof(String));

			}
			for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
			{
				string strData = "";
				for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
				{
					try
					{
						strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
						strData += strCellData + "|";
					}
					catch (Exception ex)
					{
						douCellData = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
						strData += douCellData.ToString() + "|";
					}
					ldata++;
				}
				strData = strData.Remove(strData.Length - 1, 1);
				dt.Rows.Add(strData.Split('|'));

			}

			//dtGrid.ItemsSource = dt.DefaultView;
			excelDataGridData.ItemsSource = dt.DefaultView;

			excelBook.Close(true, null, null);
			excelApp.Quit();
		}


	}
}

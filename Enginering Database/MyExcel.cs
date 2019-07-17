using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Engineering_Database;

namespace Engineering_Database
	{
	class MyExcel
	{
		/*
		//public static BindingList<issuedata> issuedatas = new BindingList<issuedata>();
		
		private static Excel.Workbook MyBook = null;
		private static Excel.Application MyApp = null;
		private static Excel.Worksheet MySheet = null;
		private static int lastRow = 0;
		

		public static BindingList<issuedata> ReadMyExcel ( )
			{
	
			
			empList.Clear();
			for(int index = 2; index <= lastRow; index++)
				{
				System.Array MyValues = (System.Array)MySheet.get_Range("A" + index.ToString(),"T" + index.ToString()).Cells.Value;
				empList.Add(new issuedata
					{
					week = MyValues.GetValue(1,1).ToString(),
					jobNumber = MyValues.GetValue(1,2).ToString(),
					repDate = MyValues.GetValue(1,3).ToString(),
					compDate = MyValues.GetValue(1,4).ToString()
					});
				}
			return empList;

			}

		public static void InitializeExcel ( )
			{
			MyApp = new Excel.Application();
			MyApp.Visible = false;
			MyBook = MyApp.Workbooks.Open(Directory.GetCurrentDirectory()+@"\database.xlsx");
			MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explict cast is not required here
			lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
			}

		public static void WriteToExcel (issuedata emp)
			{
			try
				{
				lastRow += 1;
				MySheet.Cells[lastRow,1] = emp.week;
				MySheet.Cells[lastRow,2] = emp.jobNumber;
				MySheet.Cells[lastRow,3] = emp.repDate;
				MySheet.Cells[lastRow,4] = emp.compDate;
				empList.Add(emp);
				MyBook.Save();
				}
			catch(Exception ex)
				{
				}

			}
		public static List<issuedata> FilterEmpList (string searchValue,string searchExpr)
			{
			List<issuedata> FilteredList = new List<issuedata>();
			switch(searchValue.ToUpper())
				{
				case "WEEK":
					FilteredList = empList.ToList().FindAll(emp => emp.week.ToLower().Contains(searchExpr));
					break;
				case "JOBNUMBER":
					FilteredList = empList.ToList().FindAll(emp => emp.jobNumber.ToLower().Contains(searchExpr));
					break;
					/*
				case "EMPLOYEE_ID":
					FilteredList = empList.ToList().FindAll(emp => emp.Employee_ID.ToLower().Contains(searchExpr));
					break;
				case "EMAIL_ID":
					FilteredList = empList.ToList().FindAll(emp => emp.Email_ID.ToLower().Contains(searchExpr));
					break;
				default:
					break;
				}
			return FilteredList;
			}
*/


		}
	}

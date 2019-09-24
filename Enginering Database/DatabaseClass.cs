using System;

using System.Data.OleDb;

namespace Engineering_Database
{
	class DatabaseClass
	{
		/*
		 * 
		 * 
		 * Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\engineeringDatabase.accdb;Persist Security Info=True
		 * 
		 */


		readonly OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=engineeringDatabase.accdb");
		#region connect DB close DB and DB status DB count lines functions
		public void ConnectDB()
		{




			con.Open();


		}
		public void CloseDB()
		{

			con.Dispose();
			con.Close();
		}

		public string DBStatus()
		{

			if (con.State == System.Data.ConnectionState.Open)
			{

				return "DB connected";
			}
			else if (con.State == System.Data.ConnectionState.Closed)
			{
				return "DB not Connected";
			}

			else

				return "Some kind of error";
		}

		public int DBCountLines()
		{

			string queryString = "SELECT COUNT(*) FROM engineeringDatabaseTable";
			//int count = 0;

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			//OleDbDataReader reader = cmd.ExecuteReader();
			//reader.Read();
			var data = (int)cmd.ExecuteScalar();
			cmd.Dispose();
			return data;
		}
		#endregion
		#region db query functions --> retreive data from database 3 overloaded functions string (table)/string (table) int(jobNumber)/ simple without any parameters
		public string DBQuery(string table)
		{
			string queryString = $"SELECT * FROM engineeringDatabaseTable";

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			reader.Read();
			var data = reader[table].ToString();
			cmd.Dispose();
			return data;

		}
		public string DBQuery(string table, int jobnumber)
		{

			string queryString = $"SELECT * FROM engineeringDatabaseTable WHERE JobNumber in (@param1)";


			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.Add(new OleDbParameter("@param1", jobnumber));

			OleDbDataReader reader = cmd.ExecuteReader();
			if (reader != null)
			{
				reader.Read();
			}

			var data = reader[table].ToString();
			cmd.Dispose();

			//TODO:Check if it will not cause error
			reader.Close();
			return data;
		}
		public OleDbCommand DBQuery()
		{
			string queryString = "SELECT * FROM engineeringDatabaseTable WHERE Action='Action required'";

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			//OleDbDataReader reader = cmd.ExecuteReader();
			//reader.Read();
			//var data = reader[table].ToString();
			//return data;
			return cmd;
		}

		public OleDbCommand DBQueryforJobList(string filter)
		{
			string queryString;
			if (filter == "Outstanding")
			{
				queryString = "SELECT * FROM engineeringDatabaseTable WHERE Action='Action required'";
			}
			else if (filter == "Resolved")
			{
				queryString = "SELECT * FROM engineeringDatabaseTable WHERE Completed=true";
			}
			else
			{
				queryString = "SELECT * FROM engineeringDatabaseTable WHERE Action='Action required'";

			}
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			//OleDbDataReader reader = cmd.ExecuteReader();
			//reader.Read();
			//var data = reader[table].ToString();
			//return data;
			return cmd;
		}


		public OleDbCommand DBQueryForAllLines()
		{
			string queryString = "SELECT * FROM engineeringDatabaseTable";

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			//OleDbDataReader reader = cmd.ExecuteReader();
			//reader.Read();
			//var data = reader[table].ToString();
			//return data;
			return cmd;
		}


		public OleDbCommand DBQueryForViewDatabase(int jobNumber)
		{
			string queryString = $"SELECT * FROM engineeringDatabaseTable WHERE JobNumber ={jobNumber}";

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			//OleDbDataReader reader = cmd.ExecuteReader();
			//reader.Read();
			//var data = reader[table].ToString();
			//return data;
			return cmd;
		}



		/// <summary>
		/// Returns job number which one is reported as last into database
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public int DBQueryLastJobNumber(string table)
		{
			bool LastJobNumberIsNumber;
			int number;
			int LastJobNumber;
			string queryString = "SELECT TOP 1  *  FROM engineeringDatabaseTable ORDER BY JobNumber DESC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();

			if (reader != null)
			{
				if (reader.HasRows)
				{
					reader.Read();
					cmd.Dispose();

				}
				else
				{
					cmd.Dispose();
					return 0;
				}
				reader.Close();
			}

			var data = reader[table].ToString();
			//converts data into integer. If successfull returns value as int, else returns 0 (there will not be job number as 0, so can perform check. 
			//if job number was returned as 0, then there wasn't number in database recorded. 

			LastJobNumberIsNumber = Int32.TryParse(data, out number);

			if (LastJobNumberIsNumber)
			{
				cmd.Dispose();
				LastJobNumber = number;
				return LastJobNumber;
			}
			else
			{
				cmd.Dispose();
				return 0;
			}
		}
		#endregion

		#region updateDatabase with values 3 overloaded functions - bool/string/int
		public void DBQueryInsertData(string table, int jobnumber, bool value)
		{

			string queryString = "UPDATE engineeringDatabaseTable SET " + table + " = @value WHERE JobNumber = @jobnumber";


			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);



			cmd.ExecuteNonQuery();
			cmd.Dispose();

		}
		public void DBQueryInsertData(string table, int jobnumber, string value)
		{

			string queryString = "UPDATE engineeringDatabaseTable SET " + table + " = @value WHERE JobNumber = @jobnumber";


			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);



			cmd.ExecuteNonQuery();
			cmd.Dispose();

		}
		public void DBQueryInsertData(string table, int jobnumber, int value)
		{

			string queryString = "UPDATE engineeringDatabaseTable SET " + table + " = @value WHERE JobNumber = @jobnumber";


			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);



			cmd.ExecuteNonQuery();
			cmd.Dispose();

		}
		public void DBQueryInsertData(int jobnumber, string table, string value)
		{

			//string queryString = "Update engineeringDatabaseTable SET " + table + " = @value,"+table2+"=@value2 WHERE JobNumber = @jobnumber";

			string queryString = "UPDATE engineeringDatabaseTable SET [" + table + "] = @value WHERE [JobNumber] = @jobnumber";

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			//OleDbDataReader reader;


			//cmd.Parameters.AddWithValue("@table", table);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);


			//reader = cmd.ExecuteReader();

			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}

		#endregion

		#region insert values into database
		public void InsertDataIntoDatabase(int jobNumber, string repdate, string repTime, string ReportedUsername, string assetNumber, string faultyArea, string Building, string IssueCode, string Priority, string type, string detailedDesc, string DueDate, string Area)
		{
			string queryString = "INSERT INTO engineeringDatabaseTable (JobNumber,ReportedDate,ReportedTime,ReportedUsername,AssetNumber,FaultyArea,Building,IssueCode,Priority,Type,DetailedDescription,DueDate,Area) Values(@JobNumber,@ReportedDate,@ReportedTime,@ReportedUsername,@AssetNumber,@FaultyArea,@Building,@IssueCode,@Priority,@Type,@DetailedDescription,@DueDate,@Area)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@JobNumber", jobNumber);
			cmd.Parameters.AddWithValue("@ReportedDate", repdate);
			cmd.Parameters.AddWithValue("@ReportedTime", repTime);
			cmd.Parameters.AddWithValue("@ReportedUsername", ReportedUsername);
			cmd.Parameters.AddWithValue("@AssetNumber", assetNumber);
			cmd.Parameters.AddWithValue("@FaultyArea", faultyArea);
			cmd.Parameters.AddWithValue("@Building", Building);
			cmd.Parameters.AddWithValue("@IssueCode", IssueCode);
			cmd.Parameters.AddWithValue("@Priority", Priority);
			cmd.Parameters.AddWithValue("@Type", type);
			cmd.Parameters.AddWithValue("@DetailedDescription", detailedDesc);
			//cmd.Parameters.AddWithValue("@Action", Action);
			cmd.Parameters.AddWithValue("@DueDate", DueDate);
			cmd.Parameters.AddWithValue("@Area", Area);



			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		#endregion

	}
}

﻿using System;
using System.Collections.Generic;

using System.Data;
using System.Data.OleDb;

namespace Engineering_Database
{
	internal class IssueClass
	{
		public int JobNumber { get; set; }
		public string ReportedDate { get; set; }
		public string CompletedDate { get; set; }
		public string ReportedTime { get; set; }
		public string CompletedTime { get; set; }
		public string ReportedUserName { get; set; }
		public string AssetNumber { get; set; }
		public string FaulyArea { get; set; }

		public string Building { get; set; }

		public string Code { get; set; }
		public string Priority { get; set; }
		public string Type { get; set; }
		public bool Completed { get; set; }
		public string DetailedDescription { get; set; }
		public string CompletedByUsername { get; set; }
		public string CommentsForActionsTaken { get; set; }
		public string Action { get; set; }
		public string DueDate { get; set; }
		public string Area { get; set; }
		public string AssignedTo { get; set; }
		public string Contractor { get; set; }
		public string ReportedEmail { get; set; }
		public string ReporterEmail { get; set; }
		public bool LockedOff { get; set; }

		public bool LockOffReported { get; set; }

		public string PersonLockedOff { get; set; }
		public string PersonRemovedLock { get; set; }

		private readonly DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();
		private List<IssueClass> issueDataList = new List<IssueClass>();

		public List<IssueClass> updateIssueDataList()
		{
			try
			{
				db.ConnectDB();
				issueDataList.Clear();
				OleDbDataAdapter upIsDtList = new OleDbDataAdapter(db.DBQueryForAllLines());
				DataTable dt2 = new DataTable();

				upIsDtList.Fill(dt2);

				foreach (DataRow dr in dt2.Rows)
				{
					IssueClass newIs = new IssueClass();
					newIs.JobNumber = (int)dr["JobNumber"];
					newIs.DetailedDescription = dr["DetailedDescription"].ToString();

					newIs.ReportedDate = dr["ReportedDate"].ToString();
					newIs.CompletedDate = dr["CompletedDate"].ToString();
					newIs.ReportedTime = dr["ReportedTime"].ToString();
					newIs.CompletedTime = dr["CompletedTime"].ToString();
					newIs.ReportedUserName = dr["ReportedUsername"].ToString();
					newIs.AssetNumber = dr["AssetNumber"].ToString();
					newIs.FaulyArea = dr["FaultyArea"].ToString();

					newIs.Building = dr["Building"].ToString();

					newIs.Code = dr["IssueCode"].ToString();
					newIs.Priority = dr["Priority"].ToString();
					newIs.Type = dr["Type"].ToString();
					newIs.Completed = (bool)dr["Completed"];
					newIs.DetailedDescription = dr["DetailedDescription"].ToString();
					newIs.CompletedByUsername = dr["CompletedBy"].ToString();
					newIs.CommentsForActionsTaken = dr["CommentsForActionTaken"].ToString();
					newIs.Action = dr["Action"].ToString();
					newIs.DueDate = dr["DueDate"].ToString();
					newIs.Area = dr["Area"].ToString();
					newIs.AssignedTo = dr["AssignedTo"].ToString();
					newIs.Contractor = dr["Contractor"].ToString();
					newIs.ReportedEmail = dr["ReporterEmail"].ToString();

					issueDataList.Add(newIs);
				}

				upIsDtList.Dispose();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
			return issueDataList;
		}

		public List<IssueClass> updateIssueDataListForSpecificJob(int searchJobNumber)
		{
			try
			{
				issueDataList.Clear();
				OleDbDataAdapter upIsDtList = new OleDbDataAdapter(db.DBQueryForViewDatabase(searchJobNumber));
				DataTable dt2 = new DataTable();

				upIsDtList.Fill(dt2);

				foreach (DataRow dr in dt2.Rows)
				{
					IssueClass newIs = new IssueClass();
					newIs.JobNumber = (int)dr["JobNumber"];
					newIs.DetailedDescription = dr["DetailedDescription"].ToString();

					newIs.ReportedDate = dr["ReportedDate"].ToString();
					newIs.CompletedDate = dr["CompletedDate"].ToString();
					newIs.ReportedTime = dr["ReportedTime"].ToString();
					newIs.CompletedTime = dr["CompletedTime"].ToString();
					newIs.ReportedUserName = dr["ReportedUsername"].ToString();
					newIs.AssetNumber = dr["AssetNumber"].ToString();
					newIs.FaulyArea = dr["FaultyArea"].ToString();

					newIs.Building = dr["Building"].ToString();

					newIs.Code = dr["IssueCode"].ToString();
					newIs.Priority = dr["Priority"].ToString();
					newIs.Type = dr["Type"].ToString();
					newIs.Completed = (bool)dr["Completed"];
					newIs.DetailedDescription = dr["DetailedDescription"].ToString();
					newIs.CompletedByUsername = dr["CompletedBy"].ToString();
					newIs.CommentsForActionsTaken = dr["CommentsForActionTaken"].ToString();
					newIs.Action = dr["Action"].ToString();
					newIs.DueDate = dr["DueDate"].ToString();
					newIs.Area = dr["Area"].ToString();
					newIs.AssignedTo = dr["AssignedTo"].ToString();
					newIs.Contractor = dr["Contractor"].ToString();
					newIs.ReportedEmail = dr["ReporterEmail"].ToString();

					issueDataList.Add(newIs);
				}

				issueDataList = issueDataList.FindAll(x => x.JobNumber == searchJobNumber);

				upIsDtList.Dispose();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
			return issueDataList;
		}
	}
}
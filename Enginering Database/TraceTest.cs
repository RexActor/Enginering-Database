using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Engineering_Database
{
	internal class TraceTest
	{
		private static TraceSource ts = new TraceSource("TraceTest");

		private static void Main(string[] args)
		{
			ts.TraceEvent(TraceEventType.Error, 1, "Error message.");
			ts.Close();
			return;
		}
	}
}
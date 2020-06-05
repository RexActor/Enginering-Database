using System;

namespace Engineering_Database
{
	class StatutoryClass
	{

		public int ID { get; set; }
		public string Manufacturer { get; set; }

		public string EquipmentDescription { get; set; }

		public string CompanyIssuer { get; set; }

		public string SerialNumber { get; set; }

		public string MonthlyWeekly { get; set; }
		public string DaysLeftTillInspection { get; set; }
		public string DateReportIssued { get; set; }

		public string RenewDate { get; set; }


		public DateTime RenewDateForCalculation { get; set; }
	}
}

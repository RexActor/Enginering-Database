using System;

namespace Engineering_Database
{
	class MeterReadingClass
	{
		public string InsertDate { get; set; }
		public double meterReading { get; set; }

		public string ReadingMonth { get; set; }
		public string ReadingYear { get; set; }
		public int DifferenceInDays { get; set; }
	}
}

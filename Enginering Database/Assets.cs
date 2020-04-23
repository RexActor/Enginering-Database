using System;

namespace Engineering_Database
{
	class Assets
	{



		public int ID { get; set; }
		public string Description { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string AssetNumber { get; set; }
		public string SerialNumber { get; set; }
		public string DateofManufacture { get; set; }//inputed only year
		public DateTime DateofInstallation { get; set; }
		public string IssueLevel { get; set; }
		public string InstalledOn { get; set; }
		public bool Decomissioned { get; set; }
		public bool OnSite { get; set; }



	}
}

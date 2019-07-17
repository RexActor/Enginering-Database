using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Database
{
	class LogHelper
	{

		public static log4net.ILog GetLogger([CallerFilePath]String filename= "")
		{
			return log4net.LogManager.GetLogger(filename);
		}
	}
}

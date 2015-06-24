using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
	public class TempFolderCleaner
	{
		public static void Start()
		{
			while (true)
			{
				// Delete files older then 2 days
				string[] directories = Directory.GetDirectories(Parameters_DataTransfer.TempFolder);

				foreach (string directory in directories)
				{
					DirectoryInfo fi = new DirectoryInfo(directory);
					if (fi.CreationTime < DateTime.Now.AddDays(-2))
						Directory.Delete(directory, true);
				}

				// Wait for 1/2 hour
				Thread.Sleep(1800000);
			}
		}
	}
}

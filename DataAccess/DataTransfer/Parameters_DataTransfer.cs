using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer
{
  public class Parameters_DataTransfer
  {
    public static String LogFolder;
    public static String TempFolder;
		public static String QueueFolder;
    public static String LocalQueuePath;
    public static String OutboxFolder;
		public static String InboxFolder;
		public static String ProcessedFolder;
		public static String FailedFolder;
    public static Int32 MaxQueueRetry;

		public static class SAPLoginSettings
		{
			public static String SAP_AppServerHost;
			public static String SAP_SystemNumber;
			public static String SAP_SystemID;
			public static String SAP_User;
			public static String SAP_Password;
			public static String SAP_Client;
		}
  }
}

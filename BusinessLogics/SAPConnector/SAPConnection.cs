using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.SAPConnector
{
	public class SapConnection : IDestinationConfiguration
	{
		public RfcConfigParameters GetParameters(string destinationName)
		{
			RfcConfigParameters conf = new RfcConfigParameters();
			if (destinationName == "NSP")
			{
				conf.Add(RfcConfigParameters.AppServerHost, Parameters_DataTransfer.SAPLoginSettings.SAP_AppServerHost); // vlsrv25a
				conf.Add(RfcConfigParameters.SystemNumber, Parameters_DataTransfer.SAPLoginSettings.SAP_SystemNumber); // 00
				conf.Add(RfcConfigParameters.SystemID, Parameters_DataTransfer.SAPLoginSettings.SAP_SystemID); // T22
				conf.Add(RfcConfigParameters.User, Parameters_DataTransfer.SAPLoginSettings.SAP_User); // BAPI_BEESD
				conf.Add(RfcConfigParameters.Password, Parameters_DataTransfer.SAPLoginSettings.SAP_Password); // rfc%c&ll
				conf.Add(RfcConfigParameters.Client, Parameters_DataTransfer.SAPLoginSettings.SAP_Client); // 400
			}
			return conf;
		}

		public bool ChangeEventsSupported()
		{
			return true;
		}

		public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;
	}
}

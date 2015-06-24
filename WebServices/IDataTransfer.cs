using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VanLeeuwen.Projects.WebPortal.WebServices
{
  // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDataTransferService" in both code and config file together.
  [ServiceContract]
  public interface IDataTransfer
  {
    [OperationContract]
    Boolean SendFile(String databaseName, byte[] file, String fileName, out String errorMessage);

		[OperationContract]
		Boolean ReceiveFile(String databaseName, out String fileName, out byte[] fileContent, out String errorMessage);

		[OperationContract]
		Boolean DeleteFile(String fileName);

    [OperationContract]
    String Test();
  }
}

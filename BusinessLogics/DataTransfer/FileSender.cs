using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class FileSender
  {
    public static Boolean SendFile(String databaseName, byte[] file, String fileName, out String errorMessage)
    {
      Boolean returnValue = true;
      errorMessage = String.Empty;

      try
      {
        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
        DataTransferService.DataTransferClient xmlServiceClient = new DataTransferService.DataTransferClient();

				//WebRequest.DefaultWebProxy = new WebProxy("bluecoatzwd:8080")
				//{
				//	Credentials = new NetworkCredential(@"VanLeeuwen\b1zwd", "sbo500!"),
				//};
        
        returnValue = xmlServiceClient.SendFile(databaseName, file, fileName, out errorMessage);
      }
      catch (Exception ex)
      {
				Trace.WriteLine(String.Format("Error while sending file {0}, database: {1}, errormessage: {2}", fileName, databaseName, ex.Message), "SendFile");

        errorMessage = ex.Message;
        return false;
      }

      if (!String.IsNullOrEmpty(errorMessage) || returnValue == false)
      {
				Trace.WriteLine(String.Format("Error while sending file {0}, database: {1}, errormessage: {2}", fileName, databaseName, errorMessage), "SendFile");
        return false;
      }

			Trace.WriteLine(String.Format("File send succesfully. filename:{0}, database: {1}", fileName, databaseName), "SendFile");
      return returnValue;
    }

    public static bool IgnoreCertificateErrorHandler(object sender,
      X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
      return true;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class FileReceiver
  {
		public static void Start()
		{
			while (true)
			{
				string[] directories = Directory.GetDirectories(Parameters_DataTransfer.InboxFolder);

				foreach (string directory in directories)
				{
					ReceiveFile(directory);
				}

				// Wait for 5 minutes
				Thread.Sleep(10000);//300000
			}
		}
		
		
		public static Boolean ReceiveFile(String directory)
    {
      Boolean returnValue = true;
			byte[] file;
			String fileName = String.Empty;
			String errorMessage;
			String databaseName = new DirectoryInfo(directory).Name;

      try
      {
				DataTransferService.DataTransferClient xmlServiceClient = new DataTransferService.DataTransferClient();
        returnValue = xmlServiceClient.ReceiveFile(databaseName, out fileName, out file, out errorMessage);

				if (!String.IsNullOrEmpty(errorMessage) || returnValue == false)
				{
					Trace.WriteLine(String.Format("Error while receiving file {0}, database: {1}, errormessage: {2}", fileName, databaseName, errorMessage), "ReceiveFile");
					return false;
				}

				if (String.IsNullOrEmpty(fileName))
					return true;

				// InboxFilePath
				String inboxFilePath = Path.Combine(DataAccess.DataTransfer.Parameters_DataTransfer.InboxFolder, databaseName, Path.GetFileName(fileName));

				// Save in inbox
				BinaryWriter binWriter = new BinaryWriter(File.Open(inboxFilePath, FileMode.Create, FileAccess.ReadWrite));
				binWriter.Write(file);
				binWriter.Close();

				// Delete File from Webserver if transfer succesfull
				returnValue = xmlServiceClient.DeleteFile(fileName);

				// Call ReceiveFile to get more files if available
				ReceiveFile(databaseName);
      }
      catch (Exception ex)
      {
				Trace.WriteLine(String.Format("Error while receiving file {0}, database: {1}, errormessage: {2}", fileName, databaseName, ex.Message), "ReceiveFile");

        errorMessage = ex.Message;
        return false;
      }

			Trace.WriteLine(String.Format("File received succesfully. filename:{0}, database: {1}", fileName, databaseName), "ReceiveFile");
      return returnValue;
    }
  }
}

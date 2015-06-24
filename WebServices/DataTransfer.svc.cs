using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using VanLeeuwen.Framework.Logging;
using VanLeeuwen.Projects.WebPortal.DataAccess.CachedObjects;


namespace VanLeeuwen.Projects.WebPortal.WebServices
{
  // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataTransferService" in code, svc and config file together.
  // NOTE: In order to launch WCF Test Client for testing this service, please select DataTransferService.svc or DataTransferService.svc.cs at the Solution Explorer and start debugging.
  public class DataTransferService : IDataTransfer
  {
    private Logger GetLogger()
    {
      return new Logger(ServiceSettings.LogPath, "XMLFileService", Logger.LogFileSettings.LogFilePerDay, Logger.LogFolderSettings.LogFolderPerYear | Logger.LogFolderSettings.LogFolderPerMonth);
    }

		public Boolean ReceiveFile(String databaseName, out String fileName, out byte[] fileContent, out String errorMessage)
		{
			//CreateXml();

			
			Logger objLogger = this.GetLogger();
			errorMessage = String.Empty;
			fileName = String.Empty;
			fileContent = new byte[0];

			String outboxRoot = ServiceSettings.XMLOutboxRoot;
			String outboxFolder = Path.Combine(outboxRoot, databaseName);
			
			try
			{
				// Get oldest file in folder
				DirectoryInfo info = new DirectoryInfo(outboxFolder);
				FileInfo file = info.GetFiles().OrderBy(p => p.CreationTime).FirstOrDefault();

				if (file == null)
					return true;

				objLogger.WriteLine("GetFile called");
				objLogger.WriteLine(String.Format("FileName: {0}", fileName));
				objLogger.WriteLine(String.Format("|Sending Database: {0}", databaseName));

				VanLeeuwen.Framework.IO.File.WaitForRelease(fileName, 10);

				fileName = file.FullName;
				fileContent = File.ReadAllBytes(fileName);
			}
			catch (Exception ex)
			{
				errorMessage = String.Format("The file cannot be downloaded: {0}, {1}. Error: {2}", fileName, System.Environment.MachineName, ex.Message);
				objLogger.WriteLine(errorMessage);
				return false;
			}
			return true;
		}

		private void CreateXml()
		{
			DocumentCached doc = new DocumentCached();
			doc.CustomerCode = "ABCDW";
			doc.DocDate = DateTime.Now;
			doc.Comment = "testing comment";
			doc.VLCompany = "VLB";

			DocumentLineCached line = new DocumentLineCached();
			line.Currency = "EUR";
			line.ItemCode = "A100";
			line.LineNum = 2;
			line.Price = 23;
			line.Quantity = 200;
			line.ShortText = "Testline";
			line.UnitOfMeasure = "EU";

			doc.Lines.Add(line);

			DocumentLineCached line2 = new DocumentLineCached();
			line2.Currency = "EUR";
			line2.ItemCode = "B12300";
			line2.LineNum = 3;
			line2.Price = 57;
			line2.Quantity = 450;
			line2.ShortText = "Testline2";
			line2.UnitOfMeasure = "EU";

			doc.Lines.Add(line2);

			VanLeeuwen.Framework.Xml.XmlToObject.ObjectToXml(doc, @"D:\Test\DataProcessor_Outbox\SBO_SAPECC6_PROD\testxml_" + DateTime.Now.ToShortDateString() + ".xml");
		}

		public Boolean DeleteFile(String fileName)
		{
			try
			{
				File.Delete(fileName);
			}
			catch (Exception ex)
			{
				String errorMessage = String.Format("The file cannot be deleted: {0}, {1}. Error: {2}", fileName, System.Environment.MachineName, ex.Message);
				Logger objLogger = this.GetLogger();
				objLogger.WriteLine(errorMessage);
				return false;
			}
			return true;
		}

    public Boolean SendFile(String databaseName, byte[] file, String fileName, out String errorMessage)
    {
      Logger objLogger = this.GetLogger();

      errorMessage = String.Empty;

      String inboxRoot = ServiceSettings.XMLInboxRoot;
      String inboxFilePath = Path.Combine(inboxRoot, databaseName, fileName);

      try
      {
        objLogger.WriteLine("SendXMLFile called");
        objLogger.WriteLine(String.Format("FileName: {0}", fileName));
        objLogger.WriteLine(String.Format("Database to update: {0}", databaseName));


        BinaryWriter binWriter = new BinaryWriter(File.Open(inboxFilePath, FileMode.Create, FileAccess.ReadWrite));
        binWriter.Write(file);
        binWriter.Close();

        //System.IO.File.WriteAllBytes(inboxFilePath, file);
      }
      catch (Exception ex)
      {
        errorMessage = String.Format("The file cannot be written to the disk: {0}, {1}. Error: {2}", inboxFilePath, System.Environment.MachineName, ex.Message);
        objLogger.WriteLine(errorMessage);
        return false;
      }

      try
      {
        FileInfo fileInfo = new FileInfo(inboxFilePath);
        List<String> acceptedFileExtensions = new List<String>() 
        {
          ".xml", ".pdf"
        };

        if (!acceptedFileExtensions.Contains(fileInfo.Extension))
          throw new Exception(String.Format("Unable to accept file extension {0}!", fileInfo.Extension));

        switch (fileInfo.Extension)
        {
          case ".xml":

            //Try to read the xml message
            StreamReader streamReader = new StreamReader(inboxFilePath);
            string xmlString = streamReader.ReadToEnd();
            streamReader.Close();

            XDocument xmldoc = XDocument.Parse(xmlString);
            break;
        }
      }
      catch (Exception ex)
      {
        File.Delete(inboxFilePath);

        errorMessage = String.Format("The file is not correct: {0}, {1}. Error: {2}", inboxFilePath, System.Environment.MachineName, ex.Message);
        objLogger.WriteLine(errorMessage);
        return false;
      }

      objLogger.WriteLine(String.Format("File {0} succesfully received.", fileName));
      objLogger.WriteLine(String.Empty);
      return true;
    }

    public String Test()
    {
      return "Pong";
    }
  }
}

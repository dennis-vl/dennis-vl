using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class FileProcessor
  {
    //private Companies companies;
    private String localQueuePath;
    //private String queueFailedFolder;
    private Int32 maxQueueRetry;

    public FileProcessor(/*Companies companies,*/ String localQueuePath, Int32 maxQueueRetry/*, String queueFailedFolder*/)
    {
      //this.companies = companies;
      this.localQueuePath = localQueuePath;
      this.maxQueueRetry = maxQueueRetry;
    }

    /// <summary>
    /// Process xml file picked up by the FileProcessor Service
    /// </summary>
    /// <param name="databaseName"></param>
    /// <param name="xmlFile"></param>
    /// <returns></returns>
    public Boolean ProcessXmlFile(String databaseName, XDocument xmlFile, String filePath)
    {
      //Trace.WriteLine("", "ProcessFile");
      Boolean returnValue = true;

      try
      {
        String xmlFileName = Path.GetFileName(filePath);// String.Format("{0}.xml", Guid.NewGuid().ToString());

        // Send Delivery before Certificates
        MessageQueue messageQueue = new MessageQueue(this.localQueuePath, this.maxQueueRetry);
        messageQueue.AddMessage(databaseName, filePath, xmlFileName);

        //Delivery
				String xmlMessage = xmlFile.ToString();
        if (xmlMessage.Contains("DELVRY03"))
          CheckForCertificates(xmlMessage, filePath);

        messageQueue.ReceiveLastMessage();
      }
      catch (Exception ex)
      {
        //Trace.WriteLine("", "ProcessFile");
				throw new Exception(ex.Message);
      }

      return returnValue;
    }

		/// <summary>
		/// Process xml file picked up by the FileProcessor Service
		/// </summary>
		/// <param name="databaseName"></param>
		/// <param name="xmlFile"></param>
		/// <returns></returns>
		public Boolean ProcessPortalInvoiceXml(String databaseName, XDocument xmlFile, String filePath)
		{
			//Trace.WriteLine("", "ProcessFile");
			Boolean returnValue = true;

			try
			{
				String xmlFileName = Path.GetFileName(filePath);// String.Format("{0}.xml", Guid.NewGuid().ToString());

				SAPConnector.SAPProcessor sapProcessor = new SAPConnector.SAPProcessor();
				returnValue = sapProcessor.ProcessInvoice(databaseName, xmlFile.ToString());
			}
			catch (Exception ex)
			{
				//Trace.WriteLine("", "ProcessFile");
				throw new Exception(ex.Message);
			}

			return returnValue;
		}

    /// <summary>
    /// Process xml file picked up by the FileProcessor Service
    /// </summary>
    /// <param name="databaseName"></param>
    /// <param name="xmlFile"></param>
    /// <returns></returns>
    public Boolean ProcessPDFFile(String databaseName, String filePath)
    {
      //Trace.WriteLine("", "ProcessFile");
      Boolean returnValue = true;

      //try
      //{
        String fileName = Path.GetFileName(filePath);

        MessageQueue messageQueue = new MessageQueue(this.localQueuePath, this.maxQueueRetry);
				if (!messageQueue.AddMessage(databaseName, filePath, fileName))
					throw new Exception("Unable to Add Message to Queue.");

        messageQueue.ReceiveLastMessage();
      //}
      //catch (Exception ex)
      //{
        //Trace.WriteLine("", "ProcessFile");
      //  return false;
      //}

      return returnValue;
    }


    private void CheckForCertificates(string xmlMessage, String filePath)
    {
      List<String> certificates = new List<String>();

      XDocument xmldoc = XDocument.Parse(xmlMessage);
      XElement deliveryElement = xmldoc.Element("DELVRY03").Element("IDOC").Element("E1EDL20");

      foreach (var deliveryLineElement in deliveryElement.Elements("E1EDL24").ToList())
      {
        XElement certificateElement = deliveryLineElement.Element("MFRPN");
        if(certificateElement != null)
          certificates.Add(certificateElement.Value);
      }

      if (certificates.Count == 0)
        return;

      CertificateSender certSender = new CertificateSender("http://vlsrv09a:8080", "0045", "P1");
      certSender.SendCertificates(Path.GetDirectoryName(filePath), certificates);
    }
  }
}

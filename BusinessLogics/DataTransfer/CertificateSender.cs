using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class CertificateSender
  {
    public String IXOSurl { get; set; }
    public String IXOSVersion { get; set; }
    public String ArchiveID { get; set; }
    List<Certificate> Certificates { get; set; }

    public CertificateSender(String ixosUrl, String ixosVersion, String archiveID)
    {
      this.Certificates = new List<Certificate>();

      this.IXOSurl = ixosUrl;
      this.IXOSVersion = ixosVersion;
      this.ArchiveID = archiveID;
    }

    public void SendCertificates(String outboxPath, List<String> certificateDocIds)
    {
      GetCertificatesInfo(certificateDocIds);
      GetCertificates(outboxPath);

      PDFCreator pc = new PDFCreator(Certificates);
      pc.OutboxPath = outboxPath;
      pc.CreatePdf();
    }

    private void GetCertificates(String outboxPath)
    {
      foreach (Certificate certificate in this.Certificates)
      {
        certificate.TempPath = Path.Combine(Parameters_DataTransfer.TempFolder, Guid.NewGuid().ToString());
        Directory.CreateDirectory(certificate.TempPath);

        foreach (Certificate.CertificatePage page in certificate.CertificatePages)
        {
          try
          {
            page.TempFilePath = Path.Combine(certificate.TempPath, Guid.NewGuid().ToString() + ".pdf");

            String uriString = String.Format("{0}/archive?get&pVersion={1}&contRep={2}&docId={3}&compId={4}", this.IXOSurl, this.IXOSVersion, this.ArchiveID, certificate.DocId, page.CompId);
            Trace.WriteLine(uriString, "GetCertificates");

            String errorMessage = DownloadFile(uriString, page.TempFilePath);
            if (!String.IsNullOrEmpty(errorMessage))
              throw new Exception(errorMessage);
          }
          catch (Exception ex)
          {
            String message = String.Format("Error while retrieving document from IXOS Server. DocID: {0}", certificate.DocId);

            Trace.WriteLine(message, "GetCertificates");
            Trace.WriteLine(ex.Message, "GetCertificates");
          }
          finally
          {
            //Thread.Sleep(10);
          }
        }
      }
    }

    public Boolean GetCertificatesInfo(List<String> certificateDocIds)
    {
      foreach (var docId in certificateDocIds)
      {
        Certificate certificate = new Certificate();
        certificate.CertificatePages = new List<Certificate.CertificatePage>();
        certificate.DocId = docId;

        Stream myStream;
        String uriString = String.Empty;

        try
        {
          // Get certificate info file
          WebClient myWebClient = new WebClient();
          uriString = String.Format("{0}/archive?info&pVersion={1}&contRep={2}&docId={3}", this.IXOSurl, this.IXOSVersion, this.ArchiveID, docId);

          Trace.WriteLine(uriString, "GetCertificatesInfo");

          myStream = myWebClient.OpenRead(uriString);
        }
        catch (Exception ex)
        {
          String message = String.Format("Error while retrieving documentinfo from IXOS Server. DocID: {0}", docId);

          Trace.WriteLine(message, "GetCertificatesInfo");
          Trace.WriteLine(ex.Message, "GetCertificatesInfo");

          continue;
        }

        Certificate.CertificatePage page = new Certificate.CertificatePage();

        try
        {
          // Open a stream to point to the data stream coming from the Web resource.
          using (StreamReader sr = new StreamReader(myStream))
          {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
              if (line.StartsWith("--"))
              {
                if (!String.IsNullOrEmpty(page.CompId))
                  if (page.CompId.StartsWith("data"))
                    certificate.CertificatePages.Add(page);

                page = new Certificate.CertificatePage();
              }

              String key = line.Split(':')[0];
              String value = (line.Split(':').Count() > 1) ? line.Split(':')[1].Trim() : String.Empty;

              switch (key)
              {

                case "Content-Type":
                  page.ContentType = value;
                  break;

                case "X-compId":
                  page.CompId = value;
                  break;
              }
            }
          }
          this.Certificates.Add(certificate);
        }
        catch (Exception ex)
        {
          String message = String.Format("Error while reading documentinfo from IXOS Server. DocID: {0}", docId);

          Trace.WriteLine(message, "GetCertificatesInfo");
          Trace.WriteLine(ex.Message, "GetCertificatesInfo");

          continue;
        }

        // Close the stream. 
        myStream.Close();
      }
      return true;
    }

    public static String DownloadFile(String remoteFilename, String localFilename)
    {
      String returnErrorMessage = String.Empty;

      // Function will return the number of bytes processed  
      // to the caller. Initialize to 0 here.10.  
      int bytesProcessed = 0;
      // Assign values to these objects here so that they can  
      // be referenced in the finally block 
      Stream remoteStream = null;
      Stream localStream = null;
      WebResponse response = null;

      // Use a try/catch/finally block as both the WebRequest and Stream  // classes throw exceptions upon error20.  

      try
      {
        // Create a request for the specified remote file name    
        WebRequest request = WebRequest.Create(remoteFilename);

        if (request != null)
        {
          // Send the request to the server and retrieve the      
          // WebResponse object       
          response = request.GetResponse();

          if (response != null)
          {
            // Once the WebResponse object has been retrieved,        
            // get the stream object associated with the response's data        
            remoteStream = response.GetResponseStream();

            // Create the local file        
            localStream = File.Create(localFilename);

            // Allocate a 1k buffer        
            byte[] buffer = new byte[1024];
            int bytesRead;

            // Simple do/while loop to read from stream until        
            // no bytes are returned        
            do
            {
              // Read data (up to 1k) from the stream          
              bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

              // Write the data to the local file          
              localStream.Write(buffer, 0, bytesRead);

              // Increment total bytes processed          
              bytesProcessed += bytesRead;
            } while (bytesRead > 0);
          }
        }
      }
      catch (Exception e)
      {
        returnErrorMessage = e.Message;
      }
      finally
      {
        // Close the response and streams objects here    
        // to make sure they're closed even if an exception    
        // is thrown at some point    

        if (response != null)
          response.Close();

        if (remoteStream != null)
          remoteStream.Close();

        if (localStream != null)
          localStream.Close();
      }
      // Return total bytes processed to caller.  
      return returnErrorMessage;
    }
  }
}

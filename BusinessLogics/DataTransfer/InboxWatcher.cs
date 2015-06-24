using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class InboxWatcher
  {
    FileSystemWatcher watcher;
    FileProcessor fileProcessor;

    public InboxWatcher()
    {
      this.fileProcessor = new FileProcessor(Parameters_DataTransfer.LocalQueuePath, Parameters_DataTransfer.MaxQueueRetry);

      this.CheckDirectory();

      this.watcher = new FileSystemWatcher();
      this.watcher.Path = Parameters_DataTransfer.InboxFolder;
      this.watcher.IncludeSubdirectories = true;
			this.watcher.InternalBufferSize = 64000;

      this.watcher.Created += new FileSystemEventHandler(watcher_Created);
    }

    public void CheckDirectory()
    {
      Trace.WriteLine("Check XML inbox directories for unprocessed files: " + Parameters_DataTransfer.InboxFolder, "CheckDirectory");

      String[] directories = Directory.GetDirectories(Parameters_DataTransfer.InboxFolder);

      foreach (String directory in directories)
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        var xmlFiles = directoryInfo.GetFiles();

        foreach (var file in xmlFiles.OrderBy(c => c.CreationTime))
        {
          ProcessFile(file);
        }
      }

      Trace.WriteLine("Check XML inbox directories complete: " + Parameters_DataTransfer.InboxFolder, "CheckDirectory");
    }

    private void ProcessFile(FileInfo file)
    {
			// Wait for 2 second. Till file is received.
			Thread.Sleep(2000);

      switch (file.Extension)
      {
        case ".xml":
          ProcessXmlFile(file.FullName);
          break;
      }
    }


    private void ProcessXmlFile(String filePath)
    {
      Boolean returnValue = true;

      try
      {
				Trace.WriteLine("File: " + filePath, "ProcessXmlFile");

        // Wait for file release
        VanLeeuwen.Framework.IO.File.WaitForRelease(filePath, 10);

        var fileContents = System.IO.File.ReadAllText(filePath);

				if (fileContents == null)
					throw new Exception(String.Format("Cannot read empty file: '{0}'", filePath));
				
				// remove special Characters from xml
        fileContents = fileContents.Replace("�", "");
        System.IO.File.WriteAllText(filePath, fileContents);

        // Validate the XML format
        XDocument xmlFile = XDocument.Load(filePath);

        String directory = Path.GetDirectoryName(filePath);
        String companyName = directory.Substring(directory.LastIndexOf('\\') + 1);

        returnValue = fileProcessor.ProcessPortalInvoiceXml(companyName, xmlFile, filePath);
      }
      catch (Exception ex)
      {
				Trace.WriteLine("Error while processing xml file: " + ex.Message, "ProcessXmlFile");

        // Do nothing when a connection error occurs
				// Invoice will be process on the next day when service is restarted.
				if (ex.Message.Contains("LOCATION") && ex.Message.Contains("ERROR") && ex.Message.Contains("TIME"))
					return;
			
        returnValue = false;
      }

			try
			{
				if (!returnValue)
				{
					String xmlFailedPath = filePath.Replace(Parameters_DataTransfer.InboxFolder, Parameters_DataTransfer.FailedFolder);
					VanLeeuwen.Framework.IO.File.MoveAndOverwrite(filePath, xmlFailedPath);
				}

				if (returnValue)
				{
					String xmlProcessedPath = filePath.Replace(Parameters_DataTransfer.InboxFolder, Parameters_DataTransfer.ProcessedFolder);
					VanLeeuwen.Framework.IO.File.MoveAndOverwrite(filePath, xmlProcessedPath);
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Error while moving the file to Processed folder: " + ex.Message, "ProcessXmlFile");
				returnValue = false;
			}

			if (returnValue)
				Trace.WriteLine(String.Format("File: {0} processed succesfully!", filePath), "ProcessXmlFile");
    }

    void watcher_Created(object sender, FileSystemEventArgs e)
    {
      ProcessFile(new FileInfo(e.FullPath));
    }

    public void Start()
    {
      // Begin watching.        
      this.watcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
      // Stop watching.        
      this.watcher.EnableRaisingEvents = false;
    }
  }
}


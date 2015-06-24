using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class OutboxWatcher
  {
    FileSystemWatcher watcher;
    FileProcessor fileProcessor;

    public OutboxWatcher()
    {
      this.fileProcessor = new FileProcessor(Parameters_DataTransfer.LocalQueuePath, Parameters_DataTransfer.MaxQueueRetry);

      this.CheckDirectory();

      this.watcher = new FileSystemWatcher();
      this.watcher.Path = Parameters_DataTransfer.OutboxFolder;
      this.watcher.IncludeSubdirectories = true;
			this.watcher.InternalBufferSize = 64000;

      this.watcher.Created += new FileSystemEventHandler(watcher_Created);
    }

    public void CheckDirectory()
    {
      Trace.WriteLine("Check XML outbox directories for unprocessed files: " + Parameters_DataTransfer.OutboxFolder, "CheckDirectory");

      String[] directories = Directory.GetDirectories(Parameters_DataTransfer.OutboxFolder);

      foreach (String directory in directories.Where(c => !c.Contains("SCHEDULED")))
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        var xmlFiles = directoryInfo.GetFiles();

        foreach (var file in xmlFiles.OrderBy(c => c.CreationTime))
        {
          ProcessFile(file);
        }
      }

      Trace.WriteLine("Check XML outbox directories complete: " + Parameters_DataTransfer.OutboxFolder, "CheckDirectory");
    }

    private void ProcessFile(FileInfo file)
    {
      switch (file.Extension)
      {
        case ".xml":
          ProcessXmlFile(file.FullName);
          break;
        case ".pdf":
          ProcessPDFFile(file.FullName);
          break;
      }
    }


    private void ProcessXmlFile(String filePath)
    {
      Boolean returnValue = true;

      try
      {
        // Wait for file release
        VanLeeuwen.Framework.IO.File.WaitForRelease(filePath, 10);

        // remove special Characters from xml
        var fileContents = System.IO.File.ReadAllText(filePath);
        fileContents = fileContents.Replace("�", "");
        System.IO.File.WriteAllText(filePath, fileContents);

        // Validate the XML format
        XDocument xmlFile = XDocument.Load(filePath);

        String directory = Path.GetDirectoryName(filePath);
        String companyName = directory.Substring(directory.LastIndexOf('\\') + 1);

        returnValue = fileProcessor.ProcessXmlFile(companyName, xmlFile, filePath);
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Error while reading xml file: " + ex.Message, "ProcessXmlFile");
        returnValue = false;
      }

			//if (!returnValue)
			//{
			//	//String xmlFailedPath = filePath.Replace(this.xmlOutboxRoot, this.xmlFailedRoot);
			//	//File.Move(filePath, xmlFailedPath);
			//}
			//else
			//{
			//	File.Delete(filePath);
			//}
    }

    private void ProcessPDFFile(String filePath)
    {
      Boolean returnValue = true;

      try
      {
        // Wait for file release
        VanLeeuwen.Framework.IO.File.WaitForRelease(filePath, 10);


        byte[] fileContents = File.ReadAllBytes(filePath);

        String directory = Path.GetDirectoryName(filePath);
        String companyName = directory.Substring(directory.LastIndexOf('\\') + 1);

        returnValue = fileProcessor.ProcessPDFFile(companyName, filePath);
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Error while reading xml file: " + ex.Message, "ProcessXmlFile");
        returnValue = false;
      }

      if (!returnValue)
      {
        //String xmlFailedPath = filePath.Replace(this.xmlOutboxRoot, this.xmlFailedRoot);
        //File.Move(filePath, xmlFailedPath);
      }
      else
      {
        File.Delete(filePath);
      }
    }

    void watcher_Created(object sender, FileSystemEventArgs e)
    {
      if(!e.FullPath.Contains("SCHEDULED"))
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


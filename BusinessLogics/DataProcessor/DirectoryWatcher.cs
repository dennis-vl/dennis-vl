using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor
{
  public class DirectoryWatcher
  {
    FileSystemWatcher xmlWatcher;
    XMLDataProcessor dataProcessor;
    PDFProcessor pdfProcessor;

    public DirectoryWatcher()
    {
      this.dataProcessor = new XMLDataProcessor();
      this.pdfProcessor = new PDFProcessor();
      
      this.CheckDirectory();

      this.xmlWatcher = new FileSystemWatcher();
      this.xmlWatcher.Path = Parameters_DataProcessor.XmlInboxRoot;
      this.xmlWatcher.IncludeSubdirectories = true;
			this.xmlWatcher.InternalBufferSize = 64000;

      this.xmlWatcher.Created += new FileSystemEventHandler(watcher_Created);
    }

    public void CheckDirectory()
    {
      Trace.WriteLine("Check inbox directories for unprocessed files: " + Parameters_DataProcessor.XmlInboxRoot, "CheckDirectory");

      String[] directories = Directory.GetDirectories(Parameters_DataProcessor.XmlInboxRoot);

      foreach (String directory in directories)
      {
        DirectoryInfo dir = new DirectoryInfo(directory);
        List<FileInfo> files = dir.GetFiles().Where(c => !c.Extension.Equals(String.Empty)).ToList();

        foreach (FileInfo file in files.OrderBy(f => f.LastWriteTime))
        {
          ProcessFile(file.FullName);
        }
      }

      Trace.WriteLine("Check inbox directories complete: " + Parameters_DataProcessor.XmlInboxRoot, "CheckDirectory");
    }

		private void ProcessFile(string filePath)
		{
			if (!filePath.EndsWith(".xml") && !filePath.EndsWith(".pdf"))
				return;

			Boolean returnValue = true;

			String extension = Path.GetExtension(filePath);
			String directory = Path.GetDirectoryName(filePath);
			String databaseName = directory.Substring(directory.LastIndexOf('\\') + 1);

			try
			{
				switch (extension)
				{
					case ".xml":

						Trace.WriteLine("File: " + filePath, "ProcessFile");
						String fileContent = VanLeeuwen.Framework.IO.File.GetFileContent(filePath);

						if (fileContent == null)
							throw new Exception(String.Format("Cannot read empty file: '{0}'", filePath));

						if (!dataProcessor.ProcessDataFromXML(databaseName, fileContent))
							returnValue = false;

						break;

					case ".pdf":

						Trace.WriteLine("File: " + filePath, "ProcessFile");
						if (!pdfProcessor.ProcessPDF(databaseName, filePath))
							returnValue = false;
						break;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Error while reading file: " + ex.Message, "ProcessFile");
				returnValue = false;
			}

			try
			{
				if (!returnValue)
				{
					String xmlFailedPath = filePath.Replace(Parameters_DataProcessor.XmlInboxRoot, Parameters_DataProcessor.XmlFailedRoot);
					VanLeeuwen.Framework.IO.File.MoveAndOverwrite(filePath, xmlFailedPath);
				}

				if (returnValue && extension.Equals(".xml"))
				{
					String xmlProcessedPath = filePath.Replace(Parameters_DataProcessor.XmlInboxRoot, Parameters_DataProcessor.XMLProcessedRoot);
					VanLeeuwen.Framework.IO.File.MoveAndOverwrite(filePath, xmlProcessedPath);
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Error while moving the file to Processed folder: " + ex.Message, "ProcessFile");
				returnValue = false;
			}

			if (returnValue)
				Trace.WriteLine(String.Format("File: {0} processed succesfully!", filePath), "ProcessFile");
		}

    void watcher_Created(object sender, FileSystemEventArgs e)
    {
      ProcessFile(e.FullPath);
    }

    public void Start()
    {
      // Begin watching.        
      this.xmlWatcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
      // Stop watching.        
      this.xmlWatcher.EnableRaisingEvents = false;
    }
  }
}

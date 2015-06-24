using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor
{
  public class PDFProcessor
  {
    public String CompanyFilesRoot { get; set; }

    public PDFProcessor()
    {
    }

    public Boolean ProcessPDF(String databaseName, String filePath)
    {
      Boolean returnValue = true;

      try
      {

        String fileName = Path.GetFileName(filePath);
        String fileType = fileName.Substring(0, fileName.IndexOf("_"));

        switch (fileType)
        {
          case "CERT":

						DALPortalDataContext dc = new DALPortalDataContext();

						String newPath = Path.Combine(Parameters_DataProcessor.CompanyFilesRoot, "Certificates", fileName);
            String dbPath = Path.Combine(@"~\Files\Certificates", fileName);
						String docId = fileName.Replace(fileType + "_", "").Replace(".pdf", "");

						var batchesToUpdate = dc.batches.Where(c => c.ixosArchiveId.Equals(docId)).ToList();
						if (batchesToUpdate.Count == 0)
							throw new Exception(String.Format("Unable to find Batch with certificateIndexNumber {0}", docId));

            VanLeeuwen.Framework.IO.File.WaitForRelease(filePath, 10);
            VanLeeuwen.Framework.IO.File.MoveAndOverwrite(filePath, newPath);

            // Add Path to Batches
            batchesToUpdate.ForEach(c => c.certificateLink = dbPath);
            dc.SubmitChanges();

            break;
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(String.Format("Process PDF file Failed. Error: {0}", ex.Message), "ProcessDataFromXML");
        returnValue = false;
      }

      return returnValue;
    }
  }
}

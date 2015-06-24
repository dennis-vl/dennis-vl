using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class PDFCreator
  {
    public List<Certificate> Certificates { get; set; }
    public String TempPath { get; set; }
    public String OutboxPath { get; set; }

    public PDFCreator(List<Certificate> certificates)
    {
      this.Certificates = certificates;
    }

    /// <summary>
    /// Renders a single pdf
    /// </summary>
    public void CreatePdf()
    {
      foreach (Certificate certificate in this.Certificates)
      {
        PdfDocument pdfDocument = new PdfDocument();
        pdfDocument = CreateNewPDFDocument();

        try
        {
          AddCertificates(certificate, ref pdfDocument);
          pdfDocument.Save(Path.Combine(OutboxPath, "CERT_" + certificate.DocId + ".pdf"));
        }
        catch (Exception ex)
        {
          String errorMessage = String.Format("Error while creating new pdf document: {0}", ex.Message);
          Trace.WriteLine(errorMessage, "CreatePdf");
        }
      }
    }

    private PdfDocument CreateNewPDFDocument()
    {
      PdfDocument pdfDocument = new PdfDocument();
      pdfDocument.Info.Title = "Certificate Document";
      pdfDocument.Info.Author = "Van Leeuwen";
      pdfDocument.Info.Subject = "Certificates";

      return pdfDocument;
    }


    public Boolean AddCertificates(Certificate certificate,ref PdfDocument pdfDocument)
    {
      try
      {
        //Add certificate pages from separate pdf files on other pages
        foreach (Certificate.CertificatePage cert in certificate.CertificatePages)
        {
          PdfDocument origDocument = PdfReader.Open(cert.TempFilePath, PdfDocumentOpenMode.Import);

          //Add certificate pages from current pdf
          foreach (PdfPage page in origDocument.Pages)
          {
            pdfDocument.AddPage(page);
          }
        }

        return true;
      }
      catch (Exception ex)
      {
        String errorMessage = "Error while adding certificates to new document.";
        return false;
      }
    }
  }
}

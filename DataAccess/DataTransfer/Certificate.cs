using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer
{
  public class Certificate
  {
    public String DocId { get; set; }
    public String TempPath { get; set; }
    public String FileName { get; set; }
    public List<CertificatePage> CertificatePages { get; set; }

    public class CertificatePage
    {
      public String CompId { get; set; }
      public String TempFilePath { get; set; }
      public String ContentType { get; set; }
      public String UriString { get; set; }
    }
  }
}

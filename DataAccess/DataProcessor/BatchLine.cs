using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor
{
  public class BatchLine
  {
    public Int32 DocEntry { get; set; }
    public Int32 LineNum { get; set; }
    public Decimal Quantity { get; set; }
    
    public String BatchNumber { get; set; }
    public String HeatNumber { get; set; }
    public String CertificateDocID { get; set; }
    public String CertificateLink { get; set; }
  }
}

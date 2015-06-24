using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer
{
  public class CompanySettings
  {
    private String databaseName;
    private String xmlOutboxPath;

    public CompanySettings()
    { }

    public CompanySettings(String databaseName, String xmlOutboxPath)
    {
      this.databaseName = databaseName;
      this.xmlOutboxPath = xmlOutboxPath;
    }

    public String DatabaseName
    {
      set { this.databaseName = value; }
      get { return this.databaseName; }
    }

    public String XMLOutboxPath
    {
      set { this.xmlOutboxPath = value; }
      get { return this.xmlOutboxPath; }
    }
  }
}

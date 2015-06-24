using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer
{
  public class QueueMessage
  {
    private String databaseName;
    private String filePath;
    private String fileName;
    private Int32 numberOfTries;

    public QueueMessage()
    { }

    public QueueMessage(String databaseName, String filePath, String fileName, Int32 numberOfTries)
    {
      this.databaseName = databaseName;
      this.filePath = filePath;
      this.fileName = fileName;
      this.numberOfTries = numberOfTries;
    }

    public String DatabaseName
    {
      get { return this.databaseName; }
      set { this.databaseName = value; }
    }

    public String FilePath
    {
      get { return this.filePath; }
      set { this.filePath = value; }
    }

    public String FileName
    {
      get { return this.fileName; }
      set { this.fileName = value; }
    }

    public Int32 NumberOfTries
    {
      get { return this.numberOfTries; }
      set { this.numberOfTries = value; }
    }
  }
}

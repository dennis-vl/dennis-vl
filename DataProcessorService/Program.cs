using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VanLeeuwen.Projects.WebPortal.DataProcessorService
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
      //ServiceBase[] ServicesToRun;
      //ServicesToRun = new ServiceBase[] 
      //      { 
      //          new DataProcessor() 
      //      };
      //ServiceBase.Run(ServicesToRun);



      ServiceBase[] servicesToRun;
      servicesToRun = new ServiceBase[] 
      {
          new DataProcessor()
      };
      if (Environment.UserInteractive)
      {
        Framework.Services.InteractiveRunner.RunInteractive(servicesToRun);
      }
      else
      {
        ServiceBase.Run(servicesToRun);
      }
    }
  }
}

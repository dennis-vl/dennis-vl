using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace VanLeeuwen.Projects.WebPortal.DataTransferService
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
      //          new DataTransfer() 
      //      };
      //ServiceBase.Run(ServicesToRun);


      ServiceBase[] servicesToRun;
      servicesToRun = new ServiceBase[] 
      {
          new DataTransfer()
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

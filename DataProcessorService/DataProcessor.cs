using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VanLeeuwen.Framework.Queuing;
using VanLeeuwen.Framework.Scheduling;
using VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataProcessor;

namespace VanLeeuwen.Projects.WebPortal.DataProcessorService
{
  partial class DataProcessor : ServiceBase
  {
    public DataProcessor()
    {
      InitializeComponent();
    }

    private String companiesFilePath = String.Empty;
    private Int32 maxQueueRetry;
    private DirectoryWatcher directoryWatcher;

    // Delegate used for the tasks
    private delegate void ExecuteTasksDelegate();

    // Instantiates a new Scheduler.
    private Scheduler taskScheduler = new Scheduler();

    protected override void OnStart(string[] args)
    {
      this.ReadSettings();

      Debug.Listeners.Add(
        new VanLeeuwen.Framework.Diagnostics.LogFileTraceListener(
          Parameters_DataProcessor.LogFolder,
          "Van Leeuwen WebPortal DataProcessor Service",
          VanLeeuwen.Framework.Logging.Logger.LogFileSettings.LogFilePerDay,
          VanLeeuwen.Framework.Logging.Logger.LogFolderSettings.LogFolderPerYear | VanLeeuwen.Framework.Logging.Logger.LogFolderSettings.LogFolderPerMonth,
          VanLeeuwen.Framework.Logging.Logger.TimeStamps.DateTime));

      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine("Started Van Leeuwen WebPortal DataProcessor Service...", "OnStart");
      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine(String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()), "OnStart");
      Trace.WriteLine("Logger attached", "OnStart");

      this.LogSettingsRead();

      directoryWatcher = new DirectoryWatcher();
      directoryWatcher.Start();
    }

    protected override void OnStop()
    {
      try
      {
        this.directoryWatcher.Stop();
        this.taskScheduler.Stop();
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message, "OnStop");
      }

      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine("Stopped Van Leeuwen WebPortal DataProcessor Service...", "OnStop");
      Trace.WriteLine("-------------------------------------------------------------------");
    }

    /// <summary>
    /// Opens the Settings.xml file and reads its settings.
    /// </summary>
    private void ReadSettings()
    {
      Trace.WriteLine("ReadSettings called...", "ReadSettings");

      try
      {
        companiesFilePath = Path.Combine(Application.StartupPath, "Companies.xml");

        Parameters_DataProcessor.LogFolder = DataProcessorService.Properties.Settings.Default["Logging"].ToString(); //objSettings.ReadString("Folders", "Logging", String.Empty);
        Parameters_DataProcessor.XmlInboxRoot = DataProcessorService.Properties.Settings.Default["XMLInboxRoot"].ToString();
        Parameters_DataProcessor.XmlFailedRoot = DataProcessorService.Properties.Settings.Default["XMLFailedRoot"].ToString();
        Parameters_DataProcessor.CompanyFilesRoot = DataProcessorService.Properties.Settings.Default["CompanyFilesRoot"].ToString();
        Parameters_DataProcessor.TempFolder = DataProcessorService.Properties.Settings.Default["TempFolder"].ToString();
        Parameters_DataProcessor.SMTPServer = DataProcessorService.Properties.Settings.Default["SMTPServer"].ToString();
        Parameters_DataProcessor.XMLProcessedRoot = DataProcessorService.Properties.Settings.Default["XMLProcessedRoot"].ToString();
				Parameters_DataProcessor.EmailEnabled = DataProcessorService.Properties.Settings.Default["EmailEnabled"].ToString() == "Y" ? true : false;
      }
      catch (Exception ex)
      {
        Trace.WriteLine(String.Format("ReadSettings Failed. Error: {0}" + ex.Message), "ReadSettings");
        this.Stop();
      }
    }

    /// <summary>
    /// Log the settings as they are read.
    /// </summary>
    private void LogSettingsRead()
    {
      Trace.WriteLine("LogSettingsRead called...", "LogSettingsRead");
      Trace.WriteLine("Settings.Logging                 = " + Parameters_DataProcessor.LogFolder, "LogSettingsRead");
      Trace.WriteLine("Settings.XMLInboxRoot            = " + Parameters_DataProcessor.XmlInboxRoot, "LogSettingsRead");
      Trace.WriteLine("Settings.XMLFailedRoot           = " + Parameters_DataProcessor.XmlFailedRoot, "LogSettingsRead");
      Trace.WriteLine("Settings.XMLProcessedRoot        = " + Parameters_DataProcessor.TempFolder, "LogSettingsRead");
      Trace.WriteLine("Settings.CompanyFilesRoot        = " + Parameters_DataProcessor.CompanyFilesRoot, "LogSettingsRead");
      Trace.WriteLine("Settings.TempFolder              = " + Parameters_DataProcessor.TempFolder, "LogSettingsRead");
			Trace.WriteLine("Settings.EmailEnabled            = " + Parameters_DataProcessor.EmailEnabled.ToString(), "LogSettingsRead");
    }
  }
}

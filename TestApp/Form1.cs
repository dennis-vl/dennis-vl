using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VanLeeuwen.Framework.Queuing;
using VanLeeuwen.Framework.Scheduling;
using VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace TestApp
{
  public partial class Form1 : Form
  {
    private String logFolder = String.Empty;
    private String watchFolder = String.Empty;
    private String companiesFilePath = String.Empty;
    private String localQueuePath = String.Empty;
    private Int32 maxQueueRetry;
    private XMLSettingsReader xmlSettings;
    private InboxWatcher directoryWatcher;
    private List<CompanySettings> companies;

    // Delegate used for the tasks
    private delegate void ExecuteTasksDelegate();

    // Instantiates a new Scheduler.
    private Scheduler taskScheduler = new Scheduler();


    public Form1()
    {
      InitializeComponent();
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

        this.logFolder = TestApp.Properties.Settings.Default["Logging"].ToString();
        this.watchFolder = TestApp.Properties.Settings.Default["WatchFolder"].ToString();
        this.localQueuePath = TestApp.Properties.Settings.Default["LocalQueuePath"].ToString();
        this.maxQueueRetry = Convert.ToInt32(TestApp.Properties.Settings.Default["MaxQueueRetry"].ToString());

        Trace.WriteLine("Reading Companies...", "ReadSettings");
        this.xmlSettings = new XMLSettingsReader(this.companiesFilePath);
        xmlSettings.ReadXML();

        Trace.WriteLine("Reading Companies complete", "ReadSettings");
      }
      catch (Exception ex)
      {
        Trace.WriteLine(String.Format("ReadSettings Failed. Error: {0}" + ex.Message), "ReadSettings");
        //this.Stop();
      }
    }

    /// <summary>
    /// Log the settings as they are read.
    /// </summary>
    private void LogSettingsRead()
    {
      Trace.WriteLine("LogSettingsRead called...", "LogSettingsRead");
      Trace.WriteLine("Settings.Logging                 = " + this.logFolder, "LogSettingsRead");
      Trace.WriteLine("Settings.WatchFolder             = " + this.watchFolder, "LogSettingsRead");
      Trace.WriteLine("Settings.LocalQueuePath          = " + this.localQueuePath, "LogSettingsRead");
      Trace.WriteLine("Settings.MaxQueueRetry           = " + this.maxQueueRetry, "LogSettingsRead");


      int i = 0;

      foreach (CompanySettings company in this.xmlSettings.CompanySettingsList)
      {
        i++;

        Trace.WriteLine("Company" + i, "LogSettingsRead");
        Trace.WriteLine("DatabaseName:                   = " + company.DatabaseName, "LogSettingsRead");
        Trace.WriteLine("XMLOutboxPath:                  = " + company.XMLOutboxPath, "LogSettingsRead");
      }
    }

    private void DataTransferButton_Click(object sender, EventArgs e)
    {
      this.ReadSettings();

      Debug.Listeners.Add(
        new VanLeeuwen.Framework.Diagnostics.LogFileTraceListener(
          this.logFolder,
          "Van Leeuwen Web DataTransfer Service",
          VanLeeuwen.Framework.Logging.Logger.LogFileSettings.LogFilePerDay,
          VanLeeuwen.Framework.Logging.Logger.LogFolderSettings.LogFolderPerYear | VanLeeuwen.Framework.Logging.Logger.LogFolderSettings.LogFolderPerMonth,
          VanLeeuwen.Framework.Logging.Logger.TimeStamps.DateTime));

      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine("Started Van Leeuwen Web DataTransfer Service...", "OnStart");
      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine(String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()), "OnStart");
      Trace.WriteLine("Logger attached", "OnStart");

      this.LogSettingsRead();

      directoryWatcher = new InboxWatcher();
      directoryWatcher.Start();

      if (!String.IsNullOrEmpty(this.localQueuePath))
      {
        RetryMessagesInQueue();
        InitiateScheduler();
      }
    }

    private void InitiateScheduler()
    {
      // Timed: Retrying messages in Queue
      ExecuteTasksDelegate objExecuteTasksDelegate = new ExecuteTasksDelegate(this.RetryMessagesInQueue);

      ScheduleRepeatingPattern objScheduleRepeatingPattern = new ScheduleRepeatingPattern(RepeatingPattern.PerHour, DateTime.Now);
      ProcessQueueItem objProcessQueueItem = new ProcessQueueItem(objExecuteTasksDelegate, new object[] { });
      ScheduleItem objScheduleItem = new ScheduleItem(objScheduleRepeatingPattern, objProcessQueueItem);
      this.taskScheduler.Insert(objScheduleItem, false);

      // Starts the scheduler
      Trace.WriteLine("Scheduler starting...", "InitializeScheduler");
      this.taskScheduler.Start(1000);
      Trace.WriteLine("Scheduler started", "InitializeScheduler");
    }

    private void RetryMessagesInQueue()
    {
      Trace.WriteLine("Retrying messages in Queue...", "RetryMessagesInQueue");

      MessageQueue messageQueue = new MessageQueue(this.localQueuePath, this.maxQueueRetry);
      messageQueue.BeginReceive();

      Trace.WriteLine("Retrying messages in Queue completed", "RetryMessagesInQueue");
    }
  }
}

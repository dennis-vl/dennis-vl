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
using VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.DataTransferService
{
  partial class DataTransfer : ServiceBase
  {
    private String companiesFilePath = String.Empty;
    private XMLSettingsReader xmlSettings;
    private OutboxWatcher outboxWatcher;
		private InboxWatcher inboxWatcher;
    private List<CompanySettings> companies;
		private Thread worker1;
		private Thread worker2;
		private ManualResetEvent shutdownEvent = new ManualResetEvent(false);

    // Delegate used for the tasks
    private delegate void ExecuteTasksDelegate();

    // Instantiates a new Scheduler.
    private Scheduler taskScheduler = new Scheduler();

    public DataTransfer()
    {
      InitializeComponent();
    }

    protected override void OnStart(string[] args)
    {
      this.ReadSettings();

      Debug.Listeners.Add(
        new VanLeeuwen.Framework.Diagnostics.LogFileTraceListener(
          Parameters_DataTransfer.LogFolder,
          "Van Leeuwen WebPortal DataTransfer Service",
          VanLeeuwen.Framework.Logging.Logger.LogFileSettings.LogFilePerDay,
          VanLeeuwen.Framework.Logging.Logger.LogFolderSettings.LogFolderPerYear | VanLeeuwen.Framework.Logging.Logger.LogFolderSettings.LogFolderPerMonth,
          VanLeeuwen.Framework.Logging.Logger.TimeStamps.DateTime));

      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine("Started Van Leeuwen WebPortal DataTransfer Service...", "OnStart");
      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine(String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()), "OnStart");
      Trace.WriteLine("Logger attached", "OnStart");

      this.LogSettingsRead();

      outboxWatcher = new OutboxWatcher();
      outboxWatcher.Start();

			inboxWatcher = new InboxWatcher();
			inboxWatcher.Start();

			shutdownEvent.Reset();

			worker1 = new Thread(StartFileReceiver);
			worker1.Name = "FileReceiver";
			worker1.IsBackground = true;
			worker1.Start();

			worker2 = new Thread(StartTempFolderCleaner);
			worker2.Name = "TempFolderCleaner";
			worker2.IsBackground = true;
			worker2.Start();

      if (!String.IsNullOrEmpty(Parameters_DataTransfer.LocalQueuePath))
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

      MessageQueue messageQueue = new MessageQueue(Parameters_DataTransfer.LocalQueuePath, Parameters_DataTransfer.MaxQueueRetry);
      messageQueue.BeginReceive();

      Trace.WriteLine("Retrying messages in Queue completed", "RetryMessagesInQueue");
    }
		
		public void StartTempFolderCleaner()
		{
			while (!shutdownEvent.WaitOne(0))
			{
				TempFolderCleaner.Start();
			}
		}

		public void StartFileReceiver()
		{
			while (!shutdownEvent.WaitOne(0))
			{
				FileReceiver.Start();
			}
		}

    protected override void OnStop()
    {
      try
      {
        this.outboxWatcher.Stop();
        this.taskScheduler.Stop();

				shutdownEvent.Set();
				if (!worker1.Join(3000))
				{ // give the thread 3 seconds to stop
					worker1.Abort();
				}

				shutdownEvent.Set();
				if (!worker2.Join(3000))
				{ // give the thread 3 seconds to stop
					worker2.Abort();
				}
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message, "OnStop");
      }

      Trace.WriteLine("-------------------------------------------------------------------");
      Trace.WriteLine("Stopped Van Leeuwen WebPortal DataTransfer Service...", "OnStop");
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

        Parameters_DataTransfer.LogFolder = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["Logging"].ToString();
        Parameters_DataTransfer.OutboxFolder = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["WatchFolder"].ToString();
				Parameters_DataTransfer.QueueFolder = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["QueueFolder"].ToString();
        Parameters_DataTransfer.LocalQueuePath = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["LocalQueuePath"].ToString();
				Parameters_DataTransfer.InboxFolder = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["InboxFolder"].ToString();
        Parameters_DataTransfer.MaxQueueRetry = Convert.ToInt32(VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["MaxQueueRetry"].ToString());
        Parameters_DataTransfer.TempFolder = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["TempFolder"].ToString();
				Parameters_DataTransfer.ProcessedFolder = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["ProcessedFolder"].ToString();
				Parameters_DataTransfer.FailedFolder = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["FailedFolder"].ToString();

				Parameters_DataTransfer.SAPLoginSettings.SAP_AppServerHost = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["SAP_AppServerHost"].ToString();
				Parameters_DataTransfer.SAPLoginSettings.SAP_Client = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["SAP_Client"].ToString();
				Parameters_DataTransfer.SAPLoginSettings.SAP_SystemID = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["SAP_SystemID"].ToString();
				Parameters_DataTransfer.SAPLoginSettings.SAP_SystemNumber = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["SAP_SystemNumber"].ToString();
				Parameters_DataTransfer.SAPLoginSettings.SAP_User = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["SAP_User"].ToString();
				Parameters_DataTransfer.SAPLoginSettings.SAP_Password = VanLeeuwen.Projects.WebPortal.DataTransferService.Properties.Settings.Default["SAP_Password"].ToString();

        Trace.WriteLine("Reading Companies...", "ReadSettings");
        this.xmlSettings = new XMLSettingsReader(this.companiesFilePath);
        xmlSettings.ReadXML();

        Trace.WriteLine("Reading Companies complete", "ReadSettings");
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
      Trace.WriteLine("Settings.Logging                 = " + Parameters_DataTransfer.LogFolder, "LogSettingsRead");
      Trace.WriteLine("Settings.WatchFolder             = " + Parameters_DataTransfer.OutboxFolder, "LogSettingsRead");
			Trace.WriteLine("Settings.QueueFolder             = " + Parameters_DataTransfer.QueueFolder, "LogSettingsRead");
      Trace.WriteLine("Settings.LocalQueuePath          = " + Parameters_DataTransfer.LocalQueuePath, "LogSettingsRead");
      Trace.WriteLine("Settings.MaxQueueRetry           = " + Parameters_DataTransfer.MaxQueueRetry, "LogSettingsRead");
      Trace.WriteLine("Settings.TempFolder              = " + Parameters_DataTransfer.TempFolder, "LogSettingsRead");
			Trace.WriteLine("Settings.ProcessedFolder         = " + Parameters_DataTransfer.ProcessedFolder, "LogSettingsRead");
			Trace.WriteLine("Settings.FailedFolder            = " + Parameters_DataTransfer.FailedFolder, "LogSettingsRead");

			Trace.WriteLine(String.Empty, "LogSettingsRead");
			Trace.WriteLine("SAP Connection", "LogSettingsRead");
			Trace.WriteLine("Settings.SAPLoginSettings.SAP_AppServerHost    = " + Parameters_DataTransfer.SAPLoginSettings.SAP_AppServerHost, "LogSettingsRead");
			Trace.WriteLine("Settings.SAPLoginSettings.SAP_SystemID         = " + Parameters_DataTransfer.SAPLoginSettings.SAP_SystemID, "LogSettingsRead");
			Trace.WriteLine("Settings.SAPLoginSettings.SAP_SystemNumber     = " + Parameters_DataTransfer.SAPLoginSettings.SAP_SystemNumber, "LogSettingsRead");
			Trace.WriteLine("Settings.SAPLoginSettings.SAP_Client           = " + Parameters_DataTransfer.SAPLoginSettings.SAP_Client, "LogSettingsRead");
			Trace.WriteLine("Settings.SAPLoginSettings.SAP_User             = " + Parameters_DataTransfer.SAPLoginSettings.SAP_User, "LogSettingsRead");
			Trace.WriteLine(String.Empty, "LogSettingsRead");

      int i = 0;

      foreach (CompanySettings company in this.xmlSettings.CompanySettingsList)
      {
        i++;

        Trace.WriteLine("Company" + i, "LogSettingsRead");
        Trace.WriteLine("DatabaseName:                   = " + company.DatabaseName, "LogSettingsRead");
        Trace.WriteLine("XMLOutboxPath:                  = " + company.XMLOutboxPath, "LogSettingsRead");
				Trace.WriteLine(String.Empty, "LogSettingsRead");
      }
    }
  }
}

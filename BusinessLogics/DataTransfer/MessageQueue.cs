using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using VanLeeuwen.Projects.WebPortal.DataAccess;
using VanLeeuwen.Projects.WebPortal.DataAccess.DataTransfer;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class MessageQueue
  {
    System.Messaging.MessageQueue mq;
    private String queueFailedFolder;
		private String queueFolder;
    private Int32 maxQueueRetry;

    public MessageQueue(String localQueuePath, Int32 maxQueueRetry)
    {
      this.maxQueueRetry = maxQueueRetry;
			this.queueFolder = Parameters_DataTransfer.QueueFolder;

      // Create queue if not exists
      if (!System.Messaging.MessageQueue.Exists(localQueuePath))
        System.Messaging.MessageQueue.Create(localQueuePath);

      mq = new System.Messaging.MessageQueue(localQueuePath);
      mq.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(QueueMessage) });
      mq.DefaultPropertiesToSend.Recoverable = true;
    }

    public void ReceiveLastMessage()
    {
      Trace.WriteLine("Start Receiving and sending last message from Queue", "ReceiveLastMessage");

      List<Message> messages = mq.GetAllMessages().ToList();

      if (messages.Count > 0)
      {
        Message lastMessage = messages.Last();

        QueueMessage queueMessage = (QueueMessage)mq.ReceiveById(lastMessage.Id).Body;
        String errorMessage = String.Empty;

				// Get byte array from file
				VanLeeuwen.Framework.IO.File.WaitForRelease(queueMessage.FilePath, 10);
				byte[] fileContents = File.ReadAllBytes(queueMessage.FilePath);

				if (FileSender.SendFile(queueMessage.DatabaseName, fileContents, queueMessage.FileName, out errorMessage))
        {
          // Succesfully Send!!!
					// remove file from Queue folder
					File.Delete(queueMessage.FilePath);
        }
        else
        {
          AddMessage(queueMessage);
        }

        Trace.WriteLine("Receiving and sending last message from Queue completed", "ReceiveLastMessage");
      }
    }

    public void BeginReceive()
    {
      Trace.WriteLine("Start receiving and sending messages from Queue", "BeginReceive");

      List<Message> messages = mq.GetAllMessages().ToList();

      foreach (Message message in messages)
      {
        QueueMessage queueMessage = (QueueMessage)mq.Receive().Body;
        String errorMessage = String.Empty;

				// Get byte array from file
				VanLeeuwen.Framework.IO.File.WaitForRelease(queueMessage.FilePath, 10);
				byte[] fileContents = File.ReadAllBytes(queueMessage.FilePath);

        if (FileSender.SendFile(queueMessage.DatabaseName, fileContents, queueMessage.FileName, out errorMessage))
        {
          // Succesfully Send!!!
					// remove file from Queue folder
					File.Delete(queueMessage.FilePath);
        }
        else
        {
          // Add to Queue
          if (queueMessage.NumberOfTries <= this.maxQueueRetry)
          {
            AddMessage(queueMessage);
          }
          else
          {
            MessageFailed(queueMessage);
          }
        }
      }

      Trace.WriteLine("Receiving and sending messages from Queue completed", "BeginReceive");
    }

    private void MessageFailed(QueueMessage queueMessage)
    {
      //String filepath = Path.Combine(this.queueFailedFolder, queueMessage.XmlFileName);

      //BinaryWriter binWriter = new BinaryWriter(File.Open(filepath, FileMode.CreateNew, FileAccess.ReadWrite));
      //binWriter.Write(queueMessage.File);
      //binWriter.Close();
    }

		//public Boolean AddMessage(String databaseName, String filePath, String fileName)
		//{
		//	//byte[] file = VanLeeuwen.Framework.IO.ByteArray.GetByteArrayFromString(xmlMessage);
		//	return this.AddMessage(databaseName, filePath, fileName);
		//}

		public Boolean AddMessage(String databaseName, String filePath, String fileName)
    {
			// Move to Queuefolder
			VanLeeuwen.Framework.IO.File.WaitForRelease(filePath, 10);
			VanLeeuwen.Framework.IO.File.MoveAndOverwrite(filePath, Path.Combine(Parameters_DataTransfer.QueueFolder, fileName));

			// Add Message to Queue
      QueueMessage message = new QueueMessage();
      message.DatabaseName = databaseName;
			message.FilePath = Path.Combine(Parameters_DataTransfer.QueueFolder, fileName);
      message.FileName = fileName;
      message.NumberOfTries = -1;

      return this.AddMessage(message);
    }

    public Boolean AddMessage(QueueMessage queueMessage)
    {
      try
      {
        queueMessage.NumberOfTries++;
        mq.Send(queueMessage);

        Trace.WriteLine(String.Format("Added message to Queue. NumberOfTries: {0}", queueMessage.NumberOfTries), "AddMessage");
      }
      catch (Exception ex)
      {
        return false;
      }

      return true;
    }
  }
}

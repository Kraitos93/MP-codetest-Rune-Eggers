using LogComponent;
using Newtonsoft.Json;
using System;
using System.IO;

namespace LogTest
{
    public class AsyncLog : ILog
    {

        private bool stopped = false;

        public bool Stopped
        {
            get { return stopped; }
        }

        private DateTime currentDate;

        private const string timezone = "Central European Standard Time";

        private string logQueue;
        public string LogQueue
        {
            get { return logQueue; }
        }

        private string logDirectory = @"C:\LogTest\";

        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private string timeFormat = "yyyy-MM-dd HH:mm:ss fff";

        public AsyncLog(string logQueue)
        {
            if (!Directory.Exists(logDirectory)) 
                Directory.CreateDirectory(logDirectory);

            this.logQueue = logQueue;

            this.currentDate = DateUtility.GetTime(timezone);

            this.filePath = SetUpNewLogFile(FormatLogFilePath(logDirectory, currentDate, timeFormat));

            RabbitMQHelper.CreateQueue(logQueue);

        }

        public void Stop_Without_Flush()
        {
            this.stopped = true;
        }

        public void Stop_With_Flush()
        {
            this.stopped = true;
            RabbitMQHelper.FlushQueue(logQueue);
        }

        public string WriteLog(string message)
        {
            if(!stopped)
            {
                CheckMidnightUpdate();

                string timeStampMessage = SendMessageToLog(message);
                return timeStampMessage;
            }
            return null;
        }

        private void CheckMidnightUpdate()
        {
            var currentTime = DateUtility.GetTime(timezone);
            if (!DateUtility.IsSameDate(currentTime, currentDate))
            {
                this.currentDate = currentTime;
                this.filePath = SetUpNewLogFile(FormatLogFilePath(logDirectory, currentDate, timeFormat));
            }
        }

        private string SendMessageToLog(string message)
        {
            string timeStamp = DateUtility.GetTimeStamp(DateUtility.GetTime(), timeFormat);
            var MessageObject = new MessageEntity { FilePath = filePath, MessageBody = message, TimeStamp = timeStamp };
            var jsonBody = JsonConvert.SerializeObject(MessageObject);
            RabbitMQHelper.SendMessage(jsonBody, logQueue);
            return timeStamp;
        }

        private string SetUpNewLogFile(string path)
        {
            using (var writer = File.AppendText(path))
            {
                writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
            }
            return path;
        }

        private string FormatLogFilePath(string path, DateTime date, string timeFormat)
        {
            return path + DateUtility.GetTimeStamp(date, timeFormat) + ".log";
        }
    }
}
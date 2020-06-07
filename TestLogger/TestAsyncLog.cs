using LogComponent;
using LogTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace TestLogger
{

    [TestClass]
    public class TestAsyncLog
    {
        private string filePath;
        private AsyncLog asyncLog;


        [TestInitialize]
        public void Init()
        {
            asyncLog = new AsyncLog("Test_Logger");
            filePath = asyncLog.FilePath;
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(filePath);
            RabbitMQHelper.FlushQueue("Test_Logger");
        }

        [TestMethod]
        public void TestWritingLogLine()
        {
            string timeStamp = asyncLog.WriteLog("This is a test log message");

            Thread.Sleep(200);
                
            string expectedLogFile = LogFileLineFormat("Timestamp", "Data") + LogFileLineFormat(timeStamp, "This is a test log message");

            Assert.AreEqual(expectedLogFile.Trim(), 
                File.ReadAllText(filePath).Trim());
        }

        [TestMethod]
        public void TestMidnightChange()
        {
            DateTime beforeMidnight = new DateTime(2020, 6, 6, 23, 59, 59);
            DateTime afterMidnight = beforeMidnight.AddSeconds(1);
            Assert.IsFalse(DateUtility.IsSameDate(beforeMidnight, afterMidnight));
        }


        [TestMethod]
        public void TestStopWithoutFlush()
        {
            asyncLog.Stop_Without_Flush();
            Assert.IsTrue(asyncLog.Stopped);
        }

        [TestMethod]
        public void TestStopWithFlush()
        {
            asyncLog.Stop_With_Flush();
            Assert.IsTrue(asyncLog.Stopped);
        }

        private string LogFileLineFormat(string timestamp, string data)
        {
            return timestamp.PadRight(25, ' ') + "\t" + data.PadRight(15, ' ') + "\t" + Environment.NewLine;
        }
    }
}

using LogTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestLogger
{
    [TestClass]
    public class TestRabbitMQWrapper
    {
        private string filePath { get; set; }
        private AsyncLog asyncLog { get; set; }

        private string jsonMessage { get; set; }

        [TestInitialize]
        public void Init()
        {
            asyncLog = new AsyncLog("Test_Logger_Rabbit");
            filePath = asyncLog.FilePath;
            jsonMessage = JsonConvert.SerializeObject(
                new MessageEntity { FilePath = filePath, MessageBody = "Test Message", TimeStamp = DateTime.Now.ToString() });
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(filePath);
            RabbitMQHelper.FlushQueue("Test_Logger_Rabbit");
        }


        [TestMethod]
        public void TestSendMessage()
        {
            int messageCountBefore = RabbitMQHelper.GetMessageCount("Test_Logger_Rabbit");
            RabbitMQHelper.SendMessage(jsonMessage, "Test_Logger_Rabbit");
            int messageCountAfter = RabbitMQHelper.GetMessageCount("Test_Logger_Rabbit");
            Assert.AreEqual(0, messageCountBefore);
            Assert.AreEqual(1, messageCountAfter);
        }

        [TestMethod]
        public void TestFlushQueue()
        {
            RabbitMQHelper.SendMessage(jsonMessage, "Test_Logger_Rabbit");
            RabbitMQHelper.SendMessage(jsonMessage, "Test_Logger_Rabbit");
            RabbitMQHelper.SendMessage(jsonMessage, "Test_Logger_Rabbit");
            RabbitMQHelper.SendMessage(jsonMessage, "Test_Logger_Rabbit");
            RabbitMQHelper.SendMessage(jsonMessage, "Test_Logger_Rabbit");

            int messageCount = RabbitMQHelper.GetMessageCount("Test_Logger_Rabbit");
            Assert.AreEqual(5, messageCount);

            RabbitMQHelper.FlushQueue("Test_Logger_Rabbit");

            int messageCountAfterFlush = RabbitMQHelper.GetMessageCount("Test_Logger_Rabbit");
            Assert.AreEqual(0, messageCountAfterFlush);

        }

    }
}

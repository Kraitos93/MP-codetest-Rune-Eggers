using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using LogTest;
using Newtonsoft.Json;
using System.IO;

namespace Consumer
{

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Logger",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    MessageEntity messageEntity = JsonConvert.DeserializeObject<MessageEntity>(message);

                    WriteToFile(messageEntity.FilePath, FormatLogMessage(messageEntity.MessageBody, messageEntity.TimeStamp));

                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "Logger",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static void WriteToFile(string filepath, string content)
        {
            using(var writer = File.AppendText(filepath))
            {
                writer.Write(content);
            }
        }


        private static string FormatLogMessage(string message, string timeStamp)
        {
            return timeStamp.PadRight(25, ' ') + "\t" + message + "\t" + Environment.NewLine;
        }
    }
}

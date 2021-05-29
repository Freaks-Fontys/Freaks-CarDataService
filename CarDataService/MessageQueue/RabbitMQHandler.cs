using CarDataService.Collectors;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataService.MessageQueue
{
    public class RabbitMQHandler
    {
        IConnectionFactory factory;
        IConnection connection;
        IModel channel;
        string _queueName;

        CarDataCollector carDataCollector = new CarDataCollector();


        public RabbitMQHandler(string queueName)
        {
            _queueName = queueName;
            SetupMQ();
        }

        private void SetupMQ()
        {
            factory = new ConnectionFactory
            {
                // Username and password are hardcoded
                Uri = new Uri("amqp://guest:freaks@localhost:5672")
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(_queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void SendMessage(object message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish("", _queueName, null, body);
        }

        public void QueueListener()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Do something with the message
                // await carDataCollector.GetCarDataOnVin(message);
                await carDataCollector.GetCarDataOnModel(message.Substring(0), message.Substring(0), message.Substring(0));
            };

            channel.BasicConsume(_queueName, true, consumer);
        }
    }
}

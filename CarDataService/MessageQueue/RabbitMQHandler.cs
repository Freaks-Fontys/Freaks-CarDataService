using CarDataService.Collectors;
using CarDataService.Models;
using Microsoft.Extensions.Options;
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
        RabbitMQConfig config;

        CarDataCollector carDataCollector = new CarDataCollector();


        public RabbitMQHandler(IOptions<RabbitMQConfig> options)
        {
            config = options.Value;
            SetupMQ();
        }

        private void SetupMQ()
        {
            factory = new ConnectionFactory
            {
                Uri = new Uri(config.URI)
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void SendMessage(object message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish("", config.QueueName, null, body);
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
                await carDataCollector.GetCarDataOnModel("BMW" , "M4", "2018");
            };

            channel.BasicConsume(config.QueueName, true, consumer);
        }
    }
}

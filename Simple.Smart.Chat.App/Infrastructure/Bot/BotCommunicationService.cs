using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Simple.Smart.Chat.App.Helpers;
using Simple.Smart.Chat.App.Infrastructure.Hubs;
using Simple.Smart.Chat.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Smart.Chat.App.Infrastructure.Bot
{
    public interface IRabbitMQService
    {
        void Connect();
        void Send(string command);
    }

    public class BotCommunicationService : IRabbitMQService
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IConfiguration _configuration;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly BotSettings botSettings;

        public BotCommunicationService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            // Get bot settings.
            _configuration = configuration;
            botSettings = _configuration.GetSection("BotServiceSettings").Get<BotSettings>();

            // Opens the connections to RabbitMQ
            _factory = new ConnectionFactory()
            {
                HostName = botSettings.HostName,
                UserName = botSettings.UserName,
                Password = botSettings.Password,
                Port = botSettings.Port,
                RequestedConnectionTimeout = botSettings.RequestedConnectionTimeout
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _serviceProvider = serviceProvider;
        }

        public virtual void Connect()
        {
            // Declare a RabbitMQ Queue
            _channel.QueueDeclare(queue: botSettings.InboundQueue, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);

            // When we receive a message from SignalR
            consumer.Received += delegate (object model, BasicDeliverEventArgs ea)
            {
                // Get the ChatHub from SignalR (using DI)
                var chatHub = (IHubContext<ChatRoomHub>)_serviceProvider.GetService(typeof(IHubContext<ChatRoomHub>));
                var body = ea.Body;
                var msg = Encoding.UTF8.GetString(body);
                var outMessage = new ChatMessage()
                {
                    UserName = botSettings.BotName,
                    DateSent = DateTime.Now,
                    Message = msg
                };
                // Send message to all users in SignalR
                chatHub.Clients.All.SendAsync("receiveMessage", outMessage);
            };

            // Consume a RabbitMQ Queue
            _channel.BasicConsume(queue: botSettings.InboundQueue, autoAck: true, consumer: consumer);
        }

        public void Send(string command)
        {
            _channel.QueueDeclare(queue: botSettings.OutboundQueue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(command);
            _channel.BasicPublish(exchange: "",
                                 routingKey: botSettings.OutboundQueue,
                                 basicProperties: null,
                                 body: body);
        }
    }
}

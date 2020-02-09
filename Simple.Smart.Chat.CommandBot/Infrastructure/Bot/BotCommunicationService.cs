using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Simple.Smart.Chat.CommandBot.Infrastructure.CommandProcessing;
using Simple.Smart.Chat.CommandBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Smart.Chat.CommandBot.Infrastructure.Bot
{
    public interface IRabbitMQService
    {
        void Connect();
        void Send(string commandResponse);
    }

    public class BotCommunicationService : IRabbitMQService
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IConfiguration _configuration;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICommandProcessor _commandProcessor;
        protected readonly BotSettings botSettings;
        protected readonly ILogger<BotCommunicationService> _logger;

        public BotCommunicationService(
            IServiceProvider serviceProvider, 
            IConfiguration configuration,
            ICommandProcessor commandProcessor,
            ILogger<BotCommunicationService> logger)
        {
            // Get bot settings.
            _configuration = configuration;
            _commandProcessor = commandProcessor;
            _logger = logger;
            botSettings = _configuration.GetSection("BotServiceSettings").Get<BotSettings>();

            // Opens the connections to RabbitMQ
            _factory = new ConnectionFactory()
            {
                HostName = botSettings.HostName,
                UserName = botSettings.UserName,
                Password = botSettings.Password,
                Port = botSettings.Port,
                RequestedConnectionTimeout = botSettings.RequestedConnectionTimeout,
                DispatchConsumersAsync = true
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _serviceProvider = serviceProvider;
        }

        public virtual void Connect()
        {
            // Declare a RabbitMQ Queue
            _channel.QueueDeclare(queue: botSettings.InboundQueue, durable: false, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            // When we receive a message from SignalR
            consumer.Received += Consumer_Received;

            // Consume a RabbitMQ Queue
            _channel.BasicConsume(queue: botSettings.InboundQueue, autoAck: true, consumer: consumer);
        }

        private async Task Consumer_Received(object model, BasicDeliverEventArgs ea)
        {
            // Process Command received from chatroom
            var body = ea.Body;
            var command = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Worker received command {command} at: {DateTimeOffset.Now}");
            var commandResult = await _commandProcessor.ProcessCommand(command);
            Send(commandResult);
        }

        public void Send(string commandResponse)
        {
            _channel.QueueDeclare(queue: botSettings.OutboundQueue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(commandResponse);
            _channel.BasicPublish(exchange: "",
                                 routingKey: botSettings.OutboundQueue,
                                 basicProperties: null,
                                 body: body);
        }
    }
}

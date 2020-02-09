using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simple.Smart.Chat.CommandBot.Infrastructure.Bot;

namespace Simple.Smart.Chat.CommandBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMQService _mQService;

        public Worker(ILogger<Worker> logger, IRabbitMQService mQService)
        {
            _logger = logger;
            _mQService = mQService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mQService.Connect();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}

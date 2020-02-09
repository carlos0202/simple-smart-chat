using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Simple.Smart.Chat.CommandBot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Smart.Chat.CommandBot.Infrastructure.CommandProcessing
{
    public interface ICommandProcessor
    {
        string ProcessCommand(string command);
    }

    public class CommandProcessorService : ICommandProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CommandProcessorService> _logger;

        public CommandProcessorService(
            IConfiguration configuration,
            ILogger<CommandProcessorService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string ProcessCommand(string command)
        {
            try
            {
                var url = $"{_configuration.GetValue<string>("StockApiUrl")}?s={command}&f=sd2t2ohlcv&h&e=csv";
                _logger.LogInformation($"Get Command info using URL: {url}");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var result = response.GetResponseStream();
                using var reader = new StreamReader(result);
                using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                if (csvReader.Read())
                {
                    var record = csvReader.GetRecord<StockResult>();
                    var rawRecord = csvReader.GetRecord<object>();
                    _logger.LogInformation($"Result of Command : {rawRecord}");
                    if (record != null && record.Close != "N/D")
                    {
                        return $"{command} quote is ${record.Close} per share";
                    }
                }

                return $"No results found for command /stock={command}";
            }
            catch (Exception ex)
            {
                return $"Error while processing the command: {command}. Error: {ex.Message}";
            }
        }
    }


}

using CsvHelper;
using Microsoft.Extensions.Configuration;
using Simple.Smart.Chat.CommandBot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Smart.Chat.CommandBot.Infrastructure.CommandProcessing
{
    public interface ICommandProcessor
    {
        Task<string> ProcessCommand(string command);
    }

    public class CommandProcessorService : ICommandProcessor
    {
        private readonly IConfiguration _configuration;

        public CommandProcessorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> ProcessCommand(string command)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var result = await client.GetStreamAsync(_configuration.GetValue<string>("StockApiUrl")))
                    {
                        var reader = new StreamReader(result);
                        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var record = csvReader.GetRecords<StockResult>().FirstOrDefault();
                            if (record != null && record.Close != "N/D")
                            {
                                return $"{command} quote is ${record.Close} per share";
                            }

                            return $"No results found for command /stock={command}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error while processing the command: {command}. Error: {ex.Message}";
            }
        }
    }


}

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NATS.Client;
using System.Text;
using System.Text.Json;
using DiceNats.Models;
using System.Linq;

namespace DiceNats
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly Random _random;

        public Worker(ILogger<Worker> logger, ConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _random = new System.Random();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using(var connection = _connectionFactory.CreateConnection())
            {
                var subscription = connection.SubscribeAsync("dice.roll", HandleMessage);
                await stoppingToken.WhenCanceled();
                _logger.LogInformation("STOPPING!");
                
            }
        }

        private void HandleMessage(Object sender, MsgHandlerEventArgs args)
        {
            _logger.LogInformation(Encoding.UTF8.GetString(args.Message.Data));
            var req = JsonSerializer.Deserialize<RollRequest>(Encoding.UTF8.GetString(args.Message.Data));
            var results = Enumerable.Range(0,req.NumDice).Select(i => _random.Next(req.SidesPerDie) + 1);
            var response = new RollReply {Rolls = results.ToArray()};
            args.Message.Respond(response);
        }

    }
}

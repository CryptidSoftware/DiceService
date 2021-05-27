using Iolaus;
using Iolaus.Observer;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using DiceNats.Models;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace DiceNats
{
    [Handler]
    public class DiceHandler
    {
        private ILogger<DiceHandler> _logger;
        private Random _random;

        public DiceHandler(ILogger<DiceHandler> logger)
        {
            _logger = logger;
            _random = new Random();
        }

        [Pattern("{'type': 'dice', 'cmd': 'roll'}")]
        public async Task Roll(Message msg, Func<Message,Task> reply)
        {
            _logger.LogInformation($"Recieved {msg}");
            var request = JsonSerializer.Deserialize<RollRequest>(msg.ToString());
            var results = Enumerable.Range(0,request.NumDice).Select(i => _random.Next(request.SidesPerDie) + 1);
            var response = new RollReply {Rolls = results.ToArray()};
            var replyMsg = Message.FromObject(response).Unsafe();
            _logger.LogInformation($"Replying {replyMsg}");
            await reply(replyMsg);
        }
    }
}
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DiceHttp.Models;

namespace DiceHttp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiceController : ControllerBase
    {
        private readonly Random _random;
        private readonly ILogger<DiceController> _logger;

        public DiceController(ILogger<DiceController> logger)
        {
            _logger = logger;
            _random = new Random();
        }

        [HttpPost("roll")]
        public RollReply Roll([FromBody]RollRequest request)
        {
            var results = Enumerable.Range(0,request.NumDice).Select(i => _random.Next(request.SidesPerDie) + 1);
            return new RollReply {Rolls = results.ToArray()};
        }
    }
}

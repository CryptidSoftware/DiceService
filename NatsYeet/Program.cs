using System;
using NATS.Client;
using System.Text;

namespace NatsYeet
{
    class Program
    {

        private static readonly int NumMessages = 50000;

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            var client = factory.CreateConnection();

            var bytes = Encoding.UTF8.GetBytes("{\"NumDice\": 10, \"SidesPerDie\": 20}");

            for(int i = 0; i < NumMessages; i += 1)
            {
                var reply = client.Request("dice.roll",bytes);
                if(reply.Data.Length < 10) 
                {
                    throw(new Exception("Empty Response"));
                }
            }

            Console.WriteLine($"{NumMessages} messages completed");

        }
    }
}

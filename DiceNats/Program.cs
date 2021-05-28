using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using Iolaus.Nats;
using Iolaus.Observer;

namespace DiceNats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ObserverRouter>((sp) => {
                        var or = new ObserverRouter(sp);
                        or.LoadHandlers(typeof(Program).Assembly);
                        return or;
                    });
                    services.Configure<NatsListenerOptions>( opt => {
                        opt.Topic = "dice.roll";
                    });
                    services.AddSingleton<IConnection>( (provider) => new ConnectionFactory().CreateConnection());
                    services.AddTransient<NatsListenerServices>();
                    services.AddHostedService<NatsListener>();
                });
    }
}

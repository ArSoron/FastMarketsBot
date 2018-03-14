using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using Telegram.Bot;
using FastMarketsBot.Services.Telegram;
using FastMarketsBot.Services.Telegram.Commands;
using FastMarketsBot.Services.Telegram.Helpers;
using FastMarkets.MindTricksService;
using FastMarkets.MindTricksService.Data;
using FastMarkets.MindTricksService.DataAccess;

namespace FastMarketsBot.ConsoleRunner
{
    public class Program
    {
        private static ManualResetEvent _completionEvent = new ManualResetEvent(false);
        private static IConfigurationRoot _configurationRoot;

        public static void Main(string[] args)
        {
            _configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddScoped<ITelegramService, TelegramService>()
            .AddSingleton<ITelegramConfiguration, TelegramConfiguration>()
            .AddSingleton<ITelegramBotClient>((sp) => {
                var config = sp.GetRequiredService<ITelegramConfiguration>();
                return new TelegramBotClient(config.Apikey);
            })
            .AddScoped<SelfUpdatingMessage>()
            .AddScoped<ICommandFactory, CommandFactory>()
            .AddScoped<IMindTricksService, MindTricksService>()
            .AddScoped<MySqlMarkets>()
                .AddScoped((sp) => {
                    return new MarketsContext(_configurationRoot["ConnectionStrings:MindTricks"]);
                })
            .BuildServiceProvider();

#if DEBUG
            serviceProvider
            .GetService<ILoggerFactory>()
            .AddConsole(LogLevel.Trace, true);
#else
            serviceProvider
            .GetService<ILoggerFactory>()
            .AddConsole(LogLevel.Information, true);
#endif

            using (var service = serviceProvider.GetService<ITelegramService>())
            {

                service.Register();

                service.StayingAlive();

                _completionEvent.WaitOne();
            }
        }
        private class TelegramConfiguration : ITelegramConfiguration
        {
            public string Apikey => _configurationRoot["telegram:apiKey"];
        }
    }
}

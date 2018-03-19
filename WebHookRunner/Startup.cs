using FastMarkets.MindTricksService;
using FastMarkets.MindTricksService.Data;
using FastMarkets.MindTricksService.DataAccess;
using FastMarketsBot.Services.Telegram;
using FastMarketsBot.Services.Telegram.Commands;
using FastMarketsBot.Services.Telegram.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace FastMarketsBot.WebHookRunner
{
    public class Startup
    {
        private static IConfigurationRoot _configurationRoot;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            services
                .AddLogging()
                .AddScoped<ITelegramService, TelegramService>()
                .AddSingleton<ITelegramConfiguration, TelegramConfiguration>()
                .AddSingleton<ITelegramBotClient>((sp) =>
                {
                    var config = sp.GetRequiredService<ITelegramConfiguration>();
                    return new TelegramBotClient(config.Apikey);
                })
                .AddScoped<SelfUpdatingMessage>()
                .AddScoped<StartCommand>()
                .AddScoped<ICommand, FavouritesCommand>()
                .AddScoped<ICommand, SearchCommand>()
                .AddScoped<ICommand, StartCommand>()
                .AddScoped<ICommand, SymbolDetailsCommand>()
                .AddScoped<ICommandFactory, CommandFactory>(sp =>
                {
                    return new CommandFactory(sp.GetServices<ICommand>(), sp.GetRequiredService<StartCommand>());
                })
                .AddScoped<IMindTricksService, MindTricksService>()
                .AddSingleton<IWebApiConfiguration, WebApiConfiguration>()
                .AddScoped<MySqlMarkets>()
                .AddScoped((sp) =>
                {
                    return new MarketsContext(_configurationRoot["ConnectionStrings:MindTricks"]);
                })
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private class TelegramConfiguration : ITelegramConfiguration
        {
            public string Apikey => _configurationRoot["Telegram:ApiKey"];
        }

        private class WebApiConfiguration : IWebApiConfiguration
        {
            public string HostName => _configurationRoot["WebApi:HostName"];

            public string BotUrl => _configurationRoot["WebApi:BotUrl"];

            public string SubscribeKey => _configurationRoot["WebApi:SubscribeKey"];
        }
    }
}

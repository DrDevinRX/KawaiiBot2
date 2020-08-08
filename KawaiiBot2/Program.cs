using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IO;
using KawaiiBot2.Services;

namespace KawaiiBot2
{
    class Program
    {
        private DiscordSocketClient discord;


        private static void Main()
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {

            var confdef = new { token = "", prefix = "" };

            var config = JsonConvert.DeserializeAnonymousType(File.ReadAllText("conf.json"), confdef);

            if (!string.IsNullOrWhiteSpace(config.prefix))
            {
                CommandHandlerService.Prefix = config.prefix;
            }

            if (string.IsNullOrWhiteSpace(config.token))
            {
                throw new NotSupportedException("Bot token not found in config file");
            }

            discord = new DiscordSocketClient();
            var services = ConfigureServices();
            services.GetRequiredService<Services.LoggingService>();
            await services.GetRequiredService<Services.CommandHandlerService>().InitializeAsync(services);

            await discord.LoginAsync(TokenType.Bot, config.token);
            await discord.StartAsync();

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(discord)
                .AddSingleton<CommandService>()
                .AddSingleton<Services.CommandHandlerService>()
                .AddLogging()
                .AddSingleton<Services.LoggingService>()
                .BuildServiceProvider();
        }
    }
}

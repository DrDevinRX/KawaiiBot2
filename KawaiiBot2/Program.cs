using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace KawaiiBot2
{
    class Program
    {
        private DiscordSocketClient discord;
        private 

        static void Main()
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            // This can be changed to a config file
            string token = "NzQwOTc2MTU5NjkyNDIzMjgw.Xyw10w.KJxaG_CAaepwhnoNuR2bf041x3U";
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new NotSupportedException("Bot token not found in environment variable \"KAWAII_BOT_TOKEN\"");
            }

            discord = new DiscordSocketClient();
            var services = ConfigureServices();
            services.GetRequiredService<Services.LoggingService>();
            await services.GetRequiredService<Services.CommandHandlerService>().InitializeAsync(services);

            await discord.LoginAsync(TokenType.Bot, token);
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

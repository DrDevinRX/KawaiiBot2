using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IO;
using KawaiiBot2.Services;
using System.Diagnostics;

namespace KawaiiBot2
{
    class Program
    {
        private DiscordSocketClient discord;
        const string ConfName = "conf.json";
        readonly string ConfPath = Path.Combine(Directory.GetCurrentDirectory(), ConfName);

        private static void Main()
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {

            if (!File.Exists(ConfPath))
            {
                CreateConf();
            }

            var confdef = new { token = "", prefix = "" };

            var config = JsonConvert.DeserializeAnonymousType(File.ReadAllText(ConfPath), confdef);

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
            Helpers.Client = services.GetRequiredService<APIInterfacing.Client>();

            Console.WriteLine(Helpers.Client == null);

            await discord.LoginAsync(TokenType.Bot, config.token);
            await discord.StartAsync();

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(discord)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlerService>()
                .AddLogging()
                .AddSingleton<LoggingService>()
                .AddSingleton("Awooo v2")
                .AddSingleton<APIInterfacing.Client>()
                .BuildServiceProvider();
        }

        private void CreateConf()
        {
            Console.Write("Insert bot token: ");
            var token = Console.ReadLine();
            Console.Write("Insert command prefix (default is '-'): ");
            var getPrefix = Console.ReadLine();
            var prefix = string.IsNullOrWhiteSpace(getPrefix) ? "-" : getPrefix;

            File.WriteAllText(ConfPath, JsonConvert.SerializeObject(new { token = token, prefix = prefix }));
        }
    }
}

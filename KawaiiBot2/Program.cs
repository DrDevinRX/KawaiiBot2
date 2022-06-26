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
using Microsoft.Extensions.Configuration;
using KawaiiBot2.JSONClasses;
using System.Reflection;
using Discord.Interactions;

namespace KawaiiBot2
{
    class Program
    {
        private DiscordSocketClient discord;
        private static string ConfPath = "conf.json";
        public static readonly string BotName = $"Awooo v{typeof(Program).Assembly.GetName().Version.ToString(3)} (Ver My愛)";

        private static void Main(string[] args)
        {
            //set confpath if it's given
            if (args.Length > 0)
                ConfPath = args[0];
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (!File.Exists(ConfPath))
            {
                CreateConf();
            }

            var config = JsonConvert.DeserializeObject<ConfJson>(File.ReadAllText(ConfPath));

            if (config.DevIDs != null)
            {
                Helpers.devIDs = config.DevIDs;
            }

            if (!string.IsNullOrWhiteSpace(config.Prefix))
            {
                CommandHandlerService.Prefix = config.Prefix;
            }

            if (string.IsNullOrWhiteSpace(config.Token))
            {
                throw new NotSupportedException("Bot token not found in config file");
            }

            if (string.IsNullOrWhiteSpace(config.CatApiToken))
            {
                Console.WriteLine("This probably won't fail, but you want to have a cat api token.");
            }
            else
            {
                KawaiiBot2.APIInterfacing.Interfaces.CatsApiInterface.ApiKey = config.CatApiToken;
            }

            //load persistacne
            Persistance.LoadEverything(ConfPath);
            //start persistance autosave
            Persistance.StartSaveScheduler();
            GatewayIntents intents = GatewayIntents.GuildEmojis | GatewayIntents.GuildMessageReactions | GatewayIntents.GuildMessages |
                GatewayIntents.DirectMessages | GatewayIntents.Guilds | GatewayIntents.GuildMembers | GatewayIntents.GuildPresences;
            //shut up discord.net guildpresences is necessary
            var disconfig = new DiscordSocketConfig() { GatewayIntents = intents };
            discord = new DiscordSocketClient(disconfig);
            var services = ConfigureServices();
            services.GetRequiredService<Services.LoggingService>();
            await services.GetRequiredService<Services.CommandHandlerService>().InitializeAsync(services);
            var apiClient = services.GetRequiredService<APIInterfacing.Client>();
            await apiClient.InitializeAsync(services);
            Helpers.Client = apiClient;

            await discord.LoginAsync(TokenType.Bot, config.Token);
            await discord.StartAsync();
            if (config.StartStatus != null)
                await discord.SetGameAsync(config.StartStatus);

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(discord)
                .AddSingleton<CommandService>()
                .AddSingleton<InteractionService>()
                .AddSingleton<CommandHandlerService>()
                .AddLogging()
                .AddSingleton<LoggingService>()
                .AddSingleton(BotName)
                .AddSingleton<APIInterfacing.Client>()
                .BuildServiceProvider();
        }

        private static void CreateConf()
        {
            Console.Write("Insert bot token: ");
            var token = Console.ReadLine();
            Console.Write("Insert command prefix (default is '-'): ");
            var getPrefix = Console.ReadLine();
            var prefix = string.IsNullOrWhiteSpace(getPrefix) ? "-" : getPrefix;

            File.WriteAllText(ConfPath, JsonConvert.SerializeObject(new { token, prefix }));
        }
    }
}

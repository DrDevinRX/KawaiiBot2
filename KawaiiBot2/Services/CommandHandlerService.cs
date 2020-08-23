using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using KawaiiBot2.Modules;

namespace KawaiiBot2.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private IServiceProvider _provider;

        public static string Prefix { private get; set; } = "-";

        public static int CommandsExecuted { get; private set; } = 0;

        public CommandHandlerService(DiscordSocketClient discord, CommandService commands, IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;

            Help.Commands = commands;

            _discord.MessageReceived += MessageReceived;
        }

        private async Task MessageReceived(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != Discord.MessageSource.User)
            {
                return;
            }

            if (message.Author.IsBot)
            {
                return;
            }

            int argPos = 0;

            if (!message.HasStringPrefix(Prefix, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_discord, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            if (!result.Error.HasValue)
                CommandsExecuted++;

            if (result.Error.HasValue &&
                result.Error.Value == CommandError.UnknownCommand)
            {
                return;
            }
            if (result.Error.HasValue &&
                result.Error.Value != CommandError.UnknownCommand)
            {
                switch (result.Error.Value)
                {
                    case CommandError.BadArgCount:
                        await context.Channel.SendMessageAsync("T-that's not how you do that... You've got the number of things wrong!");
                        break;
                    case CommandError.Exception:
                        await context.Channel.SendMessageAsync("Woopsies! Something went wrong@");
                        break;
                    case CommandError.MultipleMatches:
                        await context.Channel.SendMessageAsync("T-There's too many of em...");
                        break;
                    case CommandError.ObjectNotFound:
                        await context.Channel.SendMessageAsync("I-is that your invisible friend?");
                        break;
                    case CommandError.ParseFailed:
                        await context.Channel.SendMessageAsync("h-hacker detected :\"(");
                        break;
                    case CommandError.UnmetPrecondition:
                        await context.Channel.SendMessageAsync(result.ErrorReason);
                        break;
                    case CommandError.Unsuccessful:
                        await context.Channel.SendMessageAsync("No u :(");
                        break;
                    default:
                        await context.Channel.SendMessageAsync("I eat u :3 u did a woopsie");
                        break;
                }
            }
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
        }
    }
}

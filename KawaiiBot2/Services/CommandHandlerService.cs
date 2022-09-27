using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using KawaiiBot2.Modules;
using System.Collections.Generic;

namespace KawaiiBot2.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly InteractionService _interactions;
        private IServiceProvider _provider;

        public static string Prefix { get; set; } = "-";

        public static int CommandsExecuted { get; private set; } = 0;

        public CommandHandlerService(DiscordSocketClient discord, CommandService commands, InteractionService interactions, IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;
            _interactions = interactions;

            Help.Commands = commands;
            Help.Provider = provider;

            _discord.MessageReceived += MessageReceived;
            _discord.InteractionCreated += InteractionCreated;

            _discord.Ready += ClientReady;

            _commands.CommandExecuted += (i, c, r) =>
            {
                if (!i.IsSpecified) return Task.Run(() => { });
                Informational.CommandCount.AddOrUpdate(i.Value.Name, 1, (k, c) => c + 1);
                return Task.Run(() => { });
            };

            _interactions.SlashCommandExecuted += (i, c, r) =>
            {
                Informational.CommandCount.AddOrUpdate(i.Name, 1, (k, c) => c + 1);
                return Task.Run(() => { });
            };
        }

        private async Task InteractionCreated(SocketInteraction socketInteraction)
        {

            if (socketInteraction.Type == Discord.InteractionType.ApplicationCommand)
            {
                var context = new SocketInteractionContext(_discord, socketInteraction);
                await _interactions.ExecuteCommandAsync(context, _provider);
                //find some way to use _commands.ExecuteAsync instead...
            }

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

            var commandname = context.Message.ToString().Substring(Prefix.Length).Split(" ")[0].ToLower();

            HashSet<string> a = new();

            if (DevManagement.NoUsingThis.TryGetValue(context.User.Id, out a))
            {
                if (a.Contains(commandname))
                {
                    return;
                }
            }
            if (DevManagement.NoUsingThis.TryGetValue(ulong.MaxValue, out a))
            {
                if (a.Contains(commandname))
                {
                    return;
                }
            }


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

        public async Task ClientReady()
        {
            await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _interactions.RegisterCommandsGloballyAsync();
            //may have to make something where it stores what's been registered with Persistance
            foreach (var guild in _discord.Guilds)
            {

                try { await _interactions.RegisterCommandsToGuildAsync(guild.Id); }
                catch
                {
                    Console.Write($"Could not register commands for guild {guild.Name}");
                }
            }
        }
    }
}

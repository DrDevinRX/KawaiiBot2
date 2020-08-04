using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KawaiiBot2.Services
{
    class LoggingService
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commandService;
        private readonly ILoggerFactory logger;
        private readonly ILogger discordLogger;
        private readonly ILogger commandsLogger;

        public LoggingService(DiscordSocketClient client, CommandService cmdService, ILoggerFactory logFactory)
        {
            discord = client;
            commandService = cmdService;
            logger = ConfigureLogging(logFactory);
            discordLogger = logger.CreateLogger("Discord");
            commandsLogger = logger.CreateLogger("Commands");

            discord.Log += LogDiscord;
            commandService.Log += LogCommandService;

        }

        private Task LogCommandService(LogMessage message)
        {
            if (message.Exception is CommandException cmd)
            {
                // Sends error as a message to original channel
                var _ = cmd.Context.Channel.SendMessageAsync($"Error: {cmd.Message}");
            }

            commandsLogger.Log(
                LogLevelFromSeverity(message.Severity),
                0,
                message,
                message.Exception,
                (_1, _2) => message.ToString(prependTimestamp: false));
            return Task.CompletedTask;
        }

        private Task LogDiscord(LogMessage message)
        {
            discordLogger.Log(
                  LogLevelFromSeverity(message.Severity),
                  0,
                  message,
                  message.Exception,
                  (_1, _2) => message.ToString(prependTimestamp: false));
            return Task.CompletedTask;
        }

        private ILoggerFactory ConfigureLogging(ILoggerFactory logger)
        {
            logger = LoggerFactory.Create(x => x.AddConsole());
            return logger;
        }

        private static LogLevel LogLevelFromSeverity(LogSeverity severity)
            => (LogLevel)(Math.Abs((int)severity - 5));

    }
}

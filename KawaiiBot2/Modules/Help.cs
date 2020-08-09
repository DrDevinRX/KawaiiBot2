using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace KawaiiBot2.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {

        public static CommandService commands { private get; set; }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("commands")]
        [Summary("Displays all of my available commands~")]

        public async Task GetHelp()
        {
            bool isDeveloper = Program.devIDs.Contains(Context.User.Id);
            var commandDescs = (from command in commands.Commands
                                where isDeveloper || !command.Attributes.Any(a => a.GetType() == typeof(HiddenCmdAttribute))
                                where isDeveloper || !command.Attributes.Any(async => async.GetType() == typeof(DevOnlyCmdAttribute))
                                orderby command.Name
                                select Helpers.Pad(command.Name, 20) + command.Summary).ToArray();
            var halflen = commandDescs.Length / 2;
            await ReplyAsync("```" + string.Join("\n", commandDescs[0..halflen]) + "```");
            await ReplyAsync("```" + string.Join("\n", commandDescs[halflen..^1]) + "```");
        }

    }
}

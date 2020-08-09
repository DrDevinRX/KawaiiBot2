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
        private static ulong[] devIDs = { 173529942431236096L };
        public static CommandService commands { private get; set; }

        [Command("help")]

        public Task GetHelp()
        {
            bool isDeveloper = devIDs.Contains(Context.User.Id);
            var commandDescs = (from command in commands.Commands
                                where isDeveloper || !command.Attributes.Any(a => a.GetType() == typeof(HiddenCmdAttribute))
                                where isDeveloper || !command.Attributes.Any(async => async.GetType() == typeof(DevOnlyCmdAttribute))
                                orderby command.Name
                                select Helpers.Pad(command.Name, 20) + command.Summary).ToArray();
            return ReplyAsync("```" + string.Join("\n", commandDescs) + "```");
        }
    }
}

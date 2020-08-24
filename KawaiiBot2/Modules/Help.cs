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

        public static CommandService Commands { private get; set; }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("commands")]
        [Summary("Displays all of my available commands~")]

        public async Task GetHelp()
        {

            //post
            await Context.Message.AddReactionAsync(new Emoji("✉"));

            //get commands
            bool isDeveloper = Helpers.devIDs.Contains(Context.User.Id);
            var commandDescs = (from command in Commands.Commands
                                where isDeveloper || !command.Attributes.Any(a => a.GetType() == typeof(HiddenCmdAttribute))
                                where isDeveloper || !command.Attributes.Any(async => async.GetType() == typeof(DevOnlyCmdAttribute))
                                orderby command.Name
                                select Helpers.Pad(command.Name, 20) + command.Summary).ToArray();
            var halflen = commandDescs.Length / 2;
            var firstMsg = "```" + string.Join("\n", commandDescs[0..halflen]) + "```";
            var secondMsg = "```" + string.Join("\n", commandDescs[halflen..^1]) + "```";

            //get dms
            var dms = await Context.User.GetOrCreateDMChannelAsync();

            //send the help to DMs
            await dms.SendMessageAsync("Thank you for consulting me to receive help~");
            await dms.SendMessageAsync(firstMsg);
            await dms.SendMessageAsync(secondMsg);


        }

    }
}

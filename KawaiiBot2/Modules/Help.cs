﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System.Diagnostics;

namespace KawaiiBot2.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {

        public static CommandService Commands { private get; set; }
        public static IServiceProvider Provider { private get; set; }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("commands")]
        [Summary("Displays all of my available commands~")]

        public async Task GetHelp()
        {

            //post
            await Context.Message.AddReactionAsync(new Emoji("✉"));

            //get commands
            bool isDeveloper = Helpers.devIDs.Contains(Context.User.Id);
            bool isHito = Context.User.Id == 173529942431236096;
            var commandDescs = (from command in Commands.Commands
                                where isDeveloper || !command.Attributes.Any(a => a.GetType() == typeof(HiddenCmdAttribute))
                                where isDeveloper || !command.Attributes.Any(a => a.GetType() == typeof(DevOnlyCmdAttribute))
                                where isHito || !command.Attributes.Any(a => a.GetType() == typeof(HitoOnlyCmdAttribute))
                                orderby command.Name
                                select Helpers.Pad(command.Name, 20) + command.Summary).ToArray();
            var firstThird = commandDescs.Length / 3;
            var secondThird = firstThird * 2;
            var firstMsg = "Thank you for consulting me to receive help~" +
                "```" + string.Join("\n", commandDescs[0..firstThird]) + "```";
            var secondMsg = "```" + string.Join("\n", commandDescs[firstThird..secondThird]) + "```";
            var thirdMsg = "```" + string.Join("\n", commandDescs[secondThird..^1]) + "```";

            //get dms
            var dms = await Context.User.GetOrCreateDMChannelAsync();

            //send the help to DMs
            await dms.SendMessageAsync(firstMsg);
            await dms.SendMessageAsync(secondMsg);
            await dms.SendMessageAsync(thirdMsg);


        }


        [Command("timecmd", RunMode = RunMode.Async)]
        [Alias("time")]
        [DevOnlyCmd]
        [Summary("Time how long a command takes to complete.")]
        public async Task TimeCmd([Remainder] string cmd)
        {

            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                await ReplyAsync("No u ;-;");
                return;
            }
            Stopwatch s = new Stopwatch();
            s.Start();
            var x = await Commands.ExecuteAsync(Context, cmd, Provider);
            s.Stop();
            if (x.IsSuccess)
                await Context.Channel.SendMessageAsync($"Took {s.Elapsed.TotalMilliseconds.ToString("f2")}ms to execute {cmd.Clean()}");
            else
                await Context.Channel.SendMessageAsync("Didn't work.");
        }

    }
}

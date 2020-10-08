using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace KawaiiBot2.Modules
{
    public class StaticImages : ModuleBase<SocketCommandContext>
    {
        [Command("doot", RunMode = RunMode.Async)]
        [HiddenCmd]
        [Summary("Doot doot")]
        public Task Doot()
        {
            return ReplyAsync("https://cdn.discordapp.com/attachments/763105251393536000/763781191526514708/doot.gif");
        }

        [Command("notwork", RunMode = RunMode.Async)]
        [Summary("That's not how it works you little shit")]
        public Task Notwork()
        {
            return ReplyAsync("https://cdn.discordapp.com/attachments/763105251393536000/763780718105985074/notwork.png");
        }

        [Command("woop", RunMode = RunMode.Async)]
        [Summary("Woop woop!")]
        public Task Woop()
        {
            return ReplyAsync("https://cdn.discordapp.com/attachments/763105251393536000/763783588805476352/woop.gif");
        }
    }
}

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
        public Task Doot()
        {
            return Context.Channel.SendFileAsync("Resources/images/doot.gif");
        }

        [Command("notwork", RunMode = RunMode.Async)]
        public Task Notwork()
        {
            return Context.Channel.SendFileAsync("Resources/images/notwork.png");
        }

        [Command("woop", RunMode = RunMode.Async)]
        public Task Woop()
        {
            return Context.Channel.SendFileAsync("Resources/images/woop.gif");
        }
    }
}

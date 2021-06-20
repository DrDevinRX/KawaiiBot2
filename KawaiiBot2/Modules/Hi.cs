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
    public class Hi : ModuleBase<SocketCommandContext>
    {

        [Command("hi")]
        [Alias("hey")]
        [Summary("Because Father Servo's always down.")]

        public Task HiCmd()
        {
            var win = new Random().Next(10) == 7;
            var rig = yesHey ^ alwaysHey;
            yesHey = false;
            win |= rig;
            return ReplyAsync(win ? "HEY" : "hi");
        }

        private volatile static bool yesHey;
        private volatile static bool alwaysHey;
        [Command("righi")]
        [Summary("Because some people need to win everything.")]
        [HiddenCmd]
        public Task RigHi()
        {
            yesHey = true;
            return ReplyAsync("HEY!");
        }

        [Command("permarighi")]
        [Alias("alwayshey", "permahey")]
        [Summary("There is no choice in the matter.")]
        [DevOnlyCmd]
        public Task PermaRigHi(bool rig = true)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("Such a mode does not exist.");
            }
            alwaysHey = rig;
            return ReplyAsync("HEYHEYHEYHEYHEY");
        }

        [Command("navi")]
        [Summary("Hey, Listen!")]
        [HiddenCmd]
        public Task Navi()
        {
            return ReplyAsync("HEY, listen!");
        }
    }
}

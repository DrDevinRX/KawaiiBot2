﻿using System;
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
        [Summary("Because Father Servo's always down.")]

        public Task HiCmd()
        {
            if (yesHey)
            {
                yesHey = false;
                return ReplyAsync("HEY");
            }
            return ReplyAsync(new Random().Next(10) == 7 ? "HEY" : "hi");
        }

        private volatile static bool yesHey;
        [Command("righi")]
        [Summary("Because some people need to win everything.")]
        [HiddenCmd]
        public Task RigHi()
        {
            var exeName = Helpers.GetName(Context.User);
            yesHey = true;
            return ReplyAsync("HEY!");
        }
    }
}

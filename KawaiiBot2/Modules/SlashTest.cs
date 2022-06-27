using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.IO;
using Newtonsoft.Json;
using System.Security;
using KawaiiBot2.JSONClasses;
using System.Security.Cryptography;
using Discord.Interactions;

namespace KawaiiBot2.Modules
{
    public class SlashTest : InteractionModuleBase//<SocketInteractionContext>
    {
        [SlashCommand("hi", "Because Father Servo's always down.")]
        public async Task HiCmd()
        {
            var win = new Random().Next(10) == 7;
            await RespondAsync(win ? "HEY" : "hi");
        }

        [SlashCommand("grün", "Grun facts for the autoscroller")]
        [Alias("grun", "grunfacts", "grünfacts")]
        public async Task GrunFacts()
            => await RespondAsync(Grun.GrunFacts());

    }
}

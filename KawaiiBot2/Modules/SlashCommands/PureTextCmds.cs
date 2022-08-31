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
using KawaiiBot2.Modules.Shared;

namespace KawaiiBot2.Modules.SlashCommands
{
    public class PureTextCmds : InteractionModuleBase//<SocketInteractionContext>
    {
        //hi is not actually a pure text command lol
        [SlashCommand("hi", "Because Father Servo's always down.")]
        public async Task HiCmd()
        {
            var win = new Random().Next(10) == 7;
            await RespondAsync(win ? "HEY" : "hi");
        }
        //actually starts from here

        [SlashCommand("grün", "Grun facts for the autoscroller")]
        public async Task GrunFacts()
            => await RespondAsync(PureText.GrunFacts());

        [SlashCommand("touchbutt", "Touch someone's butt. What a pervert!")]
        public async Task TouchButt()
            => await RespondAsync(PureText.TouchButt);

        [SlashCommand("wha", "what the...")]
        public async Task WHA()
            => await RespondAsync(PureText.WHA);

    }
}

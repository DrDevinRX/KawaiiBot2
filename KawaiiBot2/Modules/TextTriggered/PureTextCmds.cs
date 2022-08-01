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
using KawaiiBot2.Modules.Shared;


namespace KawaiiBot2.Modules.TextTriggered
{
    public class PureTextCmds : ModuleBase<SocketCommandContext>
    {
        [Command("grün")]
        [Summary("Grün facts for the autoscroller")]
        [Alias("grun", "grunfacts", "grünfacts", "grain")]
        public Task GrunFacts([Remainder] string s = null)
    => ReplyAsync(PureText.GrunFacts());

        [Command("paimon")]
        [HiddenCmd]
        [Summary("Paimon isn't emergency food!")]
        public Task Paimon([Remainder] string s = null)
        => ReplyAsync(PureText.Paimon);

        [Command("touchbutt")]
        [Summary("Touch someone's butt. What a pervert!")]
        [Alias("fondleposterior")]
        [HiddenCmd]
        public Task TouchButt([Remainder] string s = null)
        => ReplyAsync(PureText.TouchButt);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Diagnostics;
using KawaiiBot2.Services;
using Newtonsoft.Json.Schema;

namespace KawaiiBot2.Modules
{
    [RequireContext(ContextType.Guild, ErrorMessage = "W-why would you want a meme when you're alone...?")]
    public class Memes : ModuleBase<SocketCommandContext>
    {

        [Command("meme")]
        [Alias("captcha", "calling", "facts", "scroll", "supreme", "achievement", "challenge")]
        [Summary("Makes memes. Has a lot of aliases because code magic.")]
        public Task GetMeme([Remainder] string args = null)
        {
            var name = Context.Message.ToString().Substring(CommandHandlerService.Prefix.Length).Split(" ")[0].ToLower();

            if (name == "meme") return ReplyAsync("Need to use a proper meme!");
            if (args == null) return ReplyAsync($"Can't make a blank {name}!");
            args = args.Replace(" ", "%20").Clean();

            return ReplyAsync($"https://api.alexflipnote.dev/{name}?text={args}");
        }

        [Command("meme2")]
        [Alias("drake","didyoumean")]
        [Summary("Makes a meme with a top and bottom. Has a lot of aliases because code magic.")]
        public Task GetMeme2([Remainder] string args = null)
        {
            var name = Context.Message.ToString().Substring(CommandHandlerService.Prefix.Length).Split(" ")[0].ToLower();

            if (name == "meme2") return ReplyAsync("Need to use a proper meme");
            if (args == null) return ReplyAsync($"Can't make a blank {name}!");

            var args2 = args.Split("|").ToList().Select(s => s.Replace(" ", "%20").Clean()).ToArray();
            if (args2.Length == 1) return ReplyAsync("Need two things seperated by | !");
            if (args2.Length > 2) return ReplyAsync("T-Too many...");

            return ReplyAsync($"https://api.alexflipnote.dev/{name}?top={args2[0]}&bottom={args2[1]}");
        }
    }
}

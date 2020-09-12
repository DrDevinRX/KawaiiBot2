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
        [Summary("Make a member a meme!")]
        public Task MakeUserMeme(IGuildUser user, [Remainder] string text = null)
        {
            if (text == null) return ReplyAsync("Need some text! Seperate by |");
            var texts = text.Split("|").ToList().Select(s => Uri.EscapeDataString(s.Trim())).ToArray();
            if (texts.Length > 2) return ReplyAsync("Too much text??");
            var avaUrl = Uri.EscapeDataString(user.GetEffectiveAvatarUrl());
            var textPayload = texts[0];
            if (texts.Length == 2) textPayload += "/" + texts[1];
            return ReplyAsync($"https://memegen.link/custom/{textPayload}.jpg?alt={avaUrl}");
        }

        [Command("meme")]
        [Summary("Make you a meme!")]
        public Task MakeUserMeme([Remainder] string text = null)
        {
            if (text == null) return ReplyAsync("Need some text! Seperate by |");
            var texts = text.Split("|").ToList().Select(s => Uri.EscapeDataString(s.Trim())).ToArray();
            if (texts.Length > 2) return ReplyAsync("Too much text??");
            var user = Context.User as IGuildUser;
            var avaUrl = Uri.EscapeDataString(user.GetEffectiveAvatarUrl());
            var textPayload = texts[0];
            if (texts.Length == 2) textPayload += "/" + texts[1];
            return ReplyAsync($"https://memegen.link/custom/{textPayload}.jpg?alt={avaUrl}");
        }

        [Command("memegen")]
        [Alias("captcha", "calling", "facts", "scroll", "supreme", "achievement", "challenge")]
        [Summary("Makes memes. capcha, calling, facts, scroll, supreme, achievement, challenge. So many!")]
        public Task GetMeme([Remainder] string args = null)
        {
            var name = Context.Message.ToString().Substring(CommandHandlerService.Prefix.Length).Split(" ")[0].ToLower();

            if (name == "memegen") return ReplyAsync("Need to use a proper meme!");
            if (args == null) return ReplyAsync($"Can't make a blank {name}!");
            args = Uri.EscapeDataString(args);

            return ReplyAsync($"https://api.alexflipnote.dev/{name}?text={args}");
        }

        [Command("meme2")]
        [Alias("drake", "didyoumean")]
        [Summary("Makes a meme with a top and bottom. drake, didyoumean. Very code. Much magic.")]
        public Task GetMeme2([Remainder] string args = null)
        {
            var name = Context.Message.ToString().Substring(CommandHandlerService.Prefix.Length).Split(" ")[0].ToLower();

            if (name == "meme2") return ReplyAsync("Need to use a proper meme");
            if (args == null) return ReplyAsync($"Can't make a blank {name}!");

            var args2 = args.Split("|").ToList().Select(s => Uri.EscapeDataString(s)).ToArray();
            if (args2.Length == 1) return ReplyAsync("Need two things seperated by | !");
            if (args2.Length > 2) return ReplyAsync("T-Too many...");

            return ReplyAsync($"https://api.alexflipnote.dev/{name}?top={args2[0]}&bottom={args2[1]}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace KawaiiBot2.Modules
{
    public class StaticAnimeReactions : ModuleBase<SocketCommandContext>
    {
        readonly Dictionary<string, string[]> urlDictionary =
            JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText("Resources/StaticURLImages.json"));

        private Task StaticAnimeReactCommand(
            string noUserReply,
            string botUserReply,
            string sameUserReply,
            string nominalReply,
            string name,
            IGuildUser user,
            SocketCommandContext context)
        {
            if (user == null)
            {
                return ReplyAsync(noUserReply);
            }
            else if (user.Id == context.Client.CurrentUser.Id)
            {
                return ReplyAsync(botUserReply);
            }
            else if (user.Id == context.User.Id)
            {
                return ReplyAsync(sameUserReply);
            }
            var url = Helpers.ChooseRandom(urlDictionary[name]);
            return Context.Channel.SendMessageAsync("", false, Helpers.ImgStrEmbed(url, nominalReply));
        }


        private readonly string[] flowers = { "\uD83C\uDF37", "\uD83C\uDF3C", "\uD83C\uDF38", "\uD83C\uDF3A", "\uD83C\uDF3B", "\uD83C\uDF39" };
        [Command("flower", RunMode = RunMode.Async)]
        [Summary("Give someone a flower! 🌸")]
        [RequireContext(ContextType.Guild, ErrorMessage = "There's flowers all around, just look!")]
        public async Task Flower(IGuildUser user = null)
        {
            if (user == null)
            {
                await ReplyAsync("Why are you trying to give the floor a flower?");
                return;
            }
            else if (user.Id == Context.Client.CurrentUser.Id)
            {
                await ReplyAsync("*Awww* ❤");
                return;
            }
            else if (user.Id == Context.User.Id)
            {
                await ReplyAsync("You can't give yourself flowers!");
                return;
            }
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            await ReplyAsync($"**{mentionedUserName}**, you got a {Helpers.ChooseRandom(flowers)} from **{AuthorName}**\n" +
                "https://cdn.discordapp.com/attachments/763105251393536000/763783994877542440/flower.gif");
            return;

        }

        [Command("dab")]
        [Summary("Dab on haters")]
        public Task Dab()
        {
            var url = Helpers.ChooseRandom(urlDictionary["dab"]);
            var comment = Helpers.ChooseRandom(urlDictionary["dabComments"]);
            return Context.Channel.SendMessageAsync("", false, Helpers.ImgStrEmbed(url, comment));
        }

        [Command("lewd")]
        [Summary("How lewd!")]
        public Task Lewd()
        {
            return ReplyAsync(Helpers.ChooseRandom(urlDictionary["lewd"]));
        }

        [Command("wag")]
        [Summary("Wag your tail!")]
        public Task Wag()
        {
            return ReplyAsync(Helpers.ChooseRandom(urlDictionary["wag"]));
        }
    }
}

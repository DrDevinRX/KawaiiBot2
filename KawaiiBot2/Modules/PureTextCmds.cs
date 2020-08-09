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

namespace KawaiiBot2.Modules
{
    public class PureTextCmds : ModuleBase<SocketCommandContext>
    {

        private readonly string[] hearts = { "❤", "💛", "💚", "💙", "💜" };

        [Command("f")]
        [Alias("F")]
        [Summary("Press F to pay respects")]
        public Task F(IGuildUser user)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            var heart = Helpers.ChooseRandom(hearts);
            return ReplyAsync($"**{AuthorName}** has paid their respects for **{mentionedUserName}** {heart}");
        }

        [Command("f")]
        [Alias("F")]
        [Summary("Press F to pay respects")]
        public Task F(String s = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var heart = Helpers.ChooseRandom(hearts);
            if (s == null)
                return ReplyAsync($"**{AuthorName}** has paid their respects {heart}");
            return ReplyAsync($"**{AuthorName}** has paid their respects for **{s.Clean()}** {heart}");

        }

        private static readonly string[] CoinSides = { "Heads", "Tails" };

        [Command("flip")]
        [Alias("coin")]
        [Summary("Flip a coin!")]
        public Task Flip()
        {
            var chosen = Helpers.ChooseRandom(CoinSides);
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            return ReplyAsync($"**{AuthorName}** flipped a coin and got **{chosen}**!");
        }

        private static readonly string[] BootResponses =
    JsonConvert.DeserializeObject<string[]>(File.ReadAllText("Resources/BootResponses.json"));

        [Command("boot")]
        [Summary("Throw a boot at someone")]
        public Task Boot(IGuildUser user = null)
        {
            if (user == null)
                return ReplyAsync("Give me someone to throw a boot at >-<");
            if (user.Id == Context.User.Id)
                return ReplyAsync("You don the boots.");

            var quote = Helpers.ChooseRandom(BootResponses);


            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);

            return ReplyAsync(string.Format(quote, AuthorName, mentionedUserName));
        }


        [Command("beer")]
        [Summary("Give someone a beer!")]
        public Task Beer(IGuildUser user = null, string reason = null)
        {
            if (user == null || user.Id == Context.User.Id)
            {
                return ReplyAsync("Alone? Aww ;-; I'll share a beer with you");
            }
            else if (user.Id == Context.Client.CurrentUser.Id)
            {
                var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
                return ReplyAsync($"Thanks for the beer, **{AuthorName}**");
            }
            else
            {
                var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
                var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
                if (reason == null)
                {
                    return ReplyAsync($"**{mentionedUserName}**, you got a \uD83C\uDF7A from **{AuthorName}**\n\n");
                }
                else
                {
                    return ReplyAsync($"**{mentionedUserName}**, you got a \uD83C\uDF7A from **{AuthorName}**\n\n**Reason:** {reason.Clean()}\n");
                }
            }
        }

        [Command("cookie")]
        [Summary("Give someone a cookie \uD83C\uDF6A")]
        public Task Cookie(IGuildUser user = null, string reason = null)
        {
            if (user == null)
            {
                return ReplyAsync("Why are you trying to give a cookie to thin air?");
            }
            else if (user.Id == Context.User.Id)
            {
                return ReplyAsync("I see your cookie, and I want it...");
            }
            else if (user.Id == Context.Client.CurrentUser.Id)
            {
                var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
                return ReplyAsync($"Cookie! Thankies, {AuthorName}!!");
            }
            else
            {
                var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
                var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
                if (reason == null)
                {
                    return ReplyAsync($"**{mentionedUserName}**, you got a \uD83C\uDF6A from **{AuthorName}**\n(ﾉ◕ヮ◕)ﾉ*:･ﾟ✧ \uD83C\uDF6A");
                }
                else
                {
                    return ReplyAsync($"**{mentionedUserName}**, you got a \uD83C\uDF6A from **{AuthorName}**" +
                        $"\n\n**Reason:** {reason.Clean()}\n(ﾉ◕ヮ◕)ﾉ*:･ﾟ✧ \uD83C\uDF6A");
                }
            }
        }
    }
}

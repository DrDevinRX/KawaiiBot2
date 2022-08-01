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

namespace KawaiiBot2.Modules
{
    public class PureTextCmds : ModuleBase<SocketCommandContext>
    {
        private static volatile uint deathCount = 0;

        [Command("death")]
        [Alias("deathcount", "deaths", "youdied", "died")]
        [Summary("Count the streamer's deaths")]
        public Task DeathCount([Remainder] string s = null)
        {
            deathCount++;
            var pluralize = deathCount == 1 ? "" : "s";
            var number = deathCount == 0 ? "no" : deathCount.ToString();
            return ReplyAsync($"{number} death{pluralize}");
        }

        [Command("resetdeathcount")]
        [Alias("resetdeath", "resetdeaths")]
        [Summary("Reset death count.")]
        [DevOnlyCmd]
        public Task ResetDeathCount([Remainder] string s = null)
        {
            if (Helpers.devIDs.Contains(Context.User.Id))
            {
                deathCount = 0;
                return ReplyAsync("Reset death count.");
            }
            else
            {
                return ReplyAsync("No u ;-;");
            }
        }







            [Command("roll")]
        [Summary("Rolls a number in a given range")]
        public Task Roll(int upperBound = 10, int lowerBound = 0)
        {
            if (lowerBound == upperBound) return ReplyAsync($"There's no rolling here, there's only {upperBound}");
            if (lowerBound > upperBound) (lowerBound, upperBound) = (upperBound, lowerBound);

            Random rand = new Random();

            return ReplyAsync($"{Helpers.GetName(Context.User)} rolled {lowerBound}-{upperBound} and got **{rand.Next(lowerBound, upperBound + 1)}**");
        }

        private readonly string[] fruits ={"\uD83C\uDF4F", "\uD83C\uDF4E ", "\uD83C\uDF50", "\uD83C\uDF4A",
                                            "\uD83C\uDF4B", "\uD83C\uDF4C", "\uD83C\uDF49", "\uD83C\uDF47",
                                            "\uD83C\uDF53", "\uD83C\uDF48", "\uD83C\uDF52", "\uD83C\uDF51",
                                            "\uD83C\uDF4F", "\uD83C\uDF4E ", "\uD83C\uDF50", "\uD83C\uDF4A",
                                            "\uD83C\uDF4B", "\uD83C\uDF4C", "\uD83C\uDF49", "\uD83C\uDF47",
                                            "\uD83C\uDF53", "\uD83C\uDF48", "\uD83C\uDF52", "\uD83C\uDF51"};

        [Command("fruitsnacks")]
        [Alias("fruits", "fruit")]
        [Summary("Give someone a fruit")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Nom nom nom")]
        public Task FruitSnacks(IGuildUser user = null)
        {
            if (user == null) return ReplyAsync("*splat* the fruit lands on the floor.");
            if (user.Id == Context.User.Id) return ReplyAsync("But you already have it...?");
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            var fruit = Helpers.ChooseRandom(fruits);
            return ReplyAsync($"{mentionedUserName}, you got a {fruit} from {AuthorName}\n\n(ﾉ◕ヮ◕)ﾉ*:･ﾟ✧ {fruit}");
        }


        [Command("echo")]
        [Summary("Echo               echo              *echo*")]
        public Task Echo([Remainder] string str = null)
        {
            if (str == null)
                return ReplyAsync("Nothing will echo if you don't give me something to say");
            else
                return ReplyAsync($"**{Helpers.GetName(Context.User)}**: {str.Clean()}");
        }


        [Command("lovecalc")]
        [Alias("love", "❤")]
        [Summary("Calculate love levels between two members")]
        [RequireContext(ContextType.Guild, ErrorMessage = "We're not compatible at all!")]
        public Task LoveCalc(IGuildUser user1 = null, IGuildUser user2 = null)
        {
            if (user1 == null || user2 == null) return ReplyAsync("Mention two members, please");

            bool sameUserCalc = user1.Id == user2.Id;
            bool botCalc = user1.Id == Context.Client.CurrentUser.Id || user2.Id == Context.Client.CurrentUser.Id;
            bool authorCalc = user1.Id == Context.User.Id || user2.Id == Context.User.Id;

            if (botCalc)
                return ReplyAsync("B-but I don't have feelings...");
            else if (sameUserCalc && authorCalc)
                return ReplyAsync("Sorry to see you alone இ௰இ");
            else if (sameUserCalc)
                return ReplyAsync("I-I'm not sure that's possible...");

            var percent = new Random((user1.Id + user2.Id).GetHashCode()).NextDouble() * 100;

            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("❤ Love Calculator　❤")
                .WithDescription($"Love between {Helpers.CleanGuildUserDisplayName(user1)} and {Helpers.CleanGuildUserDisplayName(user2)} is at **{percent:f1}%**");

            return Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("pickle")]
        [Alias("pickly")]
        [Summary("o///o")]
        public Task Pickle(IGuildUser user = null)
        {
            user ??= (Context.User as IGuildUser);
            IUser baseUser = user ?? (Context.User as IUser);

            var name = Helpers.GetName(baseUser);

            var size = new Random(baseUser.Id.GetHashCode()).NextDouble() * 50 / 1.17;
            size *= baseUser.Id == 173529942431236096 ? -.1 : .9;

            if (baseUser.Id == Context.User.Id)
                return ReplyAsync($"**{name}**, your pickle size is **{size:f2}cm** \uD83C\uDF80");
            else
                return ReplyAsync($"**{name}'s** pickle size is **{size:f2}cm** \uD83C\uDF80");
        }

        [Command("setstatus")]
        [Summary("Sets playing text.")]
        [Alias("setplaying", "status")]
        [DevOnlyCmd]
        public Task SetStatus([Remainder] string str = null)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("You need to be a developer to use this command");
            }
            str ??= "Awooo <3";
            return Context.Client.SetGameAsync(str);
        }

        [Command("reverse")]
        [Summary("!poow ,ffuts esreveR")]
        public Task Reverse([Remainder] string str = null)
        {
            str ??= "?esrever ot gnihtemos epyt ebyaM";
            return ReplyAsync(string.Join("", str.Reverse()).Clean());
        }

        [Command("ratewaifu")]
        [Alias("rate", "waifu", "ratehusbando", "husbando")]
        [Summary("Rates your waifu. She's trash, of course.")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Can't rate users where there are none?")]
        public Task RateWaifu(IGuildUser user)
        {
            user ??= (Context.User as IGuildUser);

            bool selfRate = user.Id == Context.User.Id;
            bool hitoRate = user.Id == 173529942431236096L;
            bool botRate = user.Id == Context.Client.CurrentUser.Id;
            Random rn = new Random(user.GetHashCode());

            if (hitoRate && selfRate)
                return ReplyAsync("Yuuhi, I'd rate you a **100/100!**");
            else if (hitoRate)
                return ReplyAsync("I'd rate Yuuhi a **100/100!**");
            else if (botRate)
                return ReplyAsync("I'd rate me a **110/100!**");
            else if (selfRate)
                return ReplyAsync($"I'd rate you a **{rn.Next(90 - 1) + 11} / 100**");
            else
                return ReplyAsync($"I'd rate `{Helpers.CleanGuildUserDisplayName(user)}` a **{rn.Next(90 - 1) + 11} / 100**");
        }

        [Command("ratewaifu")]
        [Alias("rate", "waifu")]
        [Summary("Rates your waifu. She's trash, of course.")]
        public Task RateWaifu([Remainder] string str = null)
        {
            if (str == null)
                return ReplyAsync("You have to rate something..?");
            Random rn = new Random(str.GetHashCode());
            return ReplyAsync($"I'd rate `{str.Clean()}` a **{rn.Next(100 - 1) + 1} / 100**");
        }

        private readonly string[] hearts = { "❤", "💛", "💚", "💙", "💜" };

        [Command("f")]
        [Alias("F")]
        [Summary("Press F to pay respects")]
        [RequireContext(ContextType.Guild, ErrorMessage = "No users to pay respects for!")]
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
        public Task F([Remainder] string s = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var heart = Helpers.ChooseRandom(hearts);
            if (s == null)
                return ReplyAsync($"**{AuthorName}** has paid their respects {heart}");
            return ReplyAsync($"**{AuthorName}** has paid their respects for **{s.Clean()}** {heart}");

        }

        private static readonly string[] CoinSides = { "Heads", "Tails" };

        [Command("flip")]
        [Alias("coin", "coinflip")]
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
        [RequireContext(ContextType.Guild, ErrorMessage = "No throwing boots here, quiet time")]
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
        [RequireContext(ContextType.Guild, ErrorMessage = "Sharing a beer in secret, hehehe~")]
        public Task Beer(IGuildUser user = null, [Remainder] string reason = null)
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
        [RequireContext(ContextType.Guild, ErrorMessage = "I take your cookie :3")]
        public Task Cookie(IGuildUser user = null, [Remainder] string reason = null)
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

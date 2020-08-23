using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using KawaiiBot2.APIInterfacing.Interfaces;

namespace KawaiiBot2.Modules
{
    [RequireContext(ContextType.Guild, ErrorMessage = "Y-You're the baka for trying that!")]
    public class AnimeReactions : ModuleBase<SocketCommandContext>
    {

        private Task AnimeReactCommand(
            string noUserReply,
            string botUserReplyMsg,
            string botUserReplyImg,
            string sameUserReplyMsg,
            string sameUserReplyImg,
            string failureReply,
            string successReply,
            string endpointPlainName,
            IGuildUser user,
            SocketCommandContext context)
        {
            if (user == null)
            {
                return ReplyAsync(noUserReply);
            }
            else if (user.Id == context.Client.CurrentUser.Id)
            {
                return Task.Run(async () =>
                {
                    if (botUserReplyMsg != null)
                        await ReplyAsync(botUserReplyMsg);
                    if (botUserReplyImg != null)
                        await Context.Channel.SendFileAsync(botUserReplyImg);
                });
            }
            else if (user.Id == context.User.Id)
            {
                return Task.Run(async () =>
                {
                    if (sameUserReplyMsg != null)
                        await ReplyAsync(sameUserReplyMsg);
                    if (sameUserReplyImg != null)
                        await Context.Channel.SendFileAsync(sameUserReplyImg);
                });
            }
            return Task.Run(async () =>
            {
                (bool success, string url) = await NekosLifeInterface.TryGetEndpoint(endpointPlainName);
                if (!success)
                {
                    await ReplyAsync(failureReply);
                    return;
                }
                await Context.Channel.SendMessageAsync("", false, Helpers.ImgStrEmbed(url, successReply));
            });

        }

        [Command("baka", RunMode = RunMode.Async)]
        [Summary("Call someone a baka")]
        public Task Baka(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Who are you calling a baka...?",
                $"**{AuthorName}** how could you :'(",
                null,
                null,
                "Resources/images/selfbaka.jpg",
                "S-sorry, n-no bakas...",
                 $"**{AuthorName}**, called **{mentionedUserName}** a baka",
                 "baka",
                 user,
                 Context
                );
        }


        [Command("cuddle", RunMode = RunMode.Async)]
        [Summary("Cuddle someone :3")]
        public Task Cuddle(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Are you trying to cuddle the void...?",
                $"*Cuddles **{AuthorName}** back*",
                null,
                "Sorry to see you alone ;-;",
                null,
                "S-sorry, n-no cuddles...",
                $"**{mentionedUserName}**, you got a cuddle from **{AuthorName}**",
                "cuddle",
                user,
                Context
                );
        }


        [Command("hug", RunMode = RunMode.Async)]
        [Summary("Give someone a hug o////o")]
        public Task Hug(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Are you trying to hug thin air...?",
                $"*Hugs **{AuthorName}** back* ❤",
                null,
                "Sorry to see you alone...",
                "Resources/images/selfhug.gif",
                "I-I can't find any hug gifs... I'm sorry ;-;",
                $"**{mentionedUserName}**, you got a hug from **{AuthorName}**",
                "hug",
                user,
                Context
                );
        }


        [Command("kiss", RunMode = RunMode.Async)]
        [Summary("Kiss someone :3")]
        public Task Kiss(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Are you trying to kiss the void...?",
                $"*Kisses **{AuthorName}** back* ❤",
                null,
                "Sorry to see you alone ;-;",
                null,
                "S-sorry, no kisses...",
                $"**{mentionedUserName}**,you got a kiss from **{AuthorName}**",
                "kiss",
                user,
                Context
                );
        }

        [Command("pat", RunMode = RunMode.Async)]
        [Summary("Give someone a pat! o//o")]
        public Task Pat(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Are you trying to pat air...?",
                null,
                "Resources/images/kawaiipat.gif",
                "Don't be like that ;-;",
                "Resources/images/selfpat.gif",
                "Sorry, couldn't locate headpats...",
                $"**{mentionedUserName}**,you got a pat from **{AuthorName}**",
                "pat",
                user,
                Context
                );
        }

        [Command("poke", RunMode = RunMode.Async)]
        [Summary("Poke someone :3")]
        public Task Poke(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Are you trying to poke thin air...?",
                "Don't poke me ;-;",
                null,
                "You can't poke yourself... baka ;-;",
                null,
                "No poking!",
                $"**{mentionedUserName}**,you got a poke from **{AuthorName}**",
                "poke",
                user,
                Context
                );
        }


        [Command("slap", RunMode = RunMode.Async)]
        [Summary("Slap someone ~(>_<。)＼")]
        public Task Slap(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Are you trying to slap a ghost...?",
                $"**{AuthorName}** we can no longer be friends ;-;",
                null,
                null,
                "Resources/images/butwhy.gif",
                "No slaps!",
                $"**{mentionedUserName}**,you got a slap from **{AuthorName}**",
                "slap",
                user,
                Context
                );
        }

        [Command("tickle", RunMode = RunMode.Async)]
        [Summary("Tickle someone! (●'◡'●)")]
        public Task Tickle(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Who are you trying to tickle...?",
                "*giggles* ❤",
                null,
                null,
                "Resources/images/tickle.gif",
                "S-sorry, can't find tickles...",
                $"**{mentionedUserName}**, you got tickled by **{AuthorName}**",
                "tickle",
                user,
                Context
                );
        }


    }
}

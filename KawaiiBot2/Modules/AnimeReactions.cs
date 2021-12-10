using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using KawaiiBot2.APIInterfacing.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using KawaiiBot2.APIInterfacing.ResultSchemas;
using KawaiiBot2.APIInterfacing;

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
                        await ReplyAsync(botUserReplyImg);
                });
            }
            else if (user.Id == context.User.Id)
            {
                return Task.Run(async () =>
                {
                    if (sameUserReplyMsg != null)
                        await ReplyAsync(sameUserReplyMsg);
                    if (sameUserReplyImg != null)
                        await ReplyAsync(sameUserReplyImg);
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
                "https://cdn.discordapp.com/attachments/763105251393536000/763781592426217502/selfbaka.jpg",
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
                "https://cdn.discordapp.com/attachments/763105251393536000/763781731298836511/selfhug.gif",
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
                "https://cdn.discordapp.com/attachments/763105251393536000/763781987630448680/kawaiipat.gif",
                "Don't be like that ;-;",
                "https://cdn.discordapp.com/attachments/763105251393536000/763782020441702401/selfpat.gif",
                "Sorry, couldn't locate headpats...",
                $"**{mentionedUserName}**,you got a pat from **{AuthorName}**",
                "pat",
                user,
                Context
                );
        }

        [HiddenCmd]
        [Command("patbutnotlewd")]
        [Alias("patbutnotinalewdway")]
        [Summary("Why did the original awooo even think they were?")]
        public Task PatButNotLewd(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return AnimeReactCommand(
                "Are you trying to pat air...?",
                null,
                "https://cdn.nekos.life/v3/sfw/gif/pat/pat_061.gif",
                "Do you need a hug?",
                null,
                "Sorry, couldn't locate headpats",
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
                "https://cdn.discordapp.com/attachments/763105251393536000/763781337609666560/butwhy.gif",
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
                "https://cdn.discordapp.com/attachments/763105251393536000/763782373531582474/tickle.gif",
                "S-sorry, can't find tickles...",
                $"**{mentionedUserName}**, you got tickled by **{AuthorName}**",
                "tickle",
                user,
                Context
                );
        }


        private Task WaifuPicsAnimeReactCommand(
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
                        await ReplyAsync(botUserReplyImg);
                });
            }
            else if (user.Id == context.User.Id)
            {
                return Task.Run(async () =>
                {
                    if (sameUserReplyMsg != null)
                        await ReplyAsync(sameUserReplyMsg);
                    if (sameUserReplyImg != null)
                        await ReplyAsync(sameUserReplyImg);
                });
            }
            return Task.Run(async () =>
            {
                Request req = await Helpers.Client.SendRequest("https://api.waifu.pics/sfw/" + endpointPlainName);
                WaifuPicsRes res = JsonConvert.DeserializeObject<WaifuPicsRes>(req.Content);

                if (!req.Success)
                {
                    await ReplyAsync(failureReply);
                    return;
                }
                await Context.Channel.SendMessageAsync("", false, Helpers.ImgStrEmbed(res.Url, successReply));
            });

        }


        [Command("lick", RunMode = RunMode.Async)]
        [Summary("Lick someone o///o")]
        [RequireContext(ContextType.Guild, ErrorMessage = "I don't give consent!")]
        public Task Lick(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return WaifuPicsAnimeReactCommand(
                "Are you trying to lick air ?\n"
                            + "https://cdn.discordapp.com/attachments/763105251393536000/763782657078984724/airlick.gif",
                $"{AuthorName}... w-why do you lick me ;-;",
                null,
                null,
                "https://cdn.discordapp.com/attachments/763105251393536000/763782476886704148/selflick.gif",
                "Huh? I-Impossible?",
                $"**{mentionedUserName}**, was licked by **{AuthorName}**",
                "lick",
                user,
                Context
                );
        }

        [Command("highfive", RunMode = RunMode.Async)]
        [Summary("High-five someone! o/\\o")]
        [Alias("five")]
        [RequireContext(ContextType.Guild, ErrorMessage = "*Secretively high-fives*")]
        public Task Highfive(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return WaifuPicsAnimeReactCommand(
                "Are you trying to high-five atoms...?",
                $"*High-fives **{AuthorName}** back*",
                "https://cdn.weeb.sh/images/H1Lj9ymsW.gif",
                "*awkward...*",
                "https://cdn.discordapp.com/attachments/763105251393536000/763782956690964521/selffive.gif",
                "You high five the air...",
                $"**{mentionedUserName}**, you got a high-five from **{AuthorName}**",
                "highfive",
                user,
                Context
                );
        }

        [Command("bite")]
        [HiddenCmd]
        [Summary("Bite someone :3c      I'm secretly a vampire too! hush-hush.")]
        [RequireContext(ContextType.Guild, ErrorMessage = "*blood swap*! Vampire pals :3")]
        public Task Bite(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return WaifuPicsAnimeReactCommand(
                                "Are you trying to bite thin air...?",
                "Don't b-bite me ;-;",
                null,
                "W-Why would you want to bite yourself...?",
                null,
                "*dodge*",
                $"**{mentionedUserName}**, you were bitten by **{AuthorName}**",
                "bite",
                user,
                Context);
        }

        [Command("handhold")]
        [Alias("hold")]
        [Summary("Hold someone's hand. o///////o oh my how lewd")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Now that we're in private, hand hold is go~")]
        public Task Handhold(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return WaifuPicsAnimeReactCommand(
                                "Are you trying to hold the hand of a ghost...?",
                $"*Holds **{AuthorName}**'s hand back*",
                null,
                "Sorry to see you alone ;-;",
                null,
                "You feel alone...",
                $"**{mentionedUserName}**, **{AuthorName}** is holding your hand",
                "handhold",
                user,
                Context
                );
        }

        [Command("nom")]
        [Summary("Nom someone :3")]
        [RequireContext(ContextType.Guild, ErrorMessage = "nomnomnom")]
        public Task Nom(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return WaifuPicsAnimeReactCommand(
                                "Are you trying to nom the void...?",
                $"*Noms **{AuthorName}** back* ❤",
                null,
                "Sorry to see you alone ;-;",
                null,
                "Nomnomnomnomnom",
                $"**{mentionedUserName}**, you got a nom from **{AuthorName}**",
                "nom",
                user,
                Context
                );
        }

        private Task SingularPictureWaifuPics(string endpoint)
        {
            return Task.Run(async () =>
            {
                Request req = await Helpers.Client.SendRequest("https://api.waifu.pics/sfw/" + endpoint);
                WaifuPicsRes res = JsonConvert.DeserializeObject<WaifuPicsRes>(req.Content);

                if (!req.Success)
                {
                    await ReplyAsync("N-Nothing!");
                    return;
                }
                await Context.Channel.SendMessageAsync(res.Url);
            });
        }

        [Command("blush")]
        [Summary("Posts a girl blushing o////o")]
        public Task Blush()
        {
            return SingularPictureWaifuPics("blush");
        }

        [Command("cry")]
        [Summary("Posts a crying picture when you're sad ;-;")]
        public Task Cry()
        {
            return SingularPictureWaifuPics("cry");
        }

        [Command("dance")]
        [Summary("Posts a dancing image!")]
        public Task Dance()
        {
            return SingularPictureWaifuPics("dance");
        }

        [Command("smug")]
        [Summary("Posts a smug pic.")]
        public Task Smug()
        {
            return SingularPictureWaifuPics("smug");
        }
    }
}
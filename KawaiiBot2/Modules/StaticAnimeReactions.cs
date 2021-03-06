﻿using System;
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

        [Command("lick", RunMode = RunMode.Async)]
        [Summary("Lick someone o///o")]
        [RequireContext(ContextType.Guild, ErrorMessage = "I don't give consent!")]
        public async Task Lick(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            if (user == null)
            {
                await ReplyAsync("Are you trying to lick air ?\n"
                            + "https://cdn.discordapp.com/attachments/763105251393536000/763782657078984724/airlick.gif");
                return;
            }
            else if (user.Id == Context.Client.CurrentUser.Id)
            {
                await ReplyAsync($"{AuthorName}... w-why do you lick me ;-;");
                return;
            }
            else if (user.Id == Context.User.Id)
            {
                await ReplyAsync("https://cdn.discordapp.com/attachments/763105251393536000/763782476886704148/selflick.gif");
                return;
            }
            var url = Helpers.ChooseRandom(urlDictionary["lick"]);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            var nominalReply = $"**{mentionedUserName}**, was licked by **{AuthorName}**";
            await Context.Channel.SendMessageAsync("", false, Helpers.ImgStrEmbed(url, nominalReply));
        }

        [Command("highfive", RunMode = RunMode.Async)]
        [Summary("High-five someone! o/\\o")]
        [Alias("five")]
        [RequireContext(ContextType.Guild, ErrorMessage = "*Secretively high-fives*")]
        public async Task Highfive(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            if (user == null)
            {
                await ReplyAsync("Are you trying to high-five atoms...?");
                return;
            }
            else if (user.Id == Context.Client.CurrentUser.Id)
            {

                await Context.Channel.SendMessageAsync("", false,
                    Helpers.ImgStrEmbed("https://cdn.weeb.sh/images/H1Lj9ymsW.gif", $"*High-fives **{AuthorName}** back*"));
                return;
            }
            else if (user.Id == Context.User.Id)
            {
                await ReplyAsync("*awkward...*\n" +
                        "https://cdn.discordapp.com/attachments/763105251393536000/763782956690964521/selffive.gif");
                return;
            }
            var url = Helpers.ChooseRandom(urlDictionary["highfive"]);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            var nominalReply = $"**{mentionedUserName}**, you got a high-five from **{AuthorName}**";
            await Context.Channel.SendMessageAsync("", false, Helpers.ImgStrEmbed(url, nominalReply));
        }

        [Command("bite")]
        [HiddenCmd]
        [Summary("Bite someone :3c      I'm secretly a vampire too! hush-hush.")]
        [RequireContext(ContextType.Guild, ErrorMessage = "*blood swap*! Vampire pals :3")]
        public Task Bite(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return StaticAnimeReactCommand(
                "Are you trying to bite thin air...?",
                "Don't b-bite me ;-;",
                "W-Why would you want to bite yourself...?",
                $"**{mentionedUserName}**, you were bitten by **{AuthorName}**",
                "bite",
                user,
                Context
                );
        }


        [Command("handhold")]
        [Summary("Hold someone's hand. o///////o oh my how lewd")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Now that we're in private, hand hold is go~")]
        public Task Handhold(IGuildUser user = null)
        {
            var AuthorName = Helpers.CleanGuildUserDisplayName(Context.Message.Author as IGuildUser);
            var mentionedUserName = Helpers.CleanGuildUserDisplayName(user);
            return StaticAnimeReactCommand(
                "Are you trying to hold the hand of a ghost...?",
                $"*Holds **{AuthorName}**'s hand back*",
                "Sorry to see you alone ;-;",
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
            return StaticAnimeReactCommand(
                "Are you trying to nom the void...?",
                $"*Noms **{AuthorName}** back* ❤",
                "Sorry to see you alone ;-;",
                $"**{mentionedUserName}**, you got a nom from **{AuthorName}**",
                "nom",
                user,
                Context
                );

        }

        [Command("dab")]
        [Summary("Dab on haters")]
        public Task Dab()
        {
            var url = Helpers.ChooseRandom(urlDictionary["dab"]);
            var comment = Helpers.ChooseRandom(urlDictionary["dabComments"]);
            return Context.Channel.SendMessageAsync("", false, Helpers.ImgStrEmbed(url, comment));
        }

        [Command("blush")]
        [Summary("Posts a girl blushing o////o")]
        public Task Blush()
        {
            return ReplyAsync(Helpers.ChooseRandom(urlDictionary["blush"]));
        }

        [Command("cry")]
        [Summary("Posts a crying picture when you're sad ;-;")]
        public Task Cry()
        {
            return ReplyAsync(Helpers.ChooseRandom(urlDictionary["cry"]));
        }

        [Command("dance")]
        [Summary("Posts a dancing image!")]
        public Task Dance()
        {
            return ReplyAsync(Helpers.ChooseRandom(urlDictionary["dance"]));
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

﻿using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using KawaiiBot2.APIInterfacing;
using KawaiiBot2.APIInterfacing.ResultSchemas;
using System.Diagnostics;
using System;

namespace KawaiiBot2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping", RunMode = RunMode.Async)]
        [Summary("Pong!")]
        public async Task Ping()
        {
            var startTime = DateTime.Now;
            var msg = await ReplyAsync("Pong!");
            await msg.ModifyAsync((msg) => msg.Content = $"Pong! | Time: {(DateTime.Now - startTime).TotalMilliseconds}ms");
        }

        private static readonly string[] EightBallResponses =
            JsonConvert.DeserializeObject<string[]>(File.ReadAllText("Resources/EightBallResponses.json"));
        private static readonly string[] ChooseResponses =
            JsonConvert.DeserializeObject<string[]>(File.ReadAllText("Resources/ChooseResponses.json"));

        [Command("choose")]
        [Summary("Picks from a list of choices")]
        public Task Choose([Remainder] string choiceString = null)
        {
            string[] choices = choiceString?.Split('|');

            if (choices?.Length < 2 || choices == null)
            {
                return ReplyAsync("Separate your choices (at least 2) with \"|\"");
            }

            string choice = (rigChoose ? choices[0] : Helpers.ChooseRandom(choices)).Trim().Clean();
            if (rigChoose) rigChoose = false;
            string choiceRes = Helpers.ChooseRandom(ChooseResponses);

            return ReplyAsync($"I choose this: {string.Format(choiceRes, choice)}");
        }

        volatile static bool rigChoose;

        [Command("rigchoose")]
        [Summary("Make choose always choose the first option the next time")]
        [DevOnlyCmd]
        public Task RigChoose([Remainder] string s = null)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("W-what! I would never!");
            }
            rigChoose = true;
            return ReplyAsync("Done.");
        }

        [Command("8ball")]
        [Alias("eightball", "yball")]
        [Summary("Consult 8ball to receive an answer")]
        public Task EightBall([Remainder] string question = null)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return ReplyAsync("I need a question, please");
            }

            return ReplyAsync(
                            $"🎱 **Question: **{question.Trim().Clean()}\n" +
                            $"**Answer: ** {Helpers.ChooseRandom(EightBallResponses)}");
        }

        [Command("fact", RunMode = RunMode.Async)]
        [Alias("facts", "funfact", "funfacts")]
        [Summary("Fun fact, did you know...")]
        public async Task FunFacts()
        {
            Request req = await Helpers.Client.SendRequest("https://nekos.life/api/v2/fact");
            if (!req.Success)
            {
                await ReplyAsync("I-I couldn't find any facts to tell... I'm sorry ;-;");
                return;
            }
            var res = JsonConvert.DeserializeObject<NekosFactRes>(req.Content);
            await ReplyAsync($":books: **Fun fact**:\n{ res.Fact}");
        }

        [Command("ship")]
        [RequireContext(ContextType.Guild, ErrorMessage = "*throws you overboard*")]
        [Summary("Make a lovely ship <3")]
        public Task Ship(IGuildUser user1 = null, IGuildUser user2 = null)
        {
            if (user1 == null || user2 == null)
            {
                return ReplyAsync("You must select two members");
            }
            else if (user1.Id == user2.Id)
            {
                return ReplyAsync("You can't ship someone with themselves");
            }

            var name1 = Helpers.CleanGuildUserDisplayName(user1);
            var name2 = Helpers.CleanGuildUserDisplayName(user2);

            var shipName = name1[0..(name1.Length / 2)] + name2[(name2.Length / 2)..^0];

            var shipUrl = $"https://api.alexflipnote.dev/ship?user={user1.GetEffectiveAvatarUrl()}&user2={user2.GetEffectiveAvatarUrl()}";

            return ReplyAsync($"Lovely shipping~\nShip name:**{shipName}**\n{shipUrl}");

        }

        [Command("urban", RunMode = RunMode.Async)]
        [Summary("Gets urban dictionary definitions. +lewd ;-;")]
        public async Task Urban([Remainder] string word = null)
        {

            if (word == null)
            {
                await ReplyAsync("You need to look something up?");
                return;
            }

            Request req = await Helpers.Client.SendRequest($"http://api.urbandictionary.com/v0/define?term={Uri.EscapeDataString(word)}");

            if (!req.Success)
            {
                await ReplyAsync("*stands on tiptoes and reaches up, unable to reach the shelf where the dictionary is*");
                return;
            }

            var res = JsonConvert.DeserializeObject<UrbanRes>(req.Content);
            if (res.List.Length == 0)
            {
                await ReplyAsync("N-nothing to f-find...");
                return;
            }

            //use urban's default ordering, could change
            var bestDef = res.List[0];

            if (bestDef.Definition.Length > 1024)
            {
                await ReplyAsync($"The definition is too big for discord, look here for it! {bestDef.Permalink}");
                return;
            }

            var builder = new EmbedBuilder()
                .WithTitle(bestDef.Word.Clean())
                .WithDescription($"{bestDef.Author.Clean()}")
                .AddField("Definition", bestDef.Definition.Clean())
                .AddField("Example", bestDef.Example.Clean())
                .WithFooter($"👍{bestDef.ThumbsUp} | 👎 {bestDef.ThumbsDown}");
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}

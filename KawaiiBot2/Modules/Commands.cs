using Discord;
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

            string choice = Helpers.ChooseRandom(choices).Trim().Clean();
            string choiceRes = Helpers.ChooseRandom(ChooseResponses);

            return ReplyAsync($"I choose this: {string.Format(choiceRes, choice)}");
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

            return ReplyAsync($"Lovely shipping~\nShip name:**{shipName}**");

        }
    }
}

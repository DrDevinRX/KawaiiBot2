using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace KawaiiBot2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private static readonly string[] EightBallResponses =
            JsonConvert.DeserializeObject<string[]>(File.ReadAllText("Resources/EightBallResponses.json"));
        private static readonly string[] ChooseResponses =
            JsonConvert.DeserializeObject<string[]>(File.ReadAllText("Resources/ChooseResponses.json"));

        [Command("choose")]
        public Task Choose([Remainder] string choiceString = null)
        {
            string[] choices = choiceString?.Split('|');

            if (choices?.Length < 2 || choices == null)
            {
                return ReplyAsync("Separate your choices (at least 2) with \"|\"");
            }

            string choice = Helpers.ChooseRandom(choices).Trim().Clean();
            string choiceRes = Helpers.ChooseRandom(ChooseResponses);

            return ReplyAsync($"I choose this: {string.Format(choiceRes,choice)}");
        }

        [Command("8ball")]
        [Alias("eightball")]
        public Task EightBall([Remainder] string question = null)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return ReplyAsync("I need a question, please");
            }

            return ReplyAsync(
                            $"🎱 **Question: **{question.Trim()}\n" +
                            $"**Answer: ** {Helpers.ChooseRandom(EightBallResponses)}");
        }
    }
}

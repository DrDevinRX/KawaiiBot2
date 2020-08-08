using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KawaiiBot2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private static readonly string[] SlotIcons = { "🍎", "🍊", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒" };
        private static readonly string[] EightBallResponses = 
        {   "Yes", "No", "Take a wild guess...",
            "Very doubtful", "Sure", "Without a doubt",
            "senpai, pls no ;-;", "Most likely", "Might be possible",
            "You'll be the judge", "no... (╯°□°）╯︵ ┻━┻", "no... baka"
        };

        [Command("hi")]
        public Task Hi()
        {
            return ReplyAsync($"Hi, {Context.Message.Author.Mention}");
        }

        [Command("slots")]
        public Task Slots()
        {
            string[] finalSlots = new string[3];
            string winMessage;
            Dictionary<string, int> occurrenceCount = new Dictionary<string, int>();

            for (int i = 0; i < 3; i++)
            {
                string slot = Helper.Helper.ChooseRandomItem(SlotIcons);
                finalSlots[i] = slot;
                // Add to the dictionary for easy occurrence counting
                if (occurrenceCount.ContainsKey(slot))
                {
                    occurrenceCount[slot]++;
                }
                else
                {
                    occurrenceCount[slot] = 1;
                }
            }

            // If we have more than one key, we didn't win
            if (occurrenceCount.Count > 1)
            {
                winMessage = (occurrenceCount.Count > 2) ? "and lost..." : $"and almost won ({occurrenceCount.Count}/{finalSlots.Length})";
            }
            else
            {
                winMessage = $"and won! \uD83C\uDF89";
            }

            return ReplyAsync(
                            $"**{Helper.Helper.GuildUserNameOrNickName(Context.Message.Author as IGuildUser)}** rolled the slots...\n" +
                            $"**[ {string.Join(" ", finalSlots)} ]**\n" +
                            $"{winMessage}");
        }

        // TODO: Everything
        [Command("about")]
        public Task About()
        {
            return Task.CompletedTask;
        }

        // TODO: Embed and formatting
        [Command("joinedat")]
        public Task JoinedAt(IGuildUser user = null)
        {
            // TODO
            EmbedBuilder embedBuilder = new EmbedBuilder();

            var guildUser = user ?? (Context.Message.Author as IGuildUser);
            return ReplyAsync($"**{Helper.Helper.GuildUserNameOrNickName(guildUser)}** joined on {guildUser.JoinedAt}");
        }

        [Command("choose")]
        public Task Choose([Remainder]string choiceString = null)
        {
            string[] choices = choiceString?.Split('|');

            if (choices?.Length < 2 || choices == null)
            {
                return ReplyAsync("Separate your choices (at least 2) with \"|\"");
            }

            return ReplyAsync($"I choose this: **{Helper.Helper.ChooseRandomItem(choices).Trim()}**");
        }

        [Command("8ball")]
        public Task EightBall([Remainder]string question = null)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return ReplyAsync("I need a question, please");
            }

            return ReplyAsync(
                            $"🎱 **Question: **{question.Trim()}\n" +
                            $"**Answer: ** {Helper.Helper.ChooseRandomItem(EightBallResponses)}");
        }
    }
}

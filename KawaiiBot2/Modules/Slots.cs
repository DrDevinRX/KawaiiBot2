using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace KawaiiBot2.Modules
{
    public class Slots : ModuleBase<SocketCommandContext>
    {

        private static readonly string[] SlotIcons = { "🍎", "🍊", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒" };

        [Command("slots")]
        [Summary("Roll the slot machine. may rngesus guide your path.")]
        public Task SlotsCmd()
        {
            string[] finalSlots = Enumerable.Range(0, 3).Select(i => Helpers.ChooseRandom(SlotIcons)).ToArray();


            string winMessage = "and lost....";

            if (finalSlots[0] == finalSlots[1] && finalSlots[1] == finalSlots[2])
                winMessage = "and won! \uD83C\uDF89";
            if (finalSlots[0] == finalSlots[1] || finalSlots[1] == finalSlots[2] || finalSlots[0] == finalSlots[2])
                winMessage = "and almost won (2/3)";

            var guildUser = Context.Message.Author as IGuildUser;

            return ReplyAsync(
                            $"**{Helpers.CleanGuildUserDisplayName(guildUser)}** rolled the slots...\n" +
                            $"**[ {string.Join(" ", finalSlots)} ]**\n" +
                            $"{winMessage}");
        }

    }
}

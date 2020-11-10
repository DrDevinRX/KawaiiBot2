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
        [Command("aprilfruits")]
        [Summary("Slots, but how many fruits there are are random. Because more RNG is better, right?")]
        [HiddenCmd]
        public Task AprilFruits()
        {
            var rand = new Random();
            int n = rand.Next(3, 7);
            int fromN = rand.Next(4, 14);
            var slotsicons = ((string[])SlotIcons.Clone()).OrderBy(x => rand.Next()).Take(fromN);

            string[] finalSlots = Enumerable.Range(0, n).Select(i => Helpers.ChooseRandom(slotsicons)).ToArray();
            string winMessage = "and lost....";

            if (finalSlots.All(s => s == finalSlots[0]))
                winMessage = "and won! \uD83C\uDF89";
            else if (finalSlots.Count(s => s == finalSlots[0]) == n - 1 || finalSlots.Count(s => s == finalSlots[1]) == n - 1)
                winMessage = $"and almost won ({n - 1}/{n})";



            return ReplyAsync(
                            $"**{Helpers.GetName(Context.User)}** rolled the slots...\n" +
                            $"**[ {string.Join(" ", finalSlots)} ]**\n" +
                            $"{winMessage}");
        }


        private static readonly string[] SlotIcons = { "🍎", "🍊", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒", "🍌", "🍈", "🥭", "🥝", "🍍", "🥥", "🍏", "🍑", "🍪" };
        private static readonly string[] MemeRigAllows = { "🥔", "⚗\uFE0F", "🩸", "🚮", "💧", "🔥", "☄\uFE0F", "🎏", "🎐", "🥌", "🔮", "🎮", "🎰", "🎲", "♟\uFE0F", "🀄", "🎨", "💎", "💍", "🎼" };

        [Command("slots")]
        [Summary("Roll the slot machine. may rngesus guide your path.")]
        public Task SlotsCmd(int n = 3)
        {
            if (n < 2 || n > 100)
                return ReplyAsync("Nope. No. Nope. No. No can do.");


            string[] finalSlots = Enumerable.Range(0, n).Select(i => Helpers.ChooseRandom(SlotIcons)).ToArray();
            if (rigged && (!riggedUserID.HasValue || riggedUserID == Context.User.Id))
            {
                if (riggedTo.Length == 1)
                {
                    var tmp = riggedTo[0];
                    riggedTo = new string[n];
                    for (int i = 0; i < n; i++) riggedTo[i] = tmp;
                }
                finalSlots = riggedTo;
                n = riggedTo.Length;
                rigged = false;
            }



            string winMessage = "and lost....";

            if (finalSlots.All(s => s == finalSlots[0]))
                winMessage = "and won! \uD83C\uDF89";
            else if (finalSlots.Count(s => s == finalSlots[0]) == n - 1 || finalSlots.Count(s => s == finalSlots[1]) == n - 1)
                winMessage = $"and almost won ({n - 1}/{n})";



            return ReplyAsync(
                            $"**{Helpers.GetName(Context.User)}** rolled the slots...\n" +
                            $"**[ {string.Join(" ", finalSlots)} ]**\n" +
                            $"{winMessage}");
        }

        private volatile static bool rigged = false;
        private static ulong? riggedUserID = null;
        private volatile static string[] riggedTo;

        private void Rig(string[] RiggedTo, ulong? RiggedUserID)
        {
            rigged = true;
            riggedTo = RiggedTo;
            riggedUserID = RiggedUserID;
        }

        private bool okRig(string emoji) => SlotIcons.Contains(emoji) || MemeRigAllows.Contains(emoji);

        private Task RigCommon(string rigTo, ulong? userID = null)
        {
            rigTo = rigTo.Replace(" ", "").Replace(",", "");
            if (rigTo.Length == 2 && okRig(rigTo))
            {
                Rig(new string[] { rigTo }, userID);
                return ReplyAsync($"*Bumps the slots* Feels {rigTo}.");
            }
            else
            {
                var tmpRig = Enumerable.Range(0, rigTo.Length / 2).Select(i => rigTo.Substring(2 * i, 2)).Where(c => okRig(c)).ToArray();
                if (tmpRig.Length < 2)
                    return ReplyAsync("Huh?");
                if (tmpRig.Length > 100)
                    return ReplyAsync("You've gone batty.");
                Rig(tmpRig, userID);
                return ReplyAsync($"*Bumps the slots* Feels {string.Join(" ", tmpRig)}.");

            }
        }

        [Command("rigslots")]
        [Summary("Rig the slots. Well, it does nothing.")]
        [DevOnlyCmd]
        public Task RigSlots()
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("W-what! I would never!");
            }
            RigCommon(Helpers.ChooseRandom(SlotIcons));
            return ReplyAsync("(•̀ᴗ•́)و ̑̑  Got it.");
        }


        [Command("rigslots")]
        [Summary("Rig the slots. The next time this person does it.")]
        [DevOnlyCmd]
        public Task RigSlots(ulong UserID, [Remainder] string rigTo)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("W-what! I would never!");
            }

            return RigCommon(rigTo, UserID);
        }


        [Command("rigslots")]
        [Summary("Rig the slots. The next time this person does it.")]
        [DevOnlyCmd]
        public Task RigSlots(IGuildUser User, [Remainder] string rigTo)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("W-what! I would never!");
            }

            return RigCommon(rigTo, User.Id);
        }


        [Command("rigslots")]
        [Summary("Rig the slots. Just for the next person.")]
        [DevOnlyCmd]
        public Task RigSlots([Remainder] string rigTo)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("W-what! I would never!");
            }

            return RigCommon(rigTo);
        }

        [Command("rigmemes")]
        [Summary("Extra emojis that can be used in rigged slots (only)")]
        [DevOnlyCmd]
        public Task RigMemes([Remainder] string s = null)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return new Task(() => { });
            }
            return ReplyAsync(string.Join(", ", MemeRigAllows));
        }

    }
}

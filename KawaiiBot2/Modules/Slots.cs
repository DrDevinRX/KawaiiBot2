using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using NeoSmart.Unicode;

namespace KawaiiBot2.Modules
{
    public class Slots : ModuleBase<SocketCommandContext>
    {
        private class SlotsUserData
        {
            public SlotsUserData() : this(0)
            { }
            public SlotsUserData(int Take)
            {
                takeThisMany = Take;
                longestStreak = 0;
                longestStreakIcon = "";
            }
            public volatile int takeThisMany;
            public volatile bool suppressed;
            public volatile string[] riggedTo;
            public int longestStreak;
            public string longestStreakIcon;
            public int winsCount;
            public static SlotsUserData empty => new SlotsUserData();
        }
        [Command("aprilfruits")]
        [Summary("Slots, but how many fruits there are are random. Because more RNG is better, right?")]
        [HiddenCmd]
        public Task AprilFruits()
        {

            int n = rand.Next(3, 7);
            int fromN = rand.Next(4, 14);
            var slotsicons = ((string[])SlotIcons.Clone()).OrderBy(x => rand.Next()).Take(fromN).ToArray();

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

        private static Random rand = new Random();

        private static readonly string[] SlotIcons = { "🍎", "🍊", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒", "🍌", "🍈", "🥭", "🥝", "🍍", "🥥", "🍏", "🍑", "🍪",
            "🥮", "🍡", "🍠","🍩","🍨","🎂", "🍭","🍫","🍯","🍵"};
        private static readonly string[] MemeRigAllows = { "🥔", "⚗\uFE0F", "🩸", "🚮", "💧", "🔥", "☄\uFE0F", "🎏", "🎐", "🥌", "🔮", "🎮", "🎰", "🎲",
            "♟\uFE0F", "🀄", "🎨", "💎", "💍", "🎼" };
        private static SlotsUserData global = new SlotsUserData(13);
        private static ConcurrentDictionary<ulong, SlotsUserData> userData = new ConcurrentDictionary<ulong, SlotsUserData>();

        [Command("slots", RunMode = RunMode.Async)]
        [Summary("Roll the slot machine. may rngesus guide your path.")]
        public Task SlotsCmd(int n = 3)
        {
            if (n < 2 || n > 121)
                return ReplyAsync("Nope. No. Nope. No. No can do.");

            var usersData = userData.GetOrAdd(Context.User.Id, SlotsUserData.empty);
            int iconsAmt = global.takeThisMany - (n >= 25 ? 11 : 0);
            iconsAmt += usersData.takeThisMany;
            if (iconsAmt < 1) iconsAmt = 1;
            if (iconsAmt > SlotIcons.Length) iconsAmt = SlotIcons.Length;

            var slotsicons = ((string[])SlotIcons.Clone()).OrderBy(x => rand.Next()).Take(iconsAmt).ToArray();
            string[] finalSlots = Enumerable.Range(0, n).Select(i => Helpers.ChooseRandom(slotsicons)).ToArray();

            //rig slots
            if (usersData.riggedTo != null || global.riggedTo != null)
            {
                //choose which one we act on, with user specific data taking preference
                var thisUserData = usersData.riggedTo != null ? usersData : global;
                if (thisUserData.riggedTo.Length == 1)
                {
                    var tmp = thisUserData.riggedTo[0];
                    thisUserData.riggedTo = new string[n];
                    for (int i = 0; i < n; i++) thisUserData.riggedTo[i] = tmp;
                }
                finalSlots = thisUserData.riggedTo;
                n = thisUserData.riggedTo.Length;
                thisUserData.riggedTo = null;
            }

            //Suppress slots wins
            if (usersData.suppressed || global.suppressed)
            {

                if (slotsicons.Length > 1)
                {
                    var replace = Helpers.ChooseTwoNoReplace(slotsicons);
                    finalSlots[^1] = replace.Item1;
                    finalSlots[^2] = replace.Item2;
                }
                else
                {
                    var replace = Helpers.ChooseTwoNoReplace(SlotIcons);
                    var notSoReplace = new string[] { replace.Item1, replace.Item2 };
                    finalSlots = Enumerable.Range(0, n).Select(i => Helpers.ChooseRandom(notSoReplace)).ToArray();
                    finalSlots[^1] = replace.Item1;
                    finalSlots[^2] = replace.Item2;
                }
            }

            bool win = finalSlots.All(s => s == finalSlots[0]);

            string winMessage = "and lost....";

            if (win)
                winMessage = "and won! \uD83C\uDF89";
            else if (finalSlots.Count(s => s == finalSlots[0]) == n - 1 || finalSlots.Count(s => s == finalSlots[1]) == n - 1)
                winMessage = $"and almost won ({n - 1}/{n})";

            if (win)
            {
                usersData.winsCount++;
                global.winsCount++;
            }

            //streak detection
            if (n >= 25 && iconsAmt < 5 && iconsAmt > 1)
            {
                (string, int) maxStreak = ("", -1);
                (string, int) currentStreak = ("", 0);
                foreach (string s in finalSlots)
                {
                    if (currentStreak.Item1 == s)
                    {
                        currentStreak.Item2++;
                    }
                    else
                    {
                        maxStreak = currentStreak.Item2 > maxStreak.Item2 ? currentStreak : maxStreak;
                        currentStreak = (s, 1);
                    }
                }
                //haha
                maxStreak = currentStreak.Item2 > maxStreak.Item2 ? currentStreak : maxStreak;
                if (maxStreak.Item2 >= 6)
                    winMessage += $"\nWith a {maxStreak.Item1} streak of {maxStreak.Item2}";

                if (maxStreak.Item2 >= usersData.longestStreak)
                {
                    usersData.longestStreak = maxStreak.Item2;
                    usersData.longestStreakIcon = maxStreak.Item1;
                }
                if (maxStreak.Item2 >= global.longestStreak)
                {
                    global.longestStreak = maxStreak.Item2;
                    global.longestStreakIcon = maxStreak.Item1;
                }
            }

            return ReplyAsync(
                            $"**{Helpers.GetName(Context.User)}** rolled the slots...\n" +
                            $"**[ {string.Join(" ", finalSlots)} ]**\n" +
                            $"{winMessage}");
        }

        [Command("leaderboard", RunMode = RunMode.Async)]
        [Alias("slotsboard", "board", "winners")]
        [Summary("LINQ makes everything easy, whaddaya mean this should be hard?")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Requires a guild because users")]
        public Task Leaderboard()
        {
            var shortlist = from pair in userData
                            where pair.Value.winsCount > 0
                            orderby -pair.Value.winsCount
                            select pair;
            if (shortlist.Count() == 0)
            {
                return ReplyAsync("Noone has won slots yet! So win them to be here!");
            }
            var finalList = from pair in shortlist
                            select $"{Helpers.GetName(Context.Guild.GetUser(pair.Key))}, {pair.Value.winsCount} win{(pair.Value.winsCount > 1 ? "s" : "")} " +
                                $"({pair.Value.winsCount / (double)global.winsCount * 100:f2}% of total)";

            return ReplyAsync($"Global Slots Leaderboard\n--------------------\n{string.Join("\n", finalList)}");
        }

        [Command("streakinfo")]
        [Alias("streak")]
        [Summary("Gets info about the longest streaks in slots")]
        public Task GetStreakInfo()
        {
            var usersData = userData.GetOrAdd(Context.User.Id, SlotsUserData.empty);
            var userStreakInfo = usersData.longestStreak > 0 ? $"Your longest streak was {usersData.longestStreak} with {usersData.longestStreakIcon}" :
                "You have no streaks.";
            var globalStreakInfo = global.longestStreak > 0 ? $"The longest streak globally was {global.longestStreak} with {global.longestStreakIcon}" :
                "There are no streaks globally.";
            return ReplyAsync($"{userStreakInfo}\n{globalStreakInfo}");
        }

        private bool okRig(string emoji) => SlotIcons.Contains(emoji) || MemeRigAllows.Contains(emoji);

        private void Rig(string[] RiggedTo, ulong? RiggedUserID)
        {
            SlotsUserData data;
            if (RiggedUserID == null)
            {
                data = global;
            }
            else
            {
                data = userData.GetOrAdd(RiggedUserID.Value, new SlotsUserData());
            }
            data.riggedTo = RiggedTo;
        }

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

        [Command("suppressslotswins")]
        [Alias("suppressslots", "noslotswins", "suppress")]
        [Summary("Supresses slots winning. Easy. Overrides rigging slots.")]
        [DevOnlyCmd]
        public Task SuppressSlotsWins(bool suppress = true)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return new Task(() => { });
            }
            global.suppressed = suppress;
            return ReplyAsync(suppress ? "Slots wins are now suppressed." : "Slots wins are no longer suppressed.");
        }

        [Command("setdifficulty")]
        [Alias("difficulty", "slotsdifficulty", "setglobaldifficulty", "globaldifficulty", "global", "globalslots")]
        [Summary("Sets the number of icons to use for slots globally.")]
        [DevOnlyCmd]
        public Task SetGlobalDifficulty(int n = -659838444)//hacky way to make sure noone ever finds the "print" value by accident
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
                return ReplyAsync("Maybe try +riskydice?");
            if (n == -659838444) return ReplyAsync($"Current global number of icons for slots is {global.takeThisMany}");
            if (n < 1 || n > SlotIcons.Length)
                return ReplyAsync("You're crazy. No.");
            global.takeThisMany = n;
            return ReplyAsync($"Global number of icons for slots set to {n}.");
        }

        [Command("riskydice")]
        [Alias("dice", "riskydice?")]
        [Summary("RiskyDice. Increased chances to win slots or be blocked from winning until the bot is restarted.")]
        public Task RiskyDice()
        {
            bool die = rand.Next(6) == 0;
            var data = userData.GetOrAdd(Context.User.Id, new SlotsUserData());
            if (die)
            {
                data.suppressed = true;
                data.takeThisMany = 0;
                return ReplyAsync(":blue_square:");
            }
            else
            {
                data.takeThisMany--;
                if (data.takeThisMany <= -20)
                    data.suppressed = false;
                return ReplyAsync(":black_large_square:");
            }
        }

        [Command("riskyslots")]
        [Summary("Risky dice, multiple times.")]
        public Task RiskySlots(int n = 3)
        {
            if (n < 1)
                return ReplyAsync("Not very ambitious, are we?");
            if (n == 1)
                return ReplyAsync("Maybe try +riskydice?");
            if (n == 20 && global.takeThisMany < 21)
                return ReplyAsync("一発で死ぬ。");
            if (n > 9000)
                return ReplyAsync("WHY IS IT OVER 9000!???????");
            if (n > SlotIcons.Length)
                return ReplyAsync("??? Are you serious???");
            if (n > global.takeThisMany - 1)
                return ReplyAsync("Too over the top!!");
            int[] riskySlots = Enumerable.Range(0, n).Select(i => rand.Next(6)).ToArray();
            var data = userData.GetOrAdd(Context.User.Id, new SlotsUserData());
            foreach (var i in riskySlots)
            {
                if (i == 0)
                {
                    data.suppressed = true;
                    data.takeThisMany = 0;
                }
                else
                    data.takeThisMany--;
            }
            if (data.takeThisMany <= -20)
                data.suppressed = false;
            return ReplyAsync(String.Join(" ", riskySlots.Select(i => i == 0 ? ":blue_square:" : ":black_large_square:")));
        }
    }
}

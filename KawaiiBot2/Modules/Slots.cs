﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using KawaiiBot2.JSONClasses;
using KawaiiBot2.Services;
using MathNet.Numerics.Distributions;

namespace KawaiiBot2.Modules
{
    public class SlotsUserData
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
        public static SlotsUserData Empty => new();
    }
    public class Slots : ModuleBase<SocketCommandContext>
    {
        public volatile static int totalIconsRolled;

        public volatile static int leftDoubles;
        public volatile static int rightDoubles;
        public volatile static int sidesDoubles;

        [Command("totalrolls")]
        [Alias("totalslots", "totalslotrolls")]
        [Summary("The total amount of slot icons rolled. Way too high.")]
        public Task TotalRolls()
        {
            return ReplyAsync($"A grand total of {totalIconsRolled} slot icons have been rolled. You fools.");
        }

        [Command("sidesrolls")]
        [Alias("sideroll", "siderolls", "rollsides", "rollside", "sideslots", "slotsides")]
        [Summary("Hypothesis: 2/3s in 3-slots tend not to be the two sides.")]
        [HiddenCmd]
        public Task SidesRolls()
        {
            //Binomial hypothesis test (from https://en.wikipedia.org/wiki/Binomial_test)
            var n = leftDoubles + rightDoubles + sidesDoubles;
            var expectedRatio = 1 / 3.0;
            Binomial b = new Binomial(expectedRatio, n);
            Binomial rb = new Binomial(1 - expectedRatio, n);
            var x = sidesDoubles;
            var p = b.CumulativeDistribution(x);
            var rp = rb.CumulativeDistribution(n - sidesDoubles);//can be used for the other hypothesis
            var pl = b.CumulativeDistribution(leftDoubles);
            var rj = p < .05;
            return ReplyAsync($"**Roll Sides**\nTotal: {n}\nLeft: {leftDoubles}\nRight: {rightDoubles}\nSides: {sidesDoubles}\n" +
                $"p(sides≤{sidesDoubles})={p:f2}\nBecause p {(rj ? "is" : "is not")} less than .05, we {(rj ? "reject" : "do not reject")} the null hypothesis." +
                $"\np(left≤{leftDoubles})={pl:f2}");
        }

        [Command("clearwins")]
        [DevOnlyCmd]
        [Summary("Remove everyone's wins. They were all +rigslots anyways.")]
        public Task ClearWins()
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("You ain't got the *authority* to do that.");
            }
            global.winsCount = 0;
            foreach (var pair in userData)
            {
                pair.Value.winsCount = 0;
            }
            return ReplyAsync("Master! I have cleaned out the filth!");
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

        internal static readonly Random rand = new();

        internal static readonly string[] SlotIcons = { "🥮", "🥥" , "🍎", "🍊", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒", "🍌", "🍈", "🥭", "🥝", "🍍", "🥥", "🍏", "🍑", "🍪",
                                                     "🍩","🍨","🎂", "🍭", "🍫", "🍯", "🫐", "🥐", "🧆", "🥞", "🥠", "🍘", "🥧", "🧋", "🥟"};
        internal static readonly string[] MemeRigAllows = { "🥔", "⚗\uFE0F", "🩸", "🚮", "💧", "🔥", "☄\uFE0F", "🎏", "🎐", "🥌", "🔮", "🎮", "🎰", "🎲",
            "♟\uFE0F", "🀄", "🎨", "💎", "💍", "🎼","🍕","🍝" ,"🍢", "🥯", "🍼", "🎯", "🌏", "🌍", "🌎", "🎳", "🥏", "🥌", "🌵", "🌴", "🦠", "🦂", "🧱",
            "🎈","💡","💴", "💵", "💶", "💷", "💳", "⚙\uFE0F", "🧬", "🔬", "🔭", "🛢", "🪐", "🌊", "🫀", "🛰", "🪁",/*tmp*/"🥮"};
        internal static SlotsUserData global = new(13);
        internal static readonly ConcurrentDictionary<ulong, SlotsUserData> userData = new();


        //TODO: allow DetermineIcons to use MemeRigs and ALL of the extra icons as sources
        [Command("slotsv2")]
        [Alias("slots", "sloots")]
        [Summary("Roll the slot machine. may rngesus guide your path. Better code.")]
        public Task SlotsCmd2(int n = 3, string icon = "") => ReplyAsync(new SlotsRunner(Context.User, rand).UseIconSet(SlotIcons).AlsoAllowThese(MemeRigAllows)
                                                .AddUserData(userData.GetOrAdd(Context.User.Id, SlotsUserData.Empty), global)
                                                .DetermineN(n).DetermineIcons(protocolcc2 == null ? icon : "🥮").WithRigging().WithSuppression()
                                                .WithStreakCounting().Run());


        [Command("slots", RunMode = RunMode.Async)]
        [Alias("sloots")]
        [Summary("Slots, but with a specific icon and number")]
        public Task SlotsCmd(string icon, int n = 3)
        {
            return SlotsCmd2(n, icon);
        }

        [Command("niceslots")]
        [Summary("nice.")]
        [HiddenCmd]
        public Task NiceSlots()
        {
            return SlotsCmd2(69);
        }

        [Command("niceslots")]
        [Summary("nice.")]
        [HiddenCmd]
        public Task NiceSlots(string icon)
        {
            return SlotsCmd2(69, icon);
        }

        [Command("maxslots")]
        [Summary("Who needs luck when you have more chances?")]
        [HiddenCmd]
        public Task MaxSlots()
        {
            return SlotsCmd2(100);
        }

        [Command("maxslots")]
        [Summary("Who needs luck when you have more chances?")]
        [HiddenCmd]
        public Task MaxSlots(string icon)
        {
            return SlotsCmd2(100, icon);
        }

        [HiddenCmd]
        [Summary("The real max slots")]
        [Command("ngmaxslots")]
        [Alias("ubermaxslots")]
        public Task NgMaxSlots([Remainder] string icon = "") => SlotsCmd2(195, icon);


        internal static volatile string protocolcc2 = null;

        [Command("nierslots")]
        [Alias("serverslots", "nierlost", "nierslotsç", "niersots", "nierislost", "mierslots", "nierslolts")]
        [Summary("Slots, but with all the emotes in the current server")]
        public Task NierSlots(int n = 3, string icon = "") => ReplyAsync(new SlotsRunner(Context.User, rand).UseIconSet(Context.Guild.Emotes.Select(a => a.ToString()).ToArray())
                                        .AddUserData(userData.GetOrAdd(Context.User.Id, SlotsUserData.Empty), global).DetermineN(n)
                                        .DetermineIcons(protocolcc2 ?? icon).WithSuppression().WithStreakCounting().Run());
        [Command("nierslots")]
        [Alias("serverslots", "nierlost")]
        [Summary("Slots, but with all the emotes in the current server")]
        public Task NierSlots(string icon) => NierSlots(3, icon);

        [Command("leaderboard", RunMode = RunMode.Async)]
        [Alias("slotsboard", "board", "winners", "boards", "wins", "slotswins")]
        [Summary("LINQ makes everything easy, whaddaya mean this should be hard?")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Requires a guild because users")]
        public Task Leaderboard()
        {
            var shortlist = from pair in userData
                            where pair.Value.winsCount > 0
                            orderby -pair.Value.winsCount
                            select pair;
            if (!shortlist.Any())
            {
                return ReplyAsync("Noone has won slots yet! So win them to be here!");
            }
            var finalList = from pair in shortlist
                            select $"{Helpers.GetName(Context.Guild.GetUser(pair.Key))}, {pair.Value.winsCount} win{(pair.Value.winsCount > 1 ? "s" : "")} " +
                                $"({pair.Value.winsCount / (double)global.winsCount * 100:f2}% of total wins)";

            return ReplyAsync($"Global Slots Leaderboard\n--------------------\n{string.Join("\n", finalList)}");
        }

        [Command("streakinfo")]
        [Alias("streak", "streakstats", "streaks")]
        [Summary("Gets info about the longest streaks in slots")]
        public Task GetStreakInfo([Remainder] IGuildUser user = null)
        {
            var uid = user?.Id ?? Context.User.Id;
            var usersData = userData.GetOrAdd(uid, SlotsUserData.Empty);
            if (user != null)
            {
                var uname = Helpers.CleanGuildUserDisplayName(user);
                return ReplyAsync(usersData.longestStreak > 0 ? $"{uname}'s longest streak was {usersData.longestStreak} with {usersData.longestStreakIcon}" :
                    "That user has no streaks."
                );
            }

            var userStreakInfo = usersData.longestStreak > 0 ? $"Your longest streak was {usersData.longestStreak} with {usersData.longestStreakIcon}" :
                "You have no streaks.";
            var globalStreakInfo = global.longestStreak > 0 ? $"The longest streak globally was {global.longestStreak} with {global.longestStreakIcon}" :
                "There are no streaks globally.";
            return ReplyAsync($"{userStreakInfo}\n{globalStreakInfo}");
        }

        private static bool OkRig(string emoji) => SlotIcons.Contains(emoji) || MemeRigAllows.Contains(emoji);

        private static void Rig(string[] RiggedTo, ulong? RiggedUserID)
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

            if (rigTo.Length == 2 && OkRig(rigTo))
            {
                Rig(new string[] { rigTo }, userID);
                return ReplyAsync($"*Bumps the slots* Feels {rigTo}.");
            }
            else
            {
                var tmpRig = Enumerable.Range(0, rigTo.Length / 2).Select(i => rigTo.Substring(2 * i, 2)).Where(c => OkRig(c)).ToArray();
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
        public Task RigMemes()
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return new Task(() => { });
            }
            return ReplyAsync(string.Join(", ", MemeRigAllows));
        }

        [Command("suppressslotswins")]
        [Alias("suppressslots", "noslotswins", "suppress", "suppressslotwins", "suppresslotswins", "suppresslotwins", "globalsuppressslots",
            "globalsuppress", "suppresstatus", "suppressstatus", "d7sqhj2cc2", "2xrrx29958", "disableslotswins", "disablewins")]
        [Summary("Supresses slots winning. Easy. Overrides rigging slots.")]
        [DevOnlyCmd]
        public Task SuppressSlotsWins(bool? suppress = null)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return new Task(() => { });
            }
            var name = Context.Message.ToString().Substring(CommandHandlerService.Prefix.Length).Split(" ")[0].ToLower();
            if (name == "d7sqhj2cc2")
            {
                global.suppressed = true;
                global.takeThisMany = 2;
                protocolcc2 = "<:YoNAH:843152612806754325>";
                return ReplyAsync("Activating protocol.");
            }
            else if (name == "2xrrx29958")
            {
                global.suppressed = false;
                global.takeThisMany = 7;
                protocolcc2 = null;
                return ReplyAsync("Over.");
            }
            if (!suppress.HasValue)
            {
                return ReplyAsync($"Slots wins are currently{(global.suppressed ? "" : " not")} suppressed");
            }
            bool changed = global.suppressed != suppress.Value;
            global.suppressed = suppress.Value;
            return ReplyAsync($"Slots wins are {(changed ? "now" : "still")}{(global.suppressed ? "" : " not")} suppressed.");
        }

        [Command("setdifficulty")]
        [Alias("difficulty", "slotsdifficulty", "setglobaldifficulty", "globaldifficulty", "global", "globalslots", "gloal", "gloval", "glov;")]
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
            var data = userData.GetOrAdd(Context.User.Id, new SlotsUserData());
            if (n < 1)
                return ReplyAsync("Not very ambitious, are we?");
            if (n == 1)
                return ReplyAsync("Maybe try +riskydice?");
            if (data.suppressed && n > 20)
                return ReplyAsync("You only need 20 to get back in you know???");
            if (n == 20 && global.takeThisMany < 21 && !data.suppressed)
                return ReplyAsync("一発で死ぬ。");
            if (n > 9000)
                return ReplyAsync("WHY IS IT OVER 9000!???????");
            if (n > SlotIcons.Length)
                return ReplyAsync("??? Are you serious???");
            if (n > global.takeThisMany - 1 && !data.suppressed)
                return ReplyAsync("Too over the top!!");

            int[] riskySlots = Enumerable.Range(0, n).Select(i => rand.Next(6)).ToArray();
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

        [DevOnlyCmd]
        [Command("resetriskyslots")]
        [Alias("resetrisky")]
        [Summary("Reset a person's risky slot values.")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Only in a server ok? Gotta publicly name them.")]
        public Task ResetRiskySlots(IGuildUser user)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("W-what! I would never!");
            }
            var data = userData.GetOrAdd(user.Id, new SlotsUserData());
            data.suppressed = false;
            data.takeThisMany = 0;
            return ReplyAsync($"Reset riskydice data for {Helpers.GetName(user)}");
        }

        public static object GetSlotsSaveObject()
        {
            return new
            {
                global,
                userData,
                protocolcc2,
                totalIconsRolled,
                sidesData = new { leftDoubles, rightDoubles, sidesDoubles }
            };
        }

        public static void PerpetuatePersistance(SlotsPersistanceJson persistanceData)
        {
            if (persistanceData == null) return;
            global = persistanceData.Global;
            protocolcc2 = persistanceData.ProtocolCC2;
            totalIconsRolled = persistanceData.TotalIconsRolled;
            if (persistanceData.SidesData is not null)
            {
                leftDoubles = persistanceData.SidesData.LeftDoubles;
                rightDoubles = persistanceData.SidesData.RightDoubles;
                sidesDoubles = persistanceData.SidesData.SidesDoubles;
            }
            foreach (var pair in persistanceData.UserData)
            {
                //i think we can assume this works because it should be empty
                userData.TryAdd(pair.Key, pair.Value);
            }
        }
    }
}

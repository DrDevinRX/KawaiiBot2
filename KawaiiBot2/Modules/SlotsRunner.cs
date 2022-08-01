using System;
using System.Collections.Generic;
using System.Text;
using KawaiiBot2;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace KawaiiBot2.Modules
{
    class SlotsRunner
    {
        Random rand;
        IUser _user;
        public SlotsRunner(IUser User, Random Rand)
        {
            _user = User;
            rand = Rand;
        }

        List<string> icons = new();

        List<string> EveryAllowableIcon = new();

        public SlotsRunner UseIconSet(string[] IconSet)
        {
            icons.AddRange(IconSet);
            EveryAllowableIcon.AddRange(IconSet);
            return this;
        }

        public SlotsRunner UseSeasonalIcons(Dictionary<string, string[]> SeasonalIcons)
        {
            //active season
            string season = "";
            string doubleSeason = "";
            switch (DateTime.Today.Month)
            {
                case int n when (n <= 2 || n == 12):
                    season = "Winter";
                    doubleSeason = "SummerWinter";
                    break;
                case int n when (n <= 5):
                    season = "Spring";
                    doubleSeason = "SpringFall";
                    break;
                case int n when (n <= 8):
                    season = "Summer";
                    doubleSeason = "SummerWinter";
                    break;
                case int n when (n <= 11):
                    season = "Fall";
                    doubleSeason = "SpringFall";
                    break;
            }
            UseIconSet(SeasonalIcons[season]);
            UseIconSet(SeasonalIcons[doubleSeason]);
            return this;
        }

        public SlotsRunner UseSpecialIcons(Dictionary<string, string[]> SpecialIcons)
        {
            var now = DateTime.UtcNow;
            throw new NotImplementedException();

        }

        public SlotsRunner AlsoAllowThese(string[] IconSet)
        {
            EveryAllowableIcon.AddRange(IconSet);
            return this;
        }

        SlotsUserData user, global;
        public SlotsRunner AddUserData(SlotsUserData User, SlotsUserData Global)
        {
            user = User;
            global = Global;
            return this;
        }

        int iconsAmt, n;

        public SlotsRunner DetermineN(int N = 3)
        {
            n = N;
            iconsAmt = global.takeThisMany + user.takeThisMany;
            if (iconsAmt < 1) iconsAmt = 1;
            if (iconsAmt > icons.Count) iconsAmt = icons.Count;
            if (iconsAmt == 1 && (user.suppressed || global.suppressed))
                iconsAmt = 2;
            if (n >= 25)
                iconsAmt = 1 + Math.Max(1, (int)Math.Round((global.takeThisMany - 11) / 4.0));
            return this;
        }

        string[] iconsUsed;
        string[] finalSlots;

        public SlotsRunner DetermineIcons(string MakeSureThisIsIn = "")
        {
            iconsUsed = icons.OrderBy(x => rand.Next()).Take(iconsAmt).ToArray();

            if (EveryAllowableIcon.Contains(MakeSureThisIsIn) && !iconsUsed.Contains(MakeSureThisIsIn))
            {
                iconsUsed[0] = MakeSureThisIsIn;
            }

            finalSlots = Enumerable.Range(0, n).Select(i => Helpers.ChooseRandom(iconsUsed)).ToArray();

            if (EveryAllowableIcon.Contains(MakeSureThisIsIn) && !finalSlots.Contains(MakeSureThisIsIn))
            {
                var replaceThis = Helpers.ChooseRandom(finalSlots);
                finalSlots = finalSlots.Select(a => a == replaceThis ? MakeSureThisIsIn : a).ToArray();
            }
            return this;
        }

        bool thisRigged, fullRigged;

        public SlotsRunner WithRigging()
        {
            if (user.riggedTo != null || global.riggedTo != null)
            {
                int riggedLength = (user.riggedTo ?? global.riggedTo).Length;
                if (riggedLength == n || riggedLength == 1)
                {
                    thisRigged = true;
                    fullRigged = riggedLength == n;

                    //choose which one we act on, with user specific data taking preference
                    var thisUserData = user.riggedTo != null ? user : global;
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
            }
            return this;
        }

        public SlotsRunner WithSuppression()
        {
            //Suppress slots wins
            if (user.suppressed || global.suppressed)
            {
                if (thisRigged)
                {
                    //finalslots position is random because slots can be rigged to not-all-the-same
                    //but, this has to be not the icon that's replaced.
                    var finalSlotsTakePut = Helpers.TwoNumbersNoReplace(finalSlots.Length);
                    //use the two unique numbers algorithm to guarantee that it's not the same icon
                    int r1 = icons.IndexOf(finalSlots[finalSlotsTakePut.Item1]);
                    int r2 = rand.Next(icons.Count - 1);
                    r2 += r2 >= r1 ? 1 : 0;
                    finalSlots[finalSlotsTakePut.Item2] = icons[r2];
                }
                else
                {
                    var replace = Helpers.ChooseTwoNoReplace(iconsUsed);
                    var places = Helpers.TwoNumbersNoReplace(finalSlots.Length);
                    //better stuff here
                    finalSlots[places.Item1] = replace.Item1;
                    finalSlots[places.Item2] = replace.Item2;
                }
            }
            return this;
        }

        string streakMessage = "";

        public SlotsRunner WithStreakCounting()
        {
            if (n >= 25 && iconsAmt < 5 && iconsAmt > 1 && !(fullRigged ^ thisRigged))
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
                    streakMessage = $"\nWith a {(thisRigged ? "rigged " : "")}{maxStreak.Item1} streak of {maxStreak.Item2}";

                if (!thisRigged && maxStreak.Item2 >= user.longestStreak)
                {
                    user.longestStreak = maxStreak.Item2;
                    user.longestStreakIcon = maxStreak.Item1;
                }
                if (!thisRigged && maxStreak.Item2 >= global.longestStreak)
                {
                    global.longestStreak = maxStreak.Item2;
                    global.longestStreakIcon = maxStreak.Item1;
                }
            }
            return this;
        }

        int upperLimit = 195;

        public SlotsRunner WithUpperLimit(int UpperLimit)
        {
            upperLimit = Math.Min(UpperLimit, upperLimit);
            return this;
        }

        public string Run()
        {
            if (n < 2 || n > upperLimit)
                return "Nope. No. Nope. No. No can do.";

            bool win = finalSlots.All(s => s == finalSlots[0]);
            bool almostWin = finalSlots.Count(s => s == finalSlots[0]) == n - 1 || finalSlots.Count(s => s == finalSlots[1]) == n - 1;

            string winMessage = "and lost....";

            if (win)
                winMessage = "and won! \uD83C\uDF89";
            else if (almostWin)
                winMessage = $"and almost won ({n - 1}/{n})";

            //For statistical data collection
            if (almostWin && n == 3)
            {
                if (finalSlots[0] == finalSlots[1]) Slots.leftDoubles++;
                else if (finalSlots[1] == finalSlots[2]) Slots.rightDoubles++;
                else Slots.sidesDoubles++;
            }

            if (win)
            {
                user.winsCount++;
                global.winsCount++;
            }

            string prefinal = string.Join(" ", finalSlots);

            if (prefinal.Length > 1800) return "Too long pls stop!";
            Slots.totalIconsRolled += n;

            return
                $"**{Helpers.GetName(_user)}** rolled the slots...\n" +
                $"**[ {prefinal} ]**\n" +
                $"{winMessage}{streakMessage}";
        }
    }
}

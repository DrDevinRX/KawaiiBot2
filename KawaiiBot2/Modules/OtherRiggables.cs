using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.IO;
using KawaiiBot2.JSONClasses;

namespace KawaiiBot2.Modules
{
    public class OtherRiggables : ModuleBase<SocketCommandContext>
    {

        [Command("hi")]
        [Alias("hey")]
        [Summary("Because Father Servo's always down.")]

        public Task HiCmd()
        {
            var win = new Random().Next(10) == 7;
            var rig = yesHey ^ alwaysHey;
            yesHey = false;
            win |= rig;
            return ReplyAsync(win ? "HEY" : "hi");
        }

        private volatile static bool yesHey;
        private volatile static bool alwaysHey;
        [Command("righi")]
        [Summary("Because some people need to win everything.")]
        [HiddenCmd]
        public Task RigHi()
        {
            yesHey = true;
            return ReplyAsync("HEY!");
        }

        [Command("permarighi")]
        [Alias("alwayshey", "permahey")]
        [Summary("There is no choice in the matter.")]
        [DevOnlyCmd]
        public Task PermaRigHi(bool rig = true)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("Such a mode does not exist.");
            }
            alwaysHey = rig;
            return ReplyAsync("HEYHEYHEYHEYHEY");
        }

        [Command("navi")]
        [Summary("Hey, Listen!")]
        [HiddenCmd]
        public Task Navi()
        {
            return ReplyAsync("HEY, listen!");
        }

        private static readonly string[] ChooseResponses =
                JsonConvert.DeserializeObject<string[]>(File.ReadAllText("Resources/ChooseResponses.json"));

        [Command("choose")]
        [Summary("Picks from a list of choices")]
        public Task Choose([Remainder] string choiceString = null)
        {
            IEnumerable<string> choices = choiceString?.Split('|');
            choices = choices?.Select(a => a.Trim())?.Where(a => a is not ("" or null));

            if (choices?.Count() < 2 || choices == null)
            {
                return ReplyAsync("Separate your choices (at least 2) with \"|\"");
            }



            string choice = Helpers.ChooseRandom(choices);
            if (rigChoose is not null)
            {
                if (choices.Contains(rigChoose))
                {
                    choice = rigChoose;
                    rigChoose = null;
                }

                else if (rigChoose == "")
                {
                    choice = choices.First();
                    rigChoose = null;
                }
                //otherwise we just use the default random one
            }
            string choiceRes = Helpers.ChooseRandom(ChooseResponses);

            return ReplyAsync($"I choose this: {string.Format(choiceRes, choice)}");
        }

        volatile static string rigChoose = null;

        [Command("rigchoose")]
        [Summary("Make choose always choose the given (or first) option the next time")]
        [DevOnlyCmd]
        public Task RigChoose([Remainder] string s = "")
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("W-what! I would never!");
            }
            rigChoose = s;
            return ReplyAsync("Done.");
        }


        public static object GetOtherRiggablesSaveObject()
        {
            return new
            {
                rigChoose,
                yesHey,
                alwaysHey
            };
        }

        public static void PerpetuatePersistance(OtherRiggablesPersistanceJson persistanceData)
        {
            if (persistanceData == null) return;
            rigChoose = persistanceData.RigChoose;
            yesHey = persistanceData.YesHey;
            alwaysHey = persistanceData.AlwaysHey;
        }

    }
}

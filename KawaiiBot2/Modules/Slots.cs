﻿using System;
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

        private static readonly string[] SlotIcons = { "🍎", "🍊", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒", "🍌" };

        [Command("slots")]
        [Summary("Roll the slot machine. may rngesus guide your path.")]
        public Task SlotsCmd()
        {

            string[] finalSlots = Enumerable.Range(0, 3).Select(i => Helpers.ChooseRandom(SlotIcons)).ToArray();
            if (rigged && (!riggedUserID.HasValue || riggedUserID == Context.User.Id))
            {
                finalSlots = riggedTo;
                rigged = false;
            }



            string winMessage = "and lost....";

            if (finalSlots[0] == finalSlots[1] && finalSlots[1] == finalSlots[2])
                winMessage = "and won! \uD83C\uDF89";
            else if (finalSlots[0] == finalSlots[1] || finalSlots[1] == finalSlots[2] || finalSlots[0] == finalSlots[2])
                winMessage = "and almost won (2/3)";



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

        private Task RigCommon(string rigTo, ulong? userID = null)
        {
            rigTo = rigTo.Replace(" ", "");
            if (rigTo.Length == 2 && SlotIcons.Contains(rigTo))
            {
                Rig(new string[] { rigTo, rigTo, rigTo }, userID);
                return ReplyAsync($"*Bumps the slots* Feels {rigTo}.");
            }
            else if (rigTo.Length == 6)
            {
                var tmpRig = Enumerable.Range(0, 3).Select(i => rigTo.Substring(2 * i, 2)).Where(c => SlotIcons.Contains(c)).ToArray();
                if (tmpRig.Length == 3)
                {
                    Rig(tmpRig, userID);
                    return ReplyAsync($"*Bumps the slots* Feels {rigTo}.");
                }
                else
                    return ReplyAsync("W-what! I would never!");
            }
            else
            {
                return ReplyAsync("Syntax fail.");
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

            return ReplyAsync("*falls on the floor, missing the slot machine*");
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

    }
}

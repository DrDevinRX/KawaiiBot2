using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.IO;
using Newtonsoft.Json;
using System.Security;
using KawaiiBot2.JSONClasses;
using System.Security.Cryptography;
using Discord.Interactions;
using KawaiiBot2.Modules.Shared;
using KawaiiBot2.Modules;

namespace KawaiiBot2.Modules.SlashCommands
{
    public class SlotsCmds : InteractionModuleBase
    {
        [SlashCommand("slots", "Roll the slot machine. may rngesus guide your path. Better code.")]
        public async Task SlotsCmd(int n = 3, string icon = "")
            => await RespondAsync(new SlotsRunner(Context.User, Slots.rand).UseIconSet(Slots.SlotIcons).AlsoAllowThese(Slots.MemeRigAllows)
                                                .AddUserData(Slots.userData.GetOrAdd(Context.User.Id, SlotsUserData.Empty), Slots.global)
                                                .DetermineN(n).DetermineIcons(Slots.protocolcc2 == null ? icon : "🥮").WithRigging().WithSuppression()
                                                .WithStreakCounting().Run());
        [SlashCommand("nierslots", "Slots, but with all the emotes in the current server")]
        public async Task NierSlots(int n = 3, string icon = "")
             => await RespondAsync(new SlotsRunner(Context.User, Slots.rand).UseIconSet(Context.Guild.Emotes.Select(a => a.ToString()).ToArray())
                                         .AddUserData(Slots.userData.GetOrAdd(Context.User.Id, SlotsUserData.Empty), Slots.global).DetermineN(n)
                                         .DetermineIcons(Slots.protocolcc2 ?? icon).WithSuppression().WithStreakCounting().Run());
    }
}

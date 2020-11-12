using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Diagnostics;
using KawaiiBot2.Services;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace KawaiiBot2.Modules
{
    [RequireContext(ContextType.Guild, ErrorMessage = "I can't know where you want the role if you're not in a server?")]
    public class Elements : ModuleBase<SocketCommandContext>
    {

        readonly Dictionary<string, ulong> elementsToRoleID =
                    JsonConvert.DeserializeObject<Dictionary<string, ulong>>(File.ReadAllText("Resources/Elements.json"));

        [Command("giveelement", RunMode = RunMode.Async)]
        [Summary("Give yourself an element role.")]
        [Alias("giverole", "element", "givelement", "addrole", "addelement")]
        public async Task GiveElement([Remainder] string element = null)
        {
            var server = Context.Guild;
            //only in grace's impact server
            if (server.Id != 761750996610318376)
            {
                return;
            }
            //need an element
            if (element == null)
            {
                await ReplyAsync("Need a role to give!");
                return;
            }
            element = element.ToLower();
            //need a valid element
            if (!elementsToRoleID.ContainsKey(element))
            {
                await ReplyAsync("That's not an element!");
                return;
            }
            var role = Context.Guild.GetRole(elementsToRoleID[element]);

            await (Context.User as IGuildUser).AddRoleAsync(role);
            await ReplyAsync("Added role.");
        }

        [Command("removeelement", RunMode = RunMode.Async)]
        [Summary("Remove an element role from yourself")]
        [Alias("takerole", "noelement", "takeelement", "remove")]
        public async Task RemoveElement([Remainder] string element = null)
        {
            var server = Context.Guild;
            //only in grace's impact server
            if (server.Id != 761750996610318376)
            {
                return;
            }
            //need an element
            if (element == null)
            {
                await ReplyAsync("Need a role to remove!");
                return;
            }
            //need a valid element
            if (!elementsToRoleID.ContainsKey(element))
            {
                await ReplyAsync("That's not an element!");
                return;
            }
            var role = Context.Guild.GetRole(elementsToRoleID[element]);

            await (Context.User as IGuildUser).RemoveRoleAsync(role);
            await ReplyAsync("Removed role.");
        }

        [Command("lewdme")]
        [Summary("Give yourself the 18+ role for access to the porn channel")]
        [RequireContext(ContextType.Guild)]
        public async Task giveMeR18Role([Remainder] string ack = null)
        {

            var server = Context.Guild;
            //only in grace's impact server
            if (server.Id != 761750996610318376)
            {
                return;
            }
            var userHashCode = Context.User.Id.GetHashCode();
            var ackCode = (userHashCode & 0x7FFF_FFFF) >> 50;
            var ackCodeStr = $"I acknowledge with ack code {ackCode}";
            if (ack == null)
            {
                await ReplyAsync("By accepting this, you are saying that you are above 18 and allowed to view porn stuff.\n" +
                    "This is the really really heavy stuff, the less bad stuff is in lewd art channel" +
                    "Type " +
                    $"`+lewdme {ackCodeStr}` to accept the disclaimer and get the role.\n" +
                    $"(Make sure to type it exactly or copy-paste! With nothing more than what's in the ``!)");
                return;
            }
            var role = Context.Guild.GetRole(767142223388344330);//lewd role id
            if (ack == ackCodeStr)
            {
                await (Context.User as IGuildUser).AddRoleAsync(role);
                await ReplyAsync("Lewd role added.");
            }
            else if (ack == "remove")
            {
                await (Context.User as IGuildUser).RemoveRoleAsync(role);
                await ReplyAsync("Lewd role removed.");
            }
            else
            {
                await (Context.User as IGuildUser).RemoveRoleAsync(role);
                await ReplyAsync("Lewd role denied/removed.\n" +
                    "If you wanted it, did you get your ack code correct? Use without any parameters to see it.");
            }

        }

    }
}

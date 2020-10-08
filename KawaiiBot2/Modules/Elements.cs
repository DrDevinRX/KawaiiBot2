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

namespace KawaiiBot2.Modules
{
    [RequireContext(ContextType.Guild, ErrorMessage = "I can't know where you want the role if you're not in a server?")]
    public class Elements : ModuleBase<SocketCommandContext>
    {

        readonly Dictionary<string, ulong> elementsToRoleID =
                    JsonConvert.DeserializeObject<Dictionary<string, ulong>>(File.ReadAllText("Resources/Elements.json"));

        [Command("giveelement", RunMode = RunMode.Async)]
        [Summary("Give yourself an element role.")]
        [Alias("giverole", "element", "givelement", "addrole")]
        public async Task GiveElement([Remainder] string element = null)
        {
            var server = Context.Guild;
            //only in grace's impact server
            if (server.Id != 761750996610318376)
            {
                await ReplyAsync("No!");
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
                await ReplyAsync("No!");
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
    }
}

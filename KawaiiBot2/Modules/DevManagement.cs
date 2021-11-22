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
using System.Runtime.CompilerServices;

namespace KawaiiBot2.Modules
{
    public class DevManagement : ModuleBase<SocketCommandContext>
    {
        [HitoOnlyCmd]
        [Command("listdev")]
        [Alias("devlist", "listdevs")]
        [Summary("List current developer IDs")]
        public Task DevList()
        {
            if (Context.User.Id != 173529942431236096) return Task.Run(() => { });
            if (Helpers.devIDs.Length == 0) return ReplyAsync("No gods, no heroes.");
            return ReplyAsync(string.Join(", ", Helpers.devIDs));
        }

        [HitoOnlyCmd]
        [Command("devgrant")]
        [Alias("grantdev", "makeadmin")]
        [Summary("Give someone dev priveleges.")]
        public Task DevGrant(ulong userID)
        {
            if (Context.User.Id != 173529942431236096) return Task.Run(() => { });
            if (Helpers.devIDs.Contains(userID)) return ReplyAsync("They already have them?");
            var tmp = Helpers.devIDs.ToList();
            tmp.Add(userID);
            Helpers.devIDs = tmp.ToArray();
            return ReplyAsync($"Gave dev priveleges to ID#{userID}");
        }
        [HitoOnlyCmd]
        [Command("devgrant")]
        [Alias("grantdev", "makeadmin")]
        [Summary("Give someone dev priveleges.")]
        public Task DevGrant(IUser user)
        {
            if (Context.User.Id != 173529942431236096) return Task.Run(() => { });
            if (Helpers.devIDs.Contains(user.Id)) return ReplyAsync("They already have them?");
            var tmp = Helpers.devIDs.ToList();
            tmp.Add(user.Id);
            Helpers.devIDs = tmp.ToArray();
            return ReplyAsync($"Gave dev priveleges to ID#{user.Id}");
        }

        [HitoOnlyCmd]
        [Command("devremove")]
        [Alias("removedev", "removeadmin")]
        [Summary("Take away someone's dev priveleges.")]
        public Task DevRemove(ulong userID)
        {
            if (Context.User.Id != 173529942431236096) return Task.Run(() => { });
            if (!Helpers.devIDs.Contains(userID)) return ReplyAsync("They're already a pleb?");
            var tmp = Helpers.devIDs.ToList();
            tmp.Remove(userID);
            Helpers.devIDs = tmp.ToArray();
            return ReplyAsync($"Removed dev priveleges from ID#{userID}");
        }
        [HitoOnlyCmd]
        [Command("devremove")]
        [Alias("removedev", "removeadmin")]
        [Summary("Take away someone's dev priveleges.")]
        public Task DevRemove(IUser user)
        {
            if (Context.User.Id != 173529942431236096) return Task.Run(() => { });
            if (!Helpers.devIDs.Contains(user.Id)) return ReplyAsync("They're already a pleb?");
            var tmp = Helpers.devIDs.ToList();
            tmp.Remove(user.Id);
            Helpers.devIDs = tmp.ToArray();
            return ReplyAsync($"Removed dev priveleges from ID#{user.Id}");
        }
    }
}

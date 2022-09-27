using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
using KawaiiBot2.JSONClasses;

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
        [Alias("grantdev", "makeadmin", "givedev")]
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




        public static ConcurrentDictionary<ulong, HashSet<string>> NoUsingThis = new();

        [HiddenCmd]
        [Command("no")]
        public Task NoUse(string s)
        {
            if (s == "u")
            {
                return ReplyAsync("no no u");
            }
            return Task.Run(() => { });
        }

        [HitoOnlyCmd]
        [Command("no")]
        [Alias("allow")]
        [Summary("i hate life")]
        public Task NoUse(string use, string cmd, string f = null, [Remainder] IUser user = null)
        {
            if (Context.User.Id != 173529942431236096) return Task.Run(() => { });

            var name = Context.Message.ToString().Substring(CommandHandlerService.Prefix.Length).Split(" ")[0].ToLower();

            //did you get user wrong?

            //incorrect command usage
            if (use != "using" || //this always has to be this
                (f is not ("for" or null)) || //condition
                (user == null ^ f == null)//they must be both null or non-null
                )
            {
                return ReplyAsync("Hah! You think you can just waltz in here and do whatever you want?");
            }

            ulong key = user?.Id ?? ulong.MaxValue;

            var ud = NoUsingThis.GetOrAdd(key, new HashSet<string>());
            if (name == "no")
                ud.Add(cmd);
            else if (name == "allow")
                ud.Remove(cmd);
            return ReplyAsync("ヽ(￣ω￣(￣ω￣〃)ゝ");
        }

        public static object GetManagementSaveObj()
        {
            return new
            {
                devs = Helpers.devIDs,
                noUse = NoUsingThis
            };
        }

        public static void PerpetuatePersistance(ManagementPersistanceJson persistanceJson)
        {
            if (persistanceJson == null) return;

            if (persistanceJson.Devs != null) Helpers.devIDs = persistanceJson.Devs;

            Dictionary<ulong, string[]> persistanceData = persistanceJson.NoUse;
            if (persistanceData == null) return;
            foreach (var pair in persistanceData)
            {
                var b = new HashSet<string>();
                foreach (var x in pair.Value)
                {
                    b.Add(x);
                }
                NoUsingThis.TryAdd(pair.Key, b);
            }
        }
    }
}

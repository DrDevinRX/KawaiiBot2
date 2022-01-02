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
using System.Collections.Concurrent;
using KawaiiBot2.JSONClasses;

namespace KawaiiBot2.Modules
{
    public class Informational : ModuleBase<SocketCommandContext>
    {
        [Command("about")]
        [Summary("About me~ You want to know more~?")]
        public Task About()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder
                .WithTitle($"ℹ {Program.BotName}")
                .AddField("Developers", "Hitoccchi (and Yin somewhat)")
                .AddField("Base", "KawaiiiBot (now offline) and the nier speedruns Awooo")
                .AddField("My Server!", "https://discord.nierspeedrun.com", true)
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());


            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("source")]
        [Alias("code", "github")]
        [Summary("Source code link!")]
        [HiddenCmd]
        public Task Source()
        {
            return ReplyAsync("Build your own bot using this source code:\nhttps://github.com/DrDevinRX/Kawaiibot2");
        }

        [Command("avatar")]
        [Summary("Displays your avatar")]
        public Task Avatar(IGuildUser user = null)
        {

            user ??= (Context.User as IGuildUser);

            IUser baseUser = user ?? (Context.User as IUser);

            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithDescription($"{Helpers.GetName(baseUser)}'s Avatar\n[Full Image]({baseUser.GetEffectiveAvatarUrl()})")
                .WithThumbnailUrl(baseUser.GetEffectiveAvatarUrl());

            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("joinedat")]
        [Alias("joined")]
        [Summary("Check when a user joined the current server")]
        [RequireContext(ContextType.Guild, ErrorMessage = "W-what? I-I don't know when you joined because this is nowhere!")]
        public Task JoinedAt(IGuildUser user = null)
        {
            var guildUser = user ?? (Context.Message.Author as IGuildUser);
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithDescription($"**{Helpers.CleanGuildUserDisplayName(guildUser)}** joined {Context.Guild.Name.Clean()} on {guildUser.JoinedAt}")
                .WithThumbnailUrl(guildUser.GetEffectiveAvatarUrl())
                .WithTimestamp(guildUser.JoinedAt ?? DateTimeOffset.Now);

            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("server")]
        [RequireContext(ContextType.Guild, ErrorMessage = "I'm running on hitoccchi's custom 8 EPYC CPU, 6TB RAM server! It's so spacious!")]
        [Summary("Information about the current server")]
        public Task Server()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle($"ℹ Information about {Context.Guild.Name}")
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .AddField("Server name", Context.Guild.Name, true)
                .AddField("Server ID", Context.Guild.Id, true)
                .AddField("Members", Context.Guild.MemberCount, true)
                //.AddField("Owner", $"{Context.Guild.Owner.Username}#{Context.Guild.Owner.Discriminator}", true)
                .AddField("Created", Context.Guild.CreatedAt, true);
            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("user")]
        [Summary("Get user information")]
        public Task User(IGuildUser user = null)
        {
            user ??= (Context.User as IGuildUser);

            IUser baseUser = user ?? (Context.User as IUser);

            if (baseUser == null)
            {
                return ReplyAsync();
            }

            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle($"ℹ About **{baseUser.Id}**")
                .WithThumbnailUrl(baseUser.GetEffectiveAvatarUrl())
                .AddField("Full name", $"{baseUser.Username.Clean()}#{baseUser.Discriminator}", true);
            if (user != null)
                embedBuilder.AddField("Nickname", user.Nickname?.Clean() ?? "None", true);
            embedBuilder.AddField("Account created", baseUser.CreatedAt, true);
            if (user != null)
                embedBuilder.AddField("Joined this server", user.JoinedAt, true);

            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("stats")]
        [DevOnlyCmd]
        [Summary("View internal information")]
        public Task Stats()
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("You must be a developer to use this command");
            }

            Process proc = Process.GetCurrentProcess();
            var uptime = DateTime.Now - proc.StartTime;
            var cph = CommandHandlerService.CommandsExecuted / ((uptime.TotalHours));
            var mbpriv = proc.PrivateMemorySize64 / 1_000_000d;
            var mbwork = proc.WorkingSet64 / 1_000_000d;
            var mbvirt = proc.VirtualMemorySize64 / 1_000_000d;
            var mbpaged = proc.PagedMemorySize64 / 1_000_000d;
            var mbunpaged = proc.NonpagedSystemMemorySize64 / 1_000_000d;

            Dictionary<string, string> stats = new Dictionary<string, string>()
            {
                {"\nPROCESS STATISTICS\u200b\u200b","ヾ(•ω•`)o ::\n" },
                { "☆Uptime☆",uptime.ToString()},
                {"☆Private Memory☆",mbpriv.ToString("f2")+"MB" },
                {"☆Working set☆",mbwork.ToString("f2")+"MB" },
                {"☆Virtual Memory☆",mbvirt.ToString("f2")+"MB" },
                {"☆Paged Memory☆",mbpaged.ToString("f2")+"MB" },
                {"☆Unpaged Memory☆",mbunpaged.ToString("f2")+"MB" },
                {"☆Threads☆",proc.Threads.Count.ToString() },
                {"\nBOT STATISTICS\u200b\u200b","( *^-^)ρ(*╯^╰) ::" },//"§(*￣▽￣*)§ ::\n" },
                {"☆Commands☆", CommandHandlerService.CommandsExecuted.ToString() },
                {"☆Commands Per Hour☆", cph.ToString("f3") },
                {"☆Commands All Time☆",CommandCount.Sum((a)=>a.Value).ToString() },
                {"☆Total Slot Icons☆",Slots.totalIconsRolled.ToString() }
            };

            var q = from stat in stats
                    select $"{Helpers.Pad(stat.Key, 23)}:: {stat.Value}";

            return ReplyAsync($"```   ===  {Program.BotName} Statistics  === \n{string.Join('\n', q)}```");
        }

        public static ConcurrentDictionary<string, int> CommandCount = new ConcurrentDictionary<string, int>();

        [Command("top10commands")]
        [Alias("top", "top10", "popularity", "howmuchslots", "mostused")]
        [Summary("Shows the top 10 most used commands.")]
        public Task PopularityContest()
        {
            var list = (from pair in CommandCount
                        where pair.Value > 0
                        orderby -pair.Value
                        select $"{pair.Key}: {pair.Value} times").Take(10);
            if (list.Count() == 0)
                return ReplyAsync("No commands have been used yet. This is most unusual.");
            return ReplyAsync($"Most Used Commands\n--------------------\n{string.Join("\n", list)}");
        }

        readonly private static string[] slotcommands = new string[] { "aprilfruits", "slotsv2", "slots", "niceslots", "maxslots", "ngmaxslots",
                                                                "nierslots", "leaderboard", "streakinfo", "rigslots", "rigmemes", "suppressslotswins",
                                                                "setdifficulty", "riskydice", "riskyslots", "resetrisky","slotsisoverused","yuuhislots",
                                                                "totalrolls", "sidesrolls" };

        [Command("slotsisoverused")]
        [Summary("Slots accounts for how many % of total commands?")]
        public Task SlotsIsOverused()
        {
            var count = (from pair in CommandCount select pair.Value).Sum();
            var slotscount = (from pair in CommandCount where slotcommands.Contains(pair.Key) select pair.Value).Sum();
            return ReplyAsync($"Slots accounts for {slotscount / (float)count * 100:f0}% of total commands.");
        }

        [HiddenCmd]
        [Summary("injoke")]
        [Command("yuuhislots")]
        public Task YuuhiSlots()
        {
            return ReplyAsync("ゆうひ～#2418: \"Nothing on Awooo is used but slots.\"");
        }

        [Command("usecount")]
        public Task UseCount(string commandName)
        {
            if (CommandCount.ContainsKey(commandName))
            {
                return ReplyAsync($"{commandName} has been used {CommandCount[commandName]} times.");
            }
            else
            {
                return ReplyAsync("The command has either never been used, is a nickname, or doesn't exist.");
            }
        }

        public static object GetPopularitySave()
        {
            return new
            {
                commandCount = CommandCount
            };
        }

        public static void PerpetuatePopularityPersistance(PopularityPersistanceJson persistanceData)
        {
            if (persistanceData == null) return;
            foreach (var pair in persistanceData.CommandCount)
            {
                //i think we can assume this works because it should be empty
                CommandCount.TryAdd(pair.Key, pair.Value);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace KawaiiBot2.Modules
{
    public class Informational : ModuleBase<SocketCommandContext>
    {
        [Command("about")]
        public Task About()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder
                .WithTitle("ℹ Awooo v2")
                .AddField("Developers", "Yin, (not really hitoccchi)")
                .AddField("Base", "KawaiiiBot (now offline) and the nier speedruns Awooo")
                .AddField("My Server!", "https://discord.nierspeedrun.com", true)
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());


            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("avatar")]
        public Task Avatar(IGuildUser user = null)
        {
            var guildUser = user ?? (Context.Message.Author as IGuildUser);
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithDescription($"{Helpers.CleanGuildUserDisplayName(guildUser)}'s Avatar\n[Full Image]({guildUser.GetAvatarUrl()})")
                .WithThumbnailUrl(guildUser.GetAvatarUrl());

            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("joinedat")]
        public Task JoinedAt(IGuildUser user = null)
        {
            var guildUser = user ?? (Context.Message.Author as IGuildUser);
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithDescription($"**{Helpers.CleanGuildUserDisplayName(guildUser)}** joined {Context.Guild.Name.Clean()} on {guildUser.JoinedAt}")
                .WithThumbnailUrl(guildUser.GetAvatarUrl())
                .WithTimestamp(guildUser.JoinedAt ?? DateTimeOffset.Now);

            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("server")]
        public Task Server()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle($"ℹ Information about {Context.Guild.Name}")
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .AddField("Server name", Context.Guild.Name, true)
                .AddField("Server ID", Context.Guild.Id, true)
                .AddField("Members", Context.Guild.MemberCount, true)
                .AddField("Owner", $"{Context.Guild.Owner.Username}#{Context.Guild.Owner.Discriminator}", true)
                .AddField("Created", Context.Guild.CreatedAt, true);
            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        [Command("user")]
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
                .WithThumbnailUrl(baseUser.GetAvatarUrl())
                .AddField("Full name", $"{baseUser.Username}#{baseUser.Discriminator}", true);
            if (user != null)
                embedBuilder.AddField("Nickname", user.Nickname ?? "None", true);
            embedBuilder.AddField("Account created", baseUser.CreatedAt, true);
            if (user != null)
                embedBuilder.AddField("Joined this server", user.JoinedAt, true);

            return Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }
    }
}

﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KawaiiBot2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private static readonly Random Rng = new Random();
        private static readonly string[] SlotIcons = { "🍎", "🍊", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒" };

        [Command("hi")]
        public Task Hi()
        {
            if (Context.Message.Author.IsBot)
            {
                return Task.CompletedTask;
            }

            return ReplyAsync($"Hi, {Context.Message.Author.Mention}");
        }

        [Command("slots")]
        public Task Slots()
        {
            string[] finalSlots = new string[3];
            string winMessage;
            Dictionary<string, int> occuranceCount = new Dictionary<string, int>();

            for (int i = 0; i < 3; i++)
            {
                string slot = SlotIcons[Rng.Next(SlotIcons.Length)];
                finalSlots[i] = slot;
                // Add to the dictionary for easy occurance counting
                if (occuranceCount.ContainsKey(slot))
                {
                    occuranceCount[slot]++;
                }
                else
                {
                    occuranceCount[slot] = 1;
                }
            }

            // If we have more than one key, we didn't win
            if (occuranceCount.Count > 1)
            {
                winMessage = (occuranceCount.Count > 2) ? "and lost..." : $"and almost won ({occuranceCount.Count}/{finalSlots.Length})";
            }
            else
            {
                winMessage = $"and won! \uD83C\uDF89";
            }

            var guildUser = Context.Message.Author as IGuildUser;

            return ReplyAsync(
                            $"**{guildUser.Nickname ?? guildUser.Username}** rolled the slots...\n" +
                            $"**[ {string.Join(" ", finalSlots)} ]**\n" +
                            $"{winMessage}");
        }

        // TODO: Everything
        [Command("about")]
        public Task About()
        {
            return Task.CompletedTask;
        }

        // TODO: Embed and formatting
        [Command("joinedat")]
        public Task JoinedAt(IGuildUser user = null)
        {
            if (Context.Message.Author.IsBot)
            {
                return Task.CompletedTask;
            }

            // TODO
            EmbedBuilder embedBuilder = new EmbedBuilder();

            var guildUser = user ?? (Context.Message.Author as IGuildUser);
            return ReplyAsync($"**{guildUser.Nickname ?? guildUser.Username}** joined on {guildUser.JoinedAt}");
        }
    }
}

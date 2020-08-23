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
    public class Lowlives : ModuleBase<SocketCommandContext>
    {
        [Command("clean")]
        [Summary("they did nothing!")]
        [DevOnlyCmd]
        public Task Clean([Remainder] string username)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("I-I don't know what you're talking about... :point_right: :point_left: ");
            }
            if (Hi.riggers.Contains(username)) Hi.riggers.RemoveAll(s => s == username);
            if (Slots.riggers.Contains(username)) Slots.riggers.RemoveAll(s => s == username);
            return ReplyAsync($"Cleaned the record for {username.Clean()}");
        }

        [Command("frame")]
        [Summary("they did something!")]
        [HiddenCmd]
        public Task Frame([Remainder] string username)
        {
            var rn = new Random();
            var whoToFrame = rn.Next(5) > 3 ? Helpers.GetName(Context.User) : username.Clean();
            if (rn.Next(2) == 0)
                Hi.riggers.Add(whoToFrame);
            else
                Slots.riggers.Add(whoToFrame);
            return ReplyAsync($"Framed {whoToFrame} as a rigger");
        }

        [Command("showhito")]
        [Summary("Hitoccchi (yes,no) collusion")]
        public Task ShowHito(bool show)
        {
            if (!Helpers.devIDs.Contains(Context.User.Id))
            {
                return ReplyAsync("I-I don't know what you're talking about... :point_right: :point_left: ");
            }
            showHito = show;
            var maybenot = show ? "" : " not";
            return ReplyAsync($"Got it! Hito will{maybenot} show up as being the head of everything.");
        }

        private static bool showHito = true;

        [Command("collusion")]
        [Summary("Tell who's a member of the hi slots mafia")]
        public Task Collusion()
        {
            var q = showHito ? "Head mafiosa and bot code colluder: hitoccchi\n" : "";
            return ReplyAsync($"```{q}" +
                $"Hi riggers: \n{string.Join('\n', Hi.riggers)}\n" +
                $"Slots riggers: \n{string.Join('\n', Slots.riggers)}```");
        }
    }
}

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
using KawaiiBot2.JSONClasses;

namespace KawaiiBot2.Modules
{
    public class RPS : ModuleBase<SocketCommandContext>
    {
        public RPS()
        {
            RPSJson basicJson = JsonConvert.DeserializeObject<RPSJson>(File.ReadAllText("Resources/RPS.json"));
            basicOptions = basicJson.Options;
            basicTable = basicJson.Table;
            RPSJson LSJson = JsonConvert.DeserializeObject<RPSJson>(File.ReadAllText("Resources/RPSLS.json"));
            LSOptions = LSJson.Options;
            LSTable = LSJson.Table;
            RPSJson _101Json = JsonConvert.DeserializeObject<RPSJson>(File.ReadAllText("Resources/RPS101.json"));
            _101Options = _101Json.Options;
            _101Table = _101Json.Table;
        }

        private string[] basicOptions;
        private Dictionary<string, WinLoseLists> basicTable;
        private string[] LSOptions;
        private Dictionary<string, WinLoseLists> LSTable;
        private string[] _101Options;
        private Dictionary<string, WinLoseLists> _101Table;

        private Task RPSPlus(string userSelected, string[] options, Dictionary<string, WinLoseLists> table)
        {
            userSelected = userSelected?.ToLower();
            if (!options.Contains(userSelected))
            {
                var optionsArray = (from optionn in options select $"`{optionn}`").ToArray();
                return ReplyAsync($"You have to choose between {string.Join(", ", optionsArray[0..^1])}, or {optionsArray[^1]} ;-;");
            }
            var botSelected = Helpers.ChooseRandom(options);

            if (userSelected == botSelected) return ReplyAsync("It was a tie, no one wins...");

            if (table[userSelected].Beats.Contains(botSelected))
                return ReplyAsync($"You win! {userSelected} beats {botSelected}");
            else
                return ReplyAsync($"I win! {botSelected} beats {userSelected}");
        }


        [Command("rps")]
        [Summary("Rock, paper, Scissors")]

        public Task RockPaperScissors([Remainder] string userSelected = null)
        {
            return RPSPlus(userSelected, basicOptions, basicTable);
        }

        [Command("rpsls")]
        [Summary("Rock, Paper, Scissors, Lizard, Spock")]
        public Task RPSLS([Remainder] string userSelected = null)
        {
            return RPSPlus(userSelected, LSOptions, LSTable);
        }

        [Command("rps101")]
        [Summary("Rock, Paper, Scissors 101 from https://www.umop.com/rps101.htm")]
        public Task RPS101([Remainder] string userSelected = null)
        {
            return RPSPlus(userSelected, _101Options, _101Table);
        }
    }
}

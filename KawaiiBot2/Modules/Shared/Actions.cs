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
using KawaiiBot2.Modules.Shared;

namespace KawaiiBot2.Modules.Shared
{
    internal static class Actions
    {
        private static ThrowJson throwJson = JsonConvert.DeserializeObject<ThrowJson>(File.ReadAllText("Resources/ThrowResponses.json"));
        internal static string Throw(string authorname, IGuildUser target)
        {
            if (target == null)
            {
                return "You need to throw stuff at someone...?";
            }


            var targetname = Helpers.CleanGuildUserDisplayName(target);
            if (targetname == null) return "You need to throw stuff at someone...?";

            var authorQuote = Helpers.ChooseRandom(throwJson.AuthorQuotes);
            var targetQuote = Helpers.ChooseRandom(throwJson.TargetQuotes);
            var item = Helpers.ChooseRandom(throwJson.Items);

            return $"**{authorname}** threw {item} at **{targetname}**\n\n" +
                $"{targetname}: {targetQuote}\n{authorname}: {authorQuote}";
        }
    }
}

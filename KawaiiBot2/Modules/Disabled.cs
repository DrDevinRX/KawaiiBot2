using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace KawaiiBot2.Modules
{
    public class Disabled : ModuleBase<SocketCommandContext>
    {
        readonly private static Dictionary<string, string> whyDisabled =
            JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Resources/WhyDisabled.json"));
        [Command("unavailable")]
        [Summary("Disabled commands. Run with one to see why it's disabled.")]
        /*First are actual aliases, then buttsbot conditionally, then the actually disabled commands.*/
        [Alias("disable", "disabled",
#if NOBUTTS
            "buttsbot",
#endif
             "ship", "memegen", "meme2", "captcha", "calling", "facts", "scroll", "supreme", "achievement", "challenge", "drake", "didyoumean"
            )]
        public Task DisabledCommand(string commandname = "")
        {
            if (commandname == "")
                return ReplyAsync("This command is unfortunately currently unavailable. Run this again with the command as an argument to see why.");
            else if (whyDisabled.ContainsKey(commandname))
                return ReplyAsync($"This command is unavailable because {whyDisabled[commandname]}.");
            else if (commandname is "disable" or "disabled" or "unavailable")
                return ReplyAsync("This is a functioning command to show which commands have been disabled.");
            else
                return ReplyAsync("Huh? That's not an unavailable command?");
        }
    }
}

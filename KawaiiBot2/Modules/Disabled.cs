using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using KawaiiBot2.Services;

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
#if !HAS_AXOLOTL
            "axolotl",
#endif
             "ship", "memegen", "meme2", "captcha", "calling", "facts", "scroll", "supreme", "achievement",
            "challenge", "drake", "didyoumean"
            )]
        public Task DisabledCommand([Remainder] string _ = "")
        {
            var commandname = Context.Message.ToString().Substring(CommandHandlerService.Prefix.Length).Split(" ")[0].ToLower();
            if (whyDisabled.ContainsKey(commandname))
                return ReplyAsync($"This command is unavailable because {whyDisabled[commandname]}.");
            else if (commandname is "disable" or "disabled" or "unavailable")
                return ReplyAsync("This is a functioning command to show which commands have been disabled.");
            else
                return ReplyAsync("There's no reason available, but this command is unavailable.");
        }
    }
}

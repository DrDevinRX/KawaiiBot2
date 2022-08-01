using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using KawaiiBot2.Modules.Shared;

namespace KawaiiBot2.Modules.TextTriggered
{
    public class ActionsCmds : ModuleBase<SocketCommandContext>
    {
        [Command("throw")]
        [Summary("Throw something at someone >:3")]
        [RequireContext(ContextType.Guild, ErrorMessage = "I throw it right back at you!")]
        public Task Throw(IGuildUser user = null)
    => ReplyAsync(Actions.Throw(Helpers.CleanGuildUserDisplayName(Context.User as IGuildUser), user));
    }
}

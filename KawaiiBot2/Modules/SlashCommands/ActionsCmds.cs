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
using Discord.Interactions;
using KawaiiBot2.Modules.Shared;

namespace KawaiiBot2.Modules.SlashCommands
{
    public class ActionsCmds : InteractionModuleBase
    {
        [SlashCommand("throw", "Throw something at someone >:3")]
        public Task Throw(IGuildUser user = null)
            => RespondAsync(Actions.Throw(Helpers.CleanGuildUserDisplayName(Context.User as IGuildUser), user));
    }
}

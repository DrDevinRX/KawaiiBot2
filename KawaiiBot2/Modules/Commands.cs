using Discord.Commands;
using System.Threading.Tasks;

namespace KawaiiBot2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        public Task Hi()
        {
            if (Context.Message.Author.IsBot)
            {
                return Task.CompletedTask;
            }

            return ReplyAsync($"Hi, {Context.Message.Author.Mention}");
        }
    }
}

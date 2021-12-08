using Discord.Commands;
using System.Threading.Tasks;
using KawaiiBot2.APIInterfacing.Interfaces;
using KawaiiBot2.APIInterfacing;
using KawaiiBot2.APIInterfacing.ResultSchemas;
using Newtonsoft.Json;

namespace KawaiiBot2.Modules
{
    public class RandomImages : ModuleBase<SocketCommandContext>
    {

        /*[Command("cat", RunMode = RunMode.Async)]
        [Summary("cats. Cats. CATS!")]
        public async Task Cat()
        {
            (bool success, string url) = await NekosLifeInterface.TryGetEndpoint("cat");
            if (!success)
            {
                await ReplyAsync("I-I couldn't find any cats... I'm sorry ;-;");
                return;
            }
            await ReplyAsync(url);
        }*/

        [Command("birb", RunMode = RunMode.Async)]
        [Alias("bird")]
        [Summary("Cute birbs :2")]
        public async Task Birb()
        {
            (bool success, string url) = await AlexFlipnoteInterface.TryGetEndpoint("birb");
            if (!success)
            {
                await ReplyAsync("I-I couldn't find any birbs... I'm sorry ;-;");
                return;
            }
            await ReplyAsync(url);

        }

        [Command("coffee", RunMode = RunMode.Async)]
        [Summary("Coffee images to wake you up!")]
        public async Task Coffee()
        {
            (bool success, string url) = await AlexFlipnoteInterface.TryGetEndpoint("coffee");
            if (!success)
            {
                await ReplyAsync("I-I couldn't find any coffee... I'm sorry ;-;");
                return;
            }
            await ReplyAsync(url);

        }

        [Command("dog", RunMode = RunMode.Async)]
        [Summary("Random dogs. mustpatmustpat")]
        [Alias("doge")]
        public async Task Dog()
        {
            Request req = await Helpers.Client.SendRequest("https://random.dog/woof");
            if (!req.Success)
            {
                await ReplyAsync("I-I couldn't fetch any dogs... I'm sorry ;-;");
                return;
            }
            await ReplyAsync($"https://random.dog/{req.Content}");
        }

        [Command("duck", RunMode = RunMode.Async)]
        [Summary("Positively Quack-y random ducks!")]
        public async Task Duck()
        {
            Request req = await Helpers.Client.SendRequest("https://random-d.uk/api/v1/quack");
            DuckRes res = JsonConvert.DeserializeObject<DuckRes>(req.Content);
            if (!req.Success)
            {
                await ReplyAsync("I-I couldn't find any ducks... I'm sorry ;-;");
                return;
            }
            await ReplyAsync(res.Url);
        }
    }
}

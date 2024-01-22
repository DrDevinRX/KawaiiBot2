using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
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
    public class ImageCmds : InteractionModuleBase
    {
        //Oh how I wish I had Julia's generated code...

        [SlashCommand("apod", "NASA's Astronomy Picture Of the Day", runMode: RunMode.Async)]
        public async Task APOD() => await RespondAsync(await Images.APOD());

        /*[SlashCommand("animal", "Random zoo animal.", runMode: RunMode.Async)]
        public async Task Animal() => await RespondAsync(await Images.Animal());*/

        [SlashCommand("bear", "Random bear.")]
        public Task Bear() => RespondAsync(Images.Bear());

        [SlashCommand("duck", "Positively Quack-y random ducks!", runMode: RunMode.Async)]
        public async Task Duck() => await RespondAsync(await Images.Duck());

        [SlashCommand("doge", "Random Shiba Inu", runMode: RunMode.Async)]
        public async Task Doge() => await RespondAsync(await Images.Doge());

        [SlashCommand("dog", "Random dogs. mustpatmustpat", runMode: RunMode.Async)]
        public async Task Dog() => await RespondAsync(await Images.Dog());

#if HAS_AXOLOTL
        [SlashCommand("axolotl", "Fetches Axolotl images. Cute!", runMode: RunMode.Async)]
        public async Task Axolotl() => await RespondAsync(await Images.Axolotl());
#endif

        [SlashCommand("coffee", "Coffee images to wake you up!", runMode: RunMode.Async)]
        public async Task Coffee() => await RespondAsync(await Images.Coffee());

        [SlashCommand("birb", "Cute birbs :2", runMode: RunMode.Async)]
        public async Task Birb() => await RespondAsync(await Images.Birb());

        [SlashCommand("guitarcat", "Guitar cat. That's cute.")]
        public Task GuitarCat() => RespondAsync(Images.GuitarCat());

        [SlashCommand("cat", "cats. Cats. CATS!", runMode: RunMode.Async)]
        public async Task Cat() => await RespondAsync(await Images.Cat());

        [SlashCommand("fox", "what does the...", runMode: RunMode.Async)]
        public async Task Fox() => await RespondAsync(await Images.Fox());
    }
}

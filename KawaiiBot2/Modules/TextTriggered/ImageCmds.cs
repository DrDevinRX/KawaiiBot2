using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.IO;
using Newtonsoft.Json;
using System.Security;
using KawaiiBot2.JSONClasses;
using System.Security.Cryptography;
using KawaiiBot2.Modules.Shared;

namespace KawaiiBot2.Modules.TextTriggered
{
    public class ImageCmds : ModuleBase<SocketCommandContext>
    {
        //Oh how I wish I had Julia's generated code...

        [Command("apod",  RunMode = RunMode.Async)]
        [Summary("NASA's Astronomy Picture Of the Day")]
        public async Task APOD() => await ReplyAsync(await Images.APOD());

        [Command("animal", RunMode = RunMode.Async)]
        [Summary("Random zoo animal.")]
        public async Task Animal() => await ReplyAsync(await Images.Animal());

        [Command("bear")]
        [Summary("Random bear.")]
        public Task Bear() => ReplyAsync(Images.Bear());

        [Command("duck", RunMode = RunMode.Async)]
        [Summary("Positively Quack-y random ducks!")]
        public async Task Duck() => await ReplyAsync(await Images.Duck());

        [Command("doge", RunMode = RunMode.Async)]
        [Alias("shiba", "shibe", "dopge", "doige", "dolge")]
        [Summary("Random Shiba Inu")]
        public async Task Doge() => await ReplyAsync(await Images.Doge());

        [Command("dog", RunMode = RunMode.Async)]
        [Alias("dawg", "doggo")]
        [Summary("Random dogs. mustpatmustpat")]
        public async Task Dog() => await ReplyAsync(await Images.Dog());

#if HAS_AXOLOTL
        [Command("axolotl", RunMode = RunMode.Async)]
        [Alias("axltl", "axoltl", "axlotl")]
        [Summary("Fetches Axolotl images. Cute!")]
        public async Task Axolotl() => await ReplyAsync(await Images.Axolotl());
#endif

        [Command("coffee", RunMode = RunMode.Async)]
        [Summary("Coffee images to wake you up!")]
        public async Task Coffee() => await ReplyAsync(await Images.Coffee());

        [Command("birb", RunMode = RunMode.Async)]
        [Alias("bird")]
        [Summary("Cute birbs :2")]
        public async Task Birb() => await ReplyAsync(await Images.Birb());

        [Command("guitarcat")]
        [HiddenCmd]
        [Summary("Guitar cat. That's cute.")]
        public Task GuitarCat() => ReplyAsync(Images.GuitarCat());

        [Command("cat", RunMode = RunMode.Async)]
        [Summary("cats. Cats. CATS!")]
        [Alias("catnotlewd", "caty", "chat")]//not guaranteed
        public async Task Cat() => await ReplyAsync(await Images.Cat());
    }
}

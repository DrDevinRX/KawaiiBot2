using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using KawaiiBot2.APIInterfacing;


namespace KawaiiBot2
{
    static class Helpers
    {
        private static readonly Random random = new Random();

        public static ulong[] devIDs = { 173529942431236096L, 132557773987643392L, 423574500513284098L };

        public static Client Client { get; set; } = null;

        //public static T ChooseRandom<T>(params T[] list)
        //    => list[random.Next(list.Length)];

        public static T ChooseRandom<T>(IEnumerable<T> items)
            => items.ElementAt(random.Next(items.Count()));

        public static string CleanGuildUserDisplayName(IGuildUser user)
            => (user?.Nickname ?? user?.Username ?? "User").Clean(); // If user is null, return User

        public static string GetName(IUser user)
        {
            return ((user as IGuildUser)?.Nickname ?? (user as IGuildUser)?.Username ?? user?.Username ?? "User").Clean();
        }


        public static Embed ImgStrEmbed(string imageUrl, string comment)
            => new EmbedBuilder().WithDescription(comment).WithImageUrl(imageUrl).Build();


        public static string Pad(string content, int padTo)
        {
            if (content.Length >= padTo)
                return content;
            return content + new string(' ', padTo - content.Length);
        }

    }
}

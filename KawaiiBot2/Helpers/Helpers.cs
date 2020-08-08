using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using KawaiiBot2.APIInterfacing;


namespace KawaiiBot2
{
    static class Helpers
    {
        private static readonly Random random = new Random();

        public static Client Client { get; private set; } = new Client("Awooo v2");

        public static T ChooseRandom<T>(params T[] list)
            => list[random.Next(list.Length)];

        public static T ChooseRandom<T>(IEnumerable<T> items)
            => items.ElementAt(random.Next(items.Count()));

        public static string CleanGuildUserDisplayName(IGuildUser user)
            => (user?.Nickname ?? user?.Username ?? "User").Clean(); // If user is null, return User

        public static Embed ImgStrEmbed(string imageUrl, string comment)
            => new EmbedBuilder().WithDescription(comment).WithImageUrl(imageUrl).Build();


    }
}

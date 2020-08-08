using System;
using System.Collections.Generic;
using System.Linq;
using Discord;

namespace KawaiiBot2.Helper
{
    class Helper
    {
        private static readonly Random random = new Random();

        public static T ChooseRandomItem<T>(IEnumerable<T> items) 
            => items.ElementAt(random.Next(items.Count()));

        public static string GuildUserNameOrNickName(IGuildUser user)
            => user.Nickname ?? user.Username ?? "User"; // If user is null, return User
    }
}

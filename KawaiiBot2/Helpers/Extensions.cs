using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Discord;

namespace KawaiiBot2
{
    static class Extensions
    {
        public static string Clean(this string str)
        {
            return str?.Replace("@", "@\u200b")?.Replace("`", "ˋ");
        }

        public static string GetEffectiveAvatarUrl(this IGuildUser user)
        {
            return user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();
        }

        public static string GetEffectiveAvatarUrl(this IUser user)
        {
            return user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();
        }
    }
}

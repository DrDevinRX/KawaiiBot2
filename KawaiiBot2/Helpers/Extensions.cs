using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace KawaiiBot2
{
    static class Extensions
    {
        public static string Clean(this string str)
        {
            return str?.Replace("@", "@\u200b")?.Replace("`", "ˋ");
        }
    }
}

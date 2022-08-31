using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KawaiiBot2.Modules.Shared
{
    internal static class PureText
    {
        readonly static IEnumerable<string> grunfacts = File.ReadLines("Resources/grunfacts.txt");
        internal static string GrunFacts() => Helpers.ChooseRandom(grunfacts);

        internal static string Paimon => "Paimon isn't emergency food!";

        internal static string TouchButt => "<:2Bgasm:358561754009042955>";

        internal static string WHA => "https://images-ext-2.discordapp.net/external/wamUisDomAcBplE2KewAgXl3yZaIockXyjz-9sXG2zo/https/media.tenor.com/pMkPqfRimeYAAAPo/shock-roxy-shock.mp4";
    }
}

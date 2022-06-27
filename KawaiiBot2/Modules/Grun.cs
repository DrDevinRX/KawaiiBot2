using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KawaiiBot2.Modules
{
    internal class Grun
    {
        readonly static IEnumerable<string> grunfacts = File.ReadLines("Resources/grunfacts.txt");
        internal static string GrunFacts() => Helpers.ChooseRandom(grunfacts);

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KawaiiBot2.JSONClasses
{
    class RPSJson
    {
        [JsonProperty("options")]
        public string[] Options { get; set; }


        [JsonProperty("table")]
        public Dictionary<string, WinLoseLists> Table { get; set; }
    }

    public class WinLoseLists
    {
        [JsonProperty("beats")]
        public string[] Beats { get; set; }
    }
}

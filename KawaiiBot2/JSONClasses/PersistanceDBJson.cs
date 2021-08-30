using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using KawaiiBot2.Modules;

namespace KawaiiBot2.JSONClasses
{
    public class PersistanceDBJson
    {

        [JsonProperty("saveNumber")]
        public uint SaveNumber { get; set; }

        [JsonProperty("slots")]
        public SlotsPersistanceJson Slots { get; set; }

        [JsonProperty("commandCounter")]
        public PopularityPersistanceJson CommandCounter { get; set; }
    }

    public class SlotsPersistanceJson
    {
        [JsonProperty("global")]
        public SlotsUserData Global { get; set; }

        [JsonProperty("userData")]
        public Dictionary<ulong, SlotsUserData> UserData { get; set; }
    }

    public class PopularityPersistanceJson
    {
        [JsonProperty("commandCount")]
        public Dictionary<string, int> CommandCount { get; set; }
    }
}

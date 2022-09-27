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

        [JsonProperty("totalUptime")]
        public TimeSpan TotalUptime { get; set; }

        [JsonProperty("slots")]
        public SlotsPersistanceJson Slots { get; set; }

        [JsonProperty("commandCounter")]
        public PopularityPersistanceJson CommandCounter { get; set; }

        [JsonProperty("otherRiggables")]
        public OtherRiggablesPersistanceJson OtherRiggables { get; set; }

        [JsonProperty("management")]
        public ManagementPersistanceJson Management { get; set; }

        [JsonProperty("noUse")]
        [Obsolete("literally just here for backwards compat. Will remove sometime in the future.")]
        public Dictionary<ulong, string[]> NoUse { get; set; }
    }

    public class ManagementPersistanceJson
    {
        [JsonProperty("devs")]
        public ulong[] Devs { get; set; }

        [JsonProperty("noUse")]
        public Dictionary<ulong, string[]> NoUse { get; set; }

    }

    public class SlotsPersistanceJson
    {
        [JsonProperty("global")]
        public SlotsUserData Global { get; set; }

        [JsonProperty("userData")]
        public Dictionary<ulong, SlotsUserData> UserData { get; set; }

        [JsonProperty("protocolcc2")]
        public string ProtocolCC2 { get; set; }

        [JsonProperty("totalIconsRolled")]
        public int TotalIconsRolled { get; set; }

        [JsonProperty("sidesData")]
        public SlotsSidesData SidesData { get; set; }
    }

    public class SlotsSidesData
    {
        [JsonProperty("leftDoubles")]
        public int LeftDoubles { get; set; }

        [JsonProperty("rightDoubles")]
        public int RightDoubles { get; set; }

        [JsonProperty("sidesDoubles")]
        public int SidesDoubles { get; set; }
    }

    public class PopularityPersistanceJson
    {
        [JsonProperty("commandCount")]
        public Dictionary<string, int> CommandCount { get; set; }
    }

    public class OtherRiggablesPersistanceJson
    {
        [JsonProperty("rigChoose")]
        public string RigChoose { get; set; }

        [JsonProperty("yesHey")]
        public bool YesHey { get; set; }

        [JsonProperty("alwaysHey")]
        public bool AlwaysHey { get; set; }
    }
}

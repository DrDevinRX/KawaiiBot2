using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace KawaiiBot2.JSONClasses
{
    public partial class ConfJson
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("catApiToken")]
        public string CatApiToken { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("devIDs")]
        public ulong[] DevIDs { get; set; }

        [JsonProperty("startStatus")]
        public string StartStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KawaiiBot2.JSONClasses
{
    public class ThrowJson
    {
        [JsonProperty("targetQuotes")]
        public string[] TargetQuotes { get; set; }

        [JsonProperty("authorQuotes")]
        public string[] AuthorQuotes { get; set; }

        [JsonProperty("items")]
        public string[] Items { get; set; }
    }
}

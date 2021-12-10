using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{
    public class AxolotlRes
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("facts")]
        public string Facts { get; set; }

        [JsonProperty("pics_repo")]
        public Uri PicsRepo { get; set; }

        [JsonProperty("api_repo")]
        public Uri ApiRepo { get; set; }
    }
}

using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{
    public class FoxRes
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}

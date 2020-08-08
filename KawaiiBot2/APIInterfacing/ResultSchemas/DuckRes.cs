using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{
    public class DuckRes
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

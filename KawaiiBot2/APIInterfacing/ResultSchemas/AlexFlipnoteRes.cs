using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{
    public class AlexFlipnoteRes
    {
        [JsonProperty("file")]
        public string File { get; set; }
    }
}

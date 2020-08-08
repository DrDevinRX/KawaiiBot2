using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{

    public class NekosLifeRes
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("response")]
        public Response Response { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }

    public partial class Response
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("rendered_in")]
        public string RenderedIn { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

}

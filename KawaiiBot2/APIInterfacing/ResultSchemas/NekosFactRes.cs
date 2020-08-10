using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{
    public class NekosFactRes
    {
        [JsonProperty("fact")]
        public string Fact { get; set; }
    }
}

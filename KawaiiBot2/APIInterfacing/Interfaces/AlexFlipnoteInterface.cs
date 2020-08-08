using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using KawaiiBot2.APIInterfacing.ResultSchemas;

namespace KawaiiBot2.APIInterfacing.Interfaces
{
    static class AlexFlipnoteInterface
    {
        static AlexFlipnoteInterface()
        {
            mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Resources/AFPEndpoints.json"));
        }

        private static Dictionary<string, string> mapping = new Dictionary<string, string>();

        public static async Task<(bool, string)> TryGetEndpoint(string plainName)
        {
            Request req = await Helpers.Client.SendRequest(mapping[plainName]);
            AlexFlipnoteRes res = JsonConvert.DeserializeObject<AlexFlipnoteRes>(req.Content);
            var url = req.Success ? res.File : null;
            return (req.Success, url);
        }

    }
}

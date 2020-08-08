using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using KawaiiBot2.APIInterfacing.ResultSchemas;

namespace KawaiiBot2.APIInterfacing.Interfaces
{
    static class NekosLifeInterface
    {
        static NekosLifeInterface()
        {
            mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Resources/NekosEndpoints.json"));
            baseURL = mapping["baseURL"];
        }

        private static string baseURL;

        private static Dictionary<string, string> mapping = new Dictionary<string, string>();


        public static async Task<(bool, string)> TryGetEndpoint(string plainName)
        {
            Request req = await Helpers.Client.SendRequest(baseURL + mapping[plainName]);
            NekosLifeRes res = JsonConvert.DeserializeObject<NekosLifeRes>(req.Content);
            var url = req.Success ? res.Data.Response.Url : null;
            return (req.Success, url);

        }
    }
}

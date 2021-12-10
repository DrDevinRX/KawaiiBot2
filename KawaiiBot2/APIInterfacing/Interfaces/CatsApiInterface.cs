using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KawaiiBot2.APIInterfacing.ResultSchemas;
using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.Interfaces
{
    class CatsApiInterface
    {
        public static string ApiKey { get; set; } = "";

        public static async Task<(bool, string)> GetCat()
        {
            var url = $"https://api.thecatapi.com/v1/images/search?mime_types=jpg,png&limit=1&size=medium&api_key={ApiKey}";
            Request req = await Helpers.Client.SendRequest(url);
            CatApiRes res = JsonConvert.DeserializeObject<CatApiRes[]>(req.Content)[0];
            var spiturl = req.Success ? res.Url : null;
            return (req.Success, spiturl);

        }
    }
}

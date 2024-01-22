using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KawaiiBot2.APIInterfacing.Interfaces;
using KawaiiBot2.APIInterfacing;
using KawaiiBot2.APIInterfacing.ResultSchemas;
using System.IO;
using Newtonsoft.Json;

namespace KawaiiBot2.Modules.Shared
{
    internal static class Images
    {
        public static string NASAApiKey { get; set; }

        public static async Task<string> APOD()
        {
            Request req = await Helpers.Client.SendRequest($"https://api.nasa.gov/planetary/apod?api_key={NASAApiKey}");
            ApodRes res = JsonConvert.DeserializeObject<ApodRes>(req.Content);
            return req.Success ? res.Url.ToString() : "Unavailable.";
        }

        public static async Task<string> Animal()
        {
            Request req = await Helpers.Client.SendRequest("https://zoo-animal-api.herokuapp.com/animals/rand");
            ZooAnimalRes res = JsonConvert.DeserializeObject<ZooAnimalRes>(req.Content);
            return req.Success ? res.ImageLink.ToString() : "Can't fetch.";
        }

        readonly static Random rand = new Random();

        public static string Bear()
        {
            return $"https://placebear.com/10{rand.Next(100):d2}/11{rand.Next(100):d2}.jpg";
        }

        public static async Task<string> Duck()
        {
            Request req = await Helpers.Client.SendRequest("https://random-d.uk/api/v1/quack");
            DuckRes res = JsonConvert.DeserializeObject<DuckRes>(req.Content);
            return req.Success ? res.Url : "I-I couldn't find any ducks... I'm sorry ;-;";
        }

        public static async Task<string> Doge()
        {
            Request req = await Helpers.Client.SendRequest("https://shibe.online/api/shibes?count=1&urls=true&httpsUrls=true");
            return req.Success ? $"{JsonConvert.DeserializeObject<string[]>(req.Content)[0]}" : "Lack doge";
        }

        public static async Task<string> Dog()
        {
            Request req = await Helpers.Client.SendRequest("https://random.dog/woof");
            return req.Success ? $"https://random.dog/{req.Content}" : "I-I couldn't fetch any dogs... I'm sorry ;-;";
        }


#if HAS_AXOLOTL
        public static async Task<string> Axolotl()
        {
            Request req = await Helpers.Client.SendRequest("https://axoltlapi.herokuapp.com/");
            AxolotlRes res = JsonConvert.DeserializeObject<AxolotlRes>(req.Content);
            return req.Success ? res.Url : "N-No axolotls;;;";
        }
#endif

        public static async Task<string> Coffee()
        {
            (bool success, string url) = await AlexFlipnoteInterface.TryGetEndpoint("coffee");
            return success ? url : "I-I couldn't find any coffee... I'm sorry ;-;";
        }

        public static async Task<string> Birb()
        {
            (bool success, string url) = await AlexFlipnoteInterface.TryGetEndpoint("birb");
            return success ? url : "I-I couldn't find any birbs... I'm sorry ;-;";
        }

        public static string GuitarCat()
            => "https://cdn.nekos.life/v3/sfw/img/cat/cat_1267.jpg";

        public static async Task<string> Cat()
        {
            (bool success, string url) = await CatsApiInterface.GetCat();
            return success ? url : "https://http.cat/404.jpg";
        }

        public static async Task<string> Fox()
        {
            Request req = await Helpers.Client.SendRequest("https://randomfox.ca/floof/");
            FoxRes res = JsonConvert.DeserializeObject<FoxRes>(req.Content);
            return req.Success ? res.Image : "No foxes ;-;";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{//https://zoo-animal-api.herokuapp.com/
    public class ZooAnimalRes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latin_name")]
        public string LatinName { get; set; }

        [JsonProperty("animal_type")]
        public string AnimalType { get; set; }

        [JsonProperty("active_time")]
        public string ActiveTime { get; set; }

        [JsonProperty("length_min")]
        public string LengthMin { get; set; }

        [JsonProperty("length_max")]
        public string LengthMax { get; set; }

        [JsonProperty("weight_min")]
        public string WeightMin { get; set; }

        [JsonProperty("weight_max")]
        public string WeightMax { get; set; }

        [JsonProperty("lifespan")]
        public string Lifespan { get; set; }

        [JsonProperty("habitat")]
        public string Habitat { get; set; }

        [JsonProperty("diet")]
        public string Diet { get; set; }

        [JsonProperty("geo_range")]
        public string GeoRange { get; set; }

        [JsonProperty("image_link")]
        public Uri ImageLink { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}

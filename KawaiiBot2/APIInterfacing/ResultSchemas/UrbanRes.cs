using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KawaiiBot2.APIInterfacing.ResultSchemas
{


    public class UrbanRes
    {
        [JsonProperty("list")]
        public ListItem[] List { get; set; }
    }

    public class ListItem
    {
        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("permalink")]
        public Uri Permalink { get; set; }

        [JsonProperty("thumbs_up")]
        public long ThumbsUp { get; set; }

        [JsonProperty("sound_urls")]
        public object[] SoundUrls { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }

        [JsonProperty("defid")]
        public long Defid { get; set; }

        [JsonProperty("current_vote")]
        public string CurrentVote { get; set; }

        [JsonProperty("written_on")]
        public DateTimeOffset WrittenOn { get; set; }

        [JsonProperty("example")]
        public string Example { get; set; }

        [JsonProperty("thumbs_down")]
        public long ThumbsDown { get; set; }
    }

}

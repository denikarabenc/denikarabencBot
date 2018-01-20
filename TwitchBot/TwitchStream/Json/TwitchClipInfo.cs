using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.TwitchStream.Json
{
    public class TwitchClipInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("edit_url")]
        public string EditURL { get; set; }
    }

    public class TwitchClipInfoJsonRootObject
    {
        [JsonProperty("data")]
        public List<TwitchClipInfo> Data { get; set; }
    }
}

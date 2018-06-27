using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.TwitchStream.Json
{
    public class TwitchChannelInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }

    public class TwitchChannelInfoRoot
    {
        [JsonProperty("data")]
        public List<TwitchChannelInfo> Data { get; set; }
    }
}

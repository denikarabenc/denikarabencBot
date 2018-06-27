using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.TwitchStream.Json
{
    public class TwitchGamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TwitchTopGamesInfo
    {
        [JsonProperty("game")]
        public TwitchGamesInfo Game { get; set; }
    }

    public class TwitchGamesJsonRootObject
    {
        [JsonProperty("_total")]
        public long Total { get; set; }
        [JsonProperty("top")]
        public List<TwitchTopGamesInfo> Top { get; set; }
    }
}

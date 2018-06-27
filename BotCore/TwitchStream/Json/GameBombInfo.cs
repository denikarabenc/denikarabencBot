using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.TwitchStream.Json
{
    public class GameBombInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }   

    public class GameBombInfoJsonRootObject
    {
        [JsonProperty("results")]
        public List<GameBombInfo> Results { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Steam.Json
{
    internal class SteamGameInfo
    {
        [JsonProperty("name")]
        public string GameName { get; set; }
    }

    internal class SteamGameData
    {
        [JsonProperty("data")]
        public SteamGameInfo GameData { get; set; }
    }

    //internal class SteamGameJsonRootObject
    //{
    //    public SteamGameData Game { get; set; }
    //}
}
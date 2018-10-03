using Newtonsoft.Json;

namespace BotCore.Steam.Json
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
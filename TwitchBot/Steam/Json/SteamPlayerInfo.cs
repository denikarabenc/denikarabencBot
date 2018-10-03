using Newtonsoft.Json;
using System.Collections.Generic;

namespace BotCore.Steam.Json
{
    internal class SteamPlayerInfo
    {
        [JsonProperty("steamid")]
        public string SteamID { get; set; }

        [JsonProperty("communityvisibilitystate")]
        public string CommunityVisibilityState { get; set; }

        [JsonProperty("profilestate")]
        public string ProfileState { get; set; }

        [JsonProperty("personaname")]
        public string PersonaName { get; set; }

        [JsonProperty("lastlogoff")]
        public string LastLogOff { get; set; }

        [JsonProperty("profileurl")]
        public string ProfilEurl { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("avatarmedium")]
        public string AvatarMedium { get; set; }

        [JsonProperty("avatarfull")]
        public string AvatarFull { get; set; }

        [JsonProperty("loccountrycode")]
        public string LocCountryCode { get; set; }

        [JsonProperty("primaryclanid")]
        public string PrimaryClanID { get; set; }

        [JsonProperty("realname")]
        public string RealName { get; set; }

        [JsonProperty("personastateflags")]
        public string PersonastateFlags { get; set; }

        [JsonProperty("timecreated")]
        public string TimeCreated { get; set; }

        [JsonProperty("personastate")]
        public string PersonaState { get; set; }

        [JsonProperty("gameextrainfo")]
        public string GameExtraInfo { get; set; }

        [JsonProperty("gameid")]
        public string GameID { get; set; }
    }

    internal class SteamJsonRootObject
    {
        [JsonProperty("response")]
        public Player Response { get; set; }
    }

    internal class Player
    {
        [JsonProperty("players")]
        public List<SteamPlayerInfo> Players { get; set; }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using TwitchBot.Steam.Json;

namespace TwitchBot.Steam
{
    public class SteamInfoProvider
    {
        private SteamJsonRootObject jsonResult;
        private string playerName;
        private string gameName;
        private string steamID;

        public SteamInfoProvider(string steamID)
        {
            this.steamID = steamID;
            UpdateSteamInfo();
        }

        public string PlayerName { get => playerName; set => playerName = value; }
        public string GameName { get => gameName; set => gameName = value; }

        public void UpdateSteamInfo()
        {
            string json = GetSteamJson();

            jsonResult = JsonConvert.DeserializeObject<SteamJsonRootObject>(json);

            PlayerName = jsonResult?.Response?.Players[0]?.PersonaName;
            GameName = jsonResult?.Response?.Players[0]?.GameExtraInfo;
        }

        private string GetSteamJson()
        {
            string json;
            using (var webClient = new WebClient())
            {
                try
                {
                    json = webClient.DownloadString("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=B4139755162C446B3D350BAF8D8B0EDC&steamids=" + steamID);
                }
                catch (WebException)
                {
                    json = string.Empty;
                }
            }

            return json;
        }

        public List<string> GetSteamGamesWhichWouldNotBeChangedTo()
        {
            List<string> list = new List<string>();
            list.Add("Wallpaper Engine");

            return list;
        }
    }
}

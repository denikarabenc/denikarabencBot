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
        private string gameID;

        private Dictionary<string, string> gameToTwitchMapper;

        public SteamInfoProvider(string steamID)
        {
            this.steamID = steamID;
            gameToTwitchMapper = SteamGamesDLCMapper();
            UpdateSteamInfo();
        }

        public string PlayerName { get => playerName; set => playerName = value; }
        public string GameName { get => gameName; set => gameName = value; }

        public void UpdateSteamInfo()
        {
            string json = GetSteamPlayerJson();

            jsonResult = JsonConvert.DeserializeObject<SteamJsonRootObject>(json);

            if (jsonResult?.Response?.Players.Count != 0)
            {
                PlayerName = jsonResult?.Response?.Players[0]?.PersonaName;
                GameName = jsonResult?.Response?.Players[0]?.GameExtraInfo;
            }

            //if (GameName == null)
            //{
            //    gameID = jsonResult?.Response?.Players[0]?.GameID;
            //    if (gameID == null)
            //    {
            //        return;
            //    }

            //    string jsonGame = GetSteamGameJson(gameID);

            //    SteamGameJsonRootObject jsonGameResult = JsonConvert.DeserializeObject<SteamGameJsonRootObject>(jsonGame);

            //    GameName = jsonGameResult?.Game?.GameName;
            //}

            if (GameName == null)
            {
                gameID = jsonResult?.Response?.Players[0]?.GameID;
                if (gameID == null)
                {
                    return;
                }

                string jsonGame = GetSteamGameJson(gameID);

                jsonGame = jsonGame.Substring(jsonGame.IndexOf(":") + 1);
                jsonGame = jsonGame.Remove(jsonGame.Length - 1);

                SteamGameData jsonGameResult = JsonConvert.DeserializeObject<SteamGameData>(jsonGame);

                if (gameToTwitchMapper.ContainsKey(jsonGameResult?.GameData?.GameName))
                {
                    GameName = gameToTwitchMapper[jsonGameResult.GameData.GameName];              
                }
                else
                {
                    GameName = jsonGameResult?.GameData?.GameName;                    
                }
            }
        }

        private string GetSteamPlayerJson()
        {
            string json;
            using (var webClient = new WebClient())
            {
                try
                {
                    json = webClient.DownloadString("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=B4139755162C446B3D350BAF8D8B0EDC&steamids=" + steamID);
                }
                catch (WebException ex)
                {
                    BotLogger.Logger.Log(Common.Models.LoggingType.Warning, ex);
                    json = string.Empty;
                }
            }

            return json;
        }

        private string GetSteamGameJson(string gameId)
        {
            string json;
            using (var webClient = new WebClient())
            {
                try
                {
                    //json = webClient.DownloadString("http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?key=B4139755162C446B3D350BAF8D8B0EDC&appid=" + gameId);
                    json = webClient.DownloadString("http://store.steampowered.com/api/appdetails?appids=" + gameId);
                }
                catch (WebException ex)
                {
                    BotLogger.Logger.Log(Common.Models.LoggingType.Warning, ex);
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

        private Dictionary<string, string> SteamGamesDLCMapper()
        {
            Dictionary<string, string> mapper = new Dictionary<string, string>();

            mapper.Add("The Binding of Isaac: Rebirth", "The Binding of Isaac: Afterbirth");
            mapper.Add("Tom Clancy's Rainbow Six® Siege", "Tom Clancy's Rainbow Six: Siege");
            mapper.Add("PUBG: Closed Experimental Server", "PLAYERUNKNOWN'S BATTLEGROUNDS");
            mapper.Add("Nioh: Complete Edition", "Nioh");

            return mapper;
        }
    }
}

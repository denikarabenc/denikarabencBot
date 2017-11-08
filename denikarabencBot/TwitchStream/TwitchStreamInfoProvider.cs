using BotLogger;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TwitchBot1.TwitchStream.Json;

namespace TwitchBot1.TwitchStream
{
    public class TwitchStreamInfoProvider
    {
        private List<TwitchGame> gamesPlayed;
        private string channelName;

        public TwitchStreamInfoProvider(string channelName)
        {
            this.channelName = channelName;
            gamesPlayed = new List<TwitchGame>();
        }

        public List<TwitchGame> GamesPlayed => gamesPlayed;

        private TwitchJsonRootObject GetTwitchStatus()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + channelName + "?client_id=fdl7tng741x3oys8g5ohh5s6z1zsrr"); //26213337760
                request.Method = "GET";
                request.Headers["Authorization"] = $"OAuth vv0yeswj1kpcmyvi381006bl7rxaj4";
                request.ContentType = "application/json";
                request.Accept = $"application/vnd.twitchtv.v3+json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                TwitchJsonRootObject jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    jsonResult = JsonConvert.DeserializeObject<TwitchJsonRootObject>(jsonString);
                    if (jsonResult.Stream == null)
                    {
                        jsonResult.Stream = JsonConvert.DeserializeObject<TwitchStreamInfo>(jsonString);
                    }
                }

                return jsonResult;
            }
            catch (WebException ex)
            {
                Logger.Log("[TwitchStreamInfoProvider] -> ", ex);
                return new TwitchJsonRootObject();
            }
        }

        public void AddPlayingGame(string game = null)
        {
            string currentGame = null;
            if (game != null)
            {
                currentGame = game;
            }

            if (currentGame != null)
            {
                TwitchGame tg = new TwitchGame();

                if (gamesPlayed.Count == 0)
                {
                    tg.GameName = currentGame;
                    tg.TimePlayed.Start();
                    gamesPlayed.Add(tg);
                    return;
                }

                if (gamesPlayed.Count > 0 && currentGame != gamesPlayed[gamesPlayed.Count - 1].GameName)
                {
                    gamesPlayed[gamesPlayed.Count - 1].TimePlayed.Stop();

                    tg.GameName = currentGame;
                    tg.TimePlayed.Start();
                    gamesPlayed.Add(tg);
                }
            }
        }

        public string GetCurrentTwitchGame()
        {
            TwitchJsonRootObject twitchJsonRootObject = GetTwitchStatus();
            if (twitchJsonRootObject.Stream != null)
            {
                return twitchJsonRootObject.Stream.Game;
            }
            return null;
        }

        public string GetTitle()
        {
            TwitchJsonRootObject twitchJsonRootObject = GetTwitchStatus();
            if (twitchJsonRootObject.Stream != null)
            {
                return twitchJsonRootObject.Stream.Status;
            }
            return null;
        }  

        public List<string> GetTwitchGamesWhichWouldNotBeChanged()
        {
            List<string> list = new List<string>();
            list.Add("Music");
            list.Add("Creative");
            list.Add("IRL");
            list.Add("Talk Shows");
            list.Add("Casino");
            list.Add("Social Eating");
            list.Add("Twitch Plays");

            return list;
        }
    }
}

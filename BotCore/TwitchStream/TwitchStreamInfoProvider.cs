using BotLogger;
using Common.Interfaces;
using Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TwitchBot.TwitchStream.Json;

namespace TwitchBot.TwitchStream
{
    public class TwitchStreamInfoProvider : IStreamInfoProvider
    {
        private List<StreamGame> gamesPlayed;
        private string channelName;

        public TwitchStreamInfoProvider(string channelName)
        {
            this.channelName = channelName;
            gamesPlayed = new List<StreamGame>();

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/users/" + channelName);
            //request.Method = "GET";
            //request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967";
            //request.ContentType = "application/json";
            //request.Accept = $"application/vnd.twitchtv.v3+json";

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //using (var reader = new StreamReader(response.GetResponseStream()))
            //{
            //    string jsonString = reader.ReadToEnd();
            //}
        }

        public List<StreamGame> GamesPlayed => gamesPlayed;

        private TwitchJsonRootObject GetStreamStatus()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + channelName + "?client_id=fdl7tng741x3oys8g5ohh5s6z1zsrr");
                request.Method = "GET";
                request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967";
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
                Logger.Log(LoggingType.Warning, "[TwitchStreamInfoProvider] -> ", ex);
                return new TwitchJsonRootObject();
            }

            //This works only if online. And json needs to be adjusted
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/helix/streams?user_id=31999722");
        //        request.Method = "GET";
        //        request.Headers["Authorization"] = $"Bearer zbhu1ji38wte5ovbnt785fg67hj9ay";
        //        request.Headers["Client-ID"] = $"fdl7tng741x3oys8g5ohh5s6z1zsrr";

        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //        TwitchJsonRootObject jsonResult;
        //        using (var reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            string jsonString = reader.ReadToEnd();
        //            jsonResult = JsonConvert.DeserializeObject<TwitchJsonRootObject>(jsonString);
        //            if (jsonResult.Stream == null)
        //            {
        //                jsonResult.Stream = JsonConvert.DeserializeObject<TwitchStreamInfo>(jsonString);
        //            }
        //        }

        //        return jsonResult;
        //    }
        //    catch (WebException ex)
        //    {
        //        Logger.Log(LoggingType.Warning, "[TwitchStreamInfoProvider] -> ", ex);
        //        return new TwitchJsonRootObject();
        //    }
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
                StreamGame tg = new StreamGame();

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

        public string GetCurrentStreamGame()
        {
            TwitchJsonRootObject twitchJsonRootObject = GetStreamStatus();
            if (twitchJsonRootObject.Stream != null)
            {
                return twitchJsonRootObject.Stream.Game;
            }
            return null;
        }

        public string GetTitle()
        {
            TwitchJsonRootObject twitchJsonRootObject = GetStreamStatus();
            if (twitchJsonRootObject.Stream != null)
            {
                return twitchJsonRootObject.Stream.Status;
            }
            return null;
        }  

        public IList<string> GetStreamGamesWhichWouldNotBeChanged()
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

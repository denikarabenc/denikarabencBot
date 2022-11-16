using BotLogger;
using Common.Helpers;
using Common.Interfaces;
using Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TwitchBot.TwitchStream.Json;

namespace TwitchBot.TwitchStream
{
    public class TwitchStreamUpdater : IStreamUpdater
    {
        private string channelId;
        public TwitchStreamUpdater(string channelId)
        {
            channelId.ThrowIfNull(nameof(channelId));
            this.channelId = channelId;
        }

        public string SetStreamGameAndReturnWhichGameIsSet(string game) //TODO -> switch to V5
        {
            game.ThrowIfNull(nameof(game));

            string jsonData = "{\"channel\":{\"game\":\"" + game + "\"}}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + channelId);
                request.Method = "PUT";
                request.Headers["Client-ID"] = $"fdl7tng741x3oys8g5ohh5s6z1zsrr";
                request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967";
                request.ContentType = "application/json";
                request.Accept = $"application/vnd.twitchtv.v5+json";

                if (jsonData != null)
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(jsonData);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                TwitchStreamInfo jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<TwitchStreamInfo>(jsonString);
                }
                if (jsonResult != null && jsonResult.Game == game)
                {
                    return "Game successfully changed to " + game;
                }

                return "Game change failed FeelsBadMan";
            }
            catch (WebException ex)
            {
                Logger.Log(LoggingType.Error, "[TwitchStreamUpdater] -> ", ex);

                if (ex.Message == "The remote server returned an error: (500) Internal Server Error.")
                {
                    return string.Format("Twitch API is having some issues, game may change to a {0} in a minute", game);
                }

                return "Game change failed FeelsBadMan";
            }
        }

        public string SetStreamStatusAndReturnWhichStatusIsSet(string status) //TODO -> switch to V5
        {
            status.ThrowIfNull(nameof(status));

            string jsonData = "{\"channel\":{\"status\":\"" + status + "\"}}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + channelId);
                request.Method = "PUT";
                request.Headers["Client-ID"] = $"fdl7tng741x3oys8g5ohh5s6z1zsrr"; //TODO make safe read of client ID. This is placeholder clientID
                request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967"; //TODO make safe read of auth. This is placeholder OAuth key
                request.ContentType = "application/json";
                request.Accept = $"application/vnd.twitchtv.v5+json";


                if (jsonData != null)
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(jsonData);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                TwitchStreamInfo jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<TwitchStreamInfo>(jsonString);
                }
                if (jsonResult != null && jsonResult.Channel != null && jsonResult.Channel.Title == status)
                {
                    return status;
                }

                // return "Could not get feedback. Title might be changed.";
                return status;
            }
            catch (WebException ex)
            {
                Logger.Log(LoggingType.Error, "[TwitchStreamUpdater] -> ", ex);

                if (ex.Message == "The remote server returned an error: (500) Internal Server Error.")
                {
                    return "Twitch API is having some issues, title may change in a minute";
                }

                return "Nothing, title change failed FeelsBadMan";
            }

        }
    }
}
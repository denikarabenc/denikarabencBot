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
        private string channelName;        
        public TwitchStreamUpdater(string channelName)
        {
            this.channelName = channelName;
        }

        public string SetStreamGameAndReturnWhichGameIsSet(string game) //TODO -> switch to V5
        {
            game.ThrowIfNull(nameof(game));
            List<KeyValuePair<string, string>> datas = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(game))
                datas.Add(new KeyValuePair<string, string>("game", "\"" + game + "\""));

            string payload = "";
            if (datas.Count == 1)
            {
                payload = $"\"{datas[0].Key}\": {datas[0].Value}";
            }
            else
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    if ((datas.Count - i) > 1)
                        payload = $"{payload}\"{datas[i].Key}\": {datas[i].Value},";
                    else
                        payload = $"{payload}\"{datas[i].Key}\": {datas[i].Value}";
                }
            }

            payload = "{ \"channel\": {" + payload + "} }";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + channelName + "?client_id=fdl7tng741x3oys8g5ohh5s6z1zsrr");
                request.Method = "PUT";
                request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967";
                request.ContentType = "application/json";
                request.Accept = $"application/vnd.twitchtv.v3+json";

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/helix/channels/" + channelName);
                //request.Method = "PUT";
                //request.Headers["Authorization"] = $"Bearer agjzfjjarinmxy46lc9zzae9r4e967";
                ////request.Headers["Authorization"] = $"Bearer zbhu1ji38wte5ovbnt785fg67hj9ay";
                //request.Headers["Client-ID"] = $"fdl7tng741x3oys8g5ohh5s6z1zsrr";

                if (payload != null)
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(payload);
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
                    return "Twitch API is having some issues, game may change in a minute";
                }

                return "Game change failed FeelsBadMan";
            }
        }

        public string SetStreamStatusAndReturnWhichStatusIsSet(string status) //TODO -> switch to V5
        {
            status.ThrowIfNull(nameof(status));
            List<KeyValuePair<string, string>> datas = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(status))
                datas.Add(new KeyValuePair<string, string>("status", "\"" + status + "\""));

            string payload = "";

            if (datas.Count == 1)
            {
                payload = $"\"{datas[0].Key}\": {datas[0].Value}";
            }

            payload = "{ \"channel\": {" + payload + "} }";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + channelName + "?client_id=fdl7tng741x3oys8g5ohh5s6z1zsrr");
                request.Method = "PUT";
                request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967"; //vv0yeswj1kpcmyvi381006bl7rxaj4
                request.ContentType = "application/json";
                request.Accept = $"application/vnd.twitchtv.v3+json";

                if (payload != null)
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(payload);
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


//client secret 8k2yh6aaj86t39guooqc2uiz695xz9
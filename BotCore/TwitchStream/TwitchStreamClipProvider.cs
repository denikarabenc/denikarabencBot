using BotLogger;
using Common.Helpers;
using Common.Interfaces;
using Common.Models;
using System;
using System.IO;
using System.Net;
using TwitchBot.TwitchStream.Json;
using Newtonsoft.Json;

namespace TwitchBot.TwitchStream
{
    public class TwitchStreamClipProvider : IStreamClipProvider
    {
        private string channelName;
        private readonly string streamerID;
        public TwitchStreamClipProvider(string channelName)
        {
            channelName.ThrowIfNull(nameof(channelName));
            this.channelName = channelName;
            streamerID = GetChannelId(channelName);
            if (string.IsNullOrEmpty(streamerID))
            {
                BotLogger.Logger.Log(LoggingType.Warning, "[TwitchStreamClipProvider] => streamerID is null!");
                //TODO Sta ako pukne ovde nalazenje, treba ponovo pokusati da pronadje
            }
        }

        public string CreateClip()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/helix/clips?broadcaster_id=" + streamerID);
                request.Method = "POST";
                request.Headers["Authorization"] = $"Bearer zbhu1ji38wte5ovbnt785fg67hj9ay";
                request.Headers["Client-ID"] = $"fdl7tng741x3oys8g5ohh5s6z1zsrr";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                TwitchClipInfoJsonRootObject jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    jsonResult = JsonConvert.DeserializeObject<TwitchClipInfoJsonRootObject>(jsonString);

                    return jsonResult.Data[0].ID;
                }

            }
            catch (WebException ex)
            {
                Logger.Log(LoggingType.Warning, "[TwtichStreamClipProvider] -> ", ex);
                return "Failed to generate clip FeelsBadMan";
            }
        }

        private string GetChannelId(string channelName) //TODO prebaci u TwitchCommonHelper
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/helix/users?login=" + channelName);
                request.Method = "GET";
                request.Headers["Authorization"] = $"Bearer zbhu1ji38wte5ovbnt785fg67hj9ay";
                request.Headers["Client-ID"] = $"fdl7tng741x3oys8g5ohh5s6z1zsrr";
                //request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                TwitchChannelInfoRoot jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    jsonResult = JsonConvert.DeserializeObject<TwitchChannelInfoRoot>(jsonString);

                    return jsonResult.Data[0].ID;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingType.Error, "[TwtichStreamClipProvider] -> Failed to get channel ID!", ex);
                return String.Empty;
            }
        }
    }
}

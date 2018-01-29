using BotLogger;
using Common.Helpers;
using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.TwitchStream.Json;

namespace TwitchBot.TwitchStream
{
    public class TwitchStreamClipProvider
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

        public string CreateTwitchClip()
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/users/" + channelName);
                request.Method = "GET";
                request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967";
                request.ContentType = "application/json";
                request.Accept = $"application/vnd.twitchtv.v3+json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                TwitchChannelInfo jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    jsonResult = JsonConvert.DeserializeObject<TwitchChannelInfo>(jsonString);

                    return jsonResult.ID;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(LoggingType.Error, "[TwtichStreamClipProvider] -> Failed to get channel ID!", ex);
                return String.Empty;
            }
        }
    }
}

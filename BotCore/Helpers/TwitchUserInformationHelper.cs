using BotLogger;
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

namespace TwitchIntegration.Helpers
{
    public static class TwitchUserInformationHelper
    {
        public static string GetChannelId(string channelName) //TODO extract to TwitchCommonHelper
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
                Logger.Log(LoggingType.Error, "[TwitchUserInformationHelper] -> Failed to get channel ID!");
                throw ex;
            }
        }
    }
}

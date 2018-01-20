using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.TwitchStream.Json
{
    public class TwitchChannelInfo
    {
        [JsonProperty("_id")]
        public string ID { get; set; }
    }    
}

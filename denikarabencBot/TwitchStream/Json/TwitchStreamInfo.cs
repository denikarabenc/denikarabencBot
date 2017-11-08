using Newtonsoft.Json;

namespace TwitchBot1.TwitchStream.Json
{
    public class TwitchStreamInfo
    {
        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("channel")]
        public TwtichChannelInfo Channel { get; set; }
    }

    public class TwtichChannelInfo
    {
        [JsonProperty("status")]
        public string Title { get; set; }
    }

    public class TwitchJsonRootObject
    {
        [JsonProperty("stream")]
        public TwitchStreamInfo Stream { get; set; }
    }
}

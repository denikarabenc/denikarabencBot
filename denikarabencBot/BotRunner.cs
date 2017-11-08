using TwitchBot1.BotCommands;
using TwitchBot1.Interfaces;
using TwitchBot1.Steam;
using TwitchBot1.TwitchStream;
using TwitchBot1.Helpers;
using BotLogger;

namespace TwitchBot1
{
    public class BotRunner
    {
        private readonly IIrcClient irc;
        private bool isCanceled;
        private string channelName;
        private string steamID;

        private string mediaPlayerFileName;

        public BotRunner(IIrcClient irc)
        {
            irc.ThrowIfNull(nameof(irc));
            this.irc = irc;
            isCanceled = false;
        }

        public bool IsCanceled { get => isCanceled; set => isCanceled = value; }
        public string SteamID { get => steamID; set => steamID = value; }
        public string ChannelName { get => channelName; set => channelName = value; }
        public string MediaPlayerFileName { get => mediaPlayerFileName; set => mediaPlayerFileName = value; }

        public void StartBot()
        {
            Logger.Log("[BotRunner] -> Bot started");
            BotCommandsRepository commandPool = new BotCommandsRepository();
            TwitchStreamInfoProvider twitchStreamInfoProvider = new TwitchStreamInfoProvider(ChannelName);
            SteamInfoProvider steamInfoProvider = new SteamInfoProvider(SteamID);
            //IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:vv0yeswj1kpcmyvi381006bl7rxaj4");            
            irc.JoinRoom();
            //irc.SendChatMessage(".mods");

            TimedCommandHandler timedCommandHandler = new TimedCommandHandler(commandPool, irc);
            BotMessageHandler botCommandHandler = new BotMessageHandler(commandPool, irc, twitchStreamInfoProvider, channelName);
            TwitchGameChanger twitchGameChanger = new TwitchGameChanger(irc, twitchStreamInfoProvider, steamInfoProvider, channelName);

            while (!isCanceled)
            {
                string message = irc.ReadMessage();
                botCommandHandler.HadleMessage(message);
            }
        }

        public void ShutDownBot()
        {            
            irc.LeaveRoom();
            Logger.Log("[BotRunner] -> Bot shutted down");
        }
    }
}

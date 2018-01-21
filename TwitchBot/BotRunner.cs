using TwitchBot.Interfaces;
using TwitchBot.Steam;
using TwitchBot.TwitchStream;
using BotLogger;
using Common.Helpers;
using TwitchBot.BotCommands;
using TwitchBot.CommandHandlers;
using Common.Models;
using System;
using Common.Reminders;

namespace TwitchBot
{
    public class BotRunner
    {
        private readonly IIrcClient irc;
        private bool isCanceled;
        private bool isReplayEnabled;
        private bool isAutoGameChangeEnabled;

        private string channelName;
        private string steamID;
        private string replayPath;

        private BotCommandsRepository commandPool;
        private ReminderService reminderService;
        private TwitchGameChanger twitchGameChanger;

        private string mediaPlayerFileName;

        private Action reminderCallback;
        private Action refreshCommandListCallback;

        public BotRunner(IIrcClient irc, Action reminderCallback, Action refreshCommandListCallback)
        {
            irc.ThrowIfNull(nameof(irc));
            this.irc = irc;
            isCanceled = false;
            this.reminderCallback = reminderCallback;
            this.refreshCommandListCallback = refreshCommandListCallback;
        }

        public bool IsCanceled { get => isCanceled; set => isCanceled = value; }        
        public bool IsAutoGameChangeEnabled { get => isAutoGameChangeEnabled; set => isAutoGameChangeEnabled = value; }
        public bool IsReplayEnabled { get => isReplayEnabled; set => isReplayEnabled = value; }

        public string SteamID { get => steamID; set => steamID = value; }
        public string ChannelName { get => channelName; set => channelName = value; }
        public string MediaPlayerFileName { get => mediaPlayerFileName; set => mediaPlayerFileName = value; }
        public string ReplayPath { get => replayPath; set => replayPath = value; }

        public BotCommandsRepository CommandPool => commandPool;
        public ReminderService ReminderService => reminderService;

        public void StartBot()
        {
            Logger.Log(LoggingType.Info, "[BotRunner] -> Bot started");
            commandPool = new BotCommandsRepository(IsReplayEnabled, replayPath);
            reminderService = new ReminderService();
            TwitchStreamInfoProvider twitchStreamInfoProvider = new TwitchStreamInfoProvider(ChannelName);
            SteamInfoProvider steamInfoProvider = new SteamInfoProvider(SteamID);
            //IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:agjzfjjarinmxy46lc9zzae9r4e967");            
            irc.JoinRoom();
            //irc.SendChatMessage(".mods");
            TimedCommandHandler timedCommandHandler = new TimedCommandHandler(commandPool, irc);
            BotMessageHandler botCommandHandler = new BotMessageHandler(commandPool, reminderService, irc, twitchStreamInfoProvider, channelName, reminderCallback, refreshCommandListCallback);
            if (IsAutoGameChangeEnabled)
            {
                Logger.Log(LoggingType.Info, "[BotRunner] -> Auto game change is enabled");
                twitchGameChanger = new TwitchGameChanger(irc, twitchStreamInfoProvider, steamInfoProvider, channelName);
            }
            else
            {
                Logger.Log(LoggingType.Info, "[BotRunner] -> Auto game change is disabled");
            }

            while (!isCanceled)
            {
                string message = irc.ReadMessage();
                botCommandHandler.HadleMessage(message);
            }
        }

        public void ShutDownBot()
        {            
            irc.LeaveRoom();
            twitchGameChanger?.Dispose();
            Logger.Log(LoggingType.Info, "[BotRunner] -> Bot shutted down");
        }
    }
}

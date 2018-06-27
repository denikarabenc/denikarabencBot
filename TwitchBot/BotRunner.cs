using TwitchBot.Steam;
using BotLogger;
using Common.Helpers;
using TwitchBot.BotCommands;
using TwitchBot.CommandHandlers;
using Common.Models;
using System;
using Common.Reminders;
using Common.Voting;
using Common.Interfaces;

namespace TwitchBot
{
    public class BotRunner
    {
        private readonly IIrcClient irc;
        private readonly IStreamUpdater streamUpdater;
        private readonly IStreamClipProvider streamClipProvider;
        private readonly IStreamInfoProvider streamInfoProvider;
        private readonly IMessageParser messageParser;

        private bool isCanceled;
        private bool isReplayEnabled;
        private bool isAutoGameChangeEnabled;

        private string channelName;
        private string steamID;
        private string replayPath;
        private string mediaPlayerFileName;

        private BotCommandsRepository commandPool;

        private ReminderService reminderService;
        private VotingService voteService;

        private AutoStreamGameChanger autoStreamGameChanger;

        private TimedCommandHandler timedCommandHandler;

        private Action reminderCallback;
        private Action refreshCommandListCallback;
        private Action votingCallback;

        public BotRunner(IIrcClient irc, IStreamClipProvider streamClipProvider, IStreamInfoProvider streamInfoProvider, IStreamUpdater streamUpdater, IMessageParser messageParser, Action reminderCallback, Action refreshCommandListCallback, Action votingCallback)
        {
            irc.ThrowIfNull(nameof(irc));
            messageParser.ThrowIfNull(nameof(messageParser));
            this.streamUpdater = streamUpdater;
            this.streamClipProvider = streamClipProvider;
            this.streamInfoProvider = streamInfoProvider;
            this.irc = irc;
            this.messageParser = messageParser;

            isCanceled = false;

            this.reminderCallback = reminderCallback;
            this.refreshCommandListCallback = refreshCommandListCallback;
            this.votingCallback = votingCallback;

            reminderService = new ReminderService();
            voteService = new VotingService();
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
        public VotingService VoteService => voteService;

        public void StartBot()
        {
            Logger.Log(LoggingType.Info, "[BotRunner] -> Bot started");
            commandPool = new BotCommandsRepository(IsReplayEnabled, replayPath);

            autoStreamGameChanger = new AutoStreamGameChanger(irc, streamInfoProvider, streamUpdater, new SteamInfoProvider(SteamID));
           // IStreamInfoProvider twitchStreamInfoProvider = new StreamInfoProvider(ChannelName);
           //  twitchStreamClipProvider = new TwitchStreamClipProvider(channelName);
           //SteamInfoProvider steamInfoProvider = new SteamInfoProvider(SteamID);
           //IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:agjzfjjarinmxy46lc9zzae9r4e967");            
            irc.JoinRoom();
            //irc.SendChatMessage(".mods");
            timedCommandHandler = new TimedCommandHandler(commandPool, irc);
            BotMessageHandler botCommandHandler = new BotMessageHandler(commandPool, reminderService, voteService, irc, streamInfoProvider, streamClipProvider, streamUpdater, messageParser, channelName, reminderCallback, refreshCommandListCallback, votingCallback);
            if (IsAutoGameChangeEnabled)
            {
                Logger.Log(LoggingType.Info, "[BotRunner] -> Auto game change is enabled");
                //twitchGameChanger = new TwitchGameChanger(irc, twitchStreamInfoProvider, steamInfoProvider, channelName);
                autoStreamGameChanger.Start();
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

            ShutDownBot();
        }

        public void ShutDownBot()
        {
            irc.LeaveRoom();
            timedCommandHandler?.Dispose();
            autoStreamGameChanger?.Dispose();
            Logger.Log(LoggingType.Info, "[BotRunner] -> Bot shutted down");
        }

        public void PlayReplay()
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Logger.Log(LoggingType.Info, "[BotRunner] -> PlayReplay");

                string loadingMessage = "Will try, {0}";
                irc.SendInformationChatMessage(String.Format(loadingMessage, channelName));

                string filename = commandPool.GetMediaCommandFileNameAndPath();

                if (filename == string.Empty)
                {
                    string error = "Sorry, didn't make it FeelsBadMan";
                    irc.SendInformationChatMessage(String.Format(error, channelName));
                    return;
                }

                irc.SendInformationChatMessage("Here it goes! FeelsGoodMan");

                VideoWindow vw = new VideoWindow();
                vw.mediaPlayer.Source = new Uri(filename);
                vw.Show();
            });
        }

        public void CreateAndPlayClip()
        {
            Logger.Log(LoggingType.Info, "[BotRunner] -> CreateAndPlayClip");

            string clipId = streamClipProvider.CreateClip();

            string loadingMessage = "Will try, {0}";
            irc.SendInformationChatMessage(String.Format(loadingMessage, channelName));

            System.Timers.Timer clipTimer = new System.Timers.Timer(10000);
            clipTimer.AutoReset = false;
            clipTimer.Enabled = true;
            clipTimer.Elapsed += (sender, e) => ClipTimer_Elapsed(sender, e, clipId);
        }

        private void ClipTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, string clipId)
        {
            if (clipId != "Failed to generate clip FeelsBadMan")
            {
                irc.SendInformationChatMessage("Clip url is https://clips.twitch.tv/" + clipId);
                commandPool.CreateClipCommandFileNameAndPath(clipId);
            }
            else
            {
                irc.SendInformationChatMessage(clipId);
            }
        }
    }
}

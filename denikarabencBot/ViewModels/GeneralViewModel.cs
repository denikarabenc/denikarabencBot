﻿using System.ComponentModel;
using System.Windows;
using TwitchBot;

namespace denikarabencBot.ViewModels
{
    public class GeneralViewModel : INotifyPropertyChanged
    {
        private Visibility removeBotButtonVisibility;
        private bool joinButtonEnabled;

        private string twitchChannelName;
        private string steamID;

        private BotRunner bot;

        public GeneralViewModel()
        {
            twitchChannelName = "denikarabenc";
            steamID = "76561197999517010";
            joinButtonEnabled = true;
            removeBotButtonVisibility = Visibility.Collapsed;
        }

        public Visibility RemoveBotButtonVisibility
        {
            get => removeBotButtonVisibility;
            set
            {
                removeBotButtonVisibility = value;
                OnPropertyChanged(nameof(RemoveBotButtonVisibility));
            }
        }
        public bool JoinButtonEnabled
        {
            get => joinButtonEnabled;
            set
            {
                joinButtonEnabled = value;
                OnPropertyChanged(nameof(JoinButtonEnabled));
            }
        }

        public string TwitchChannelName
        {
            get => twitchChannelName;
            set
            {
                twitchChannelName = value;
                Properties.Settings.Default.TwitchUserName = twitchChannelName;
                OnPropertyChanged(nameof(TwitchChannelName));
            }
        }
        public string SteamID
        {
            get => steamID;
            set
            {
                steamID = value;
                Properties.Settings.Default.SteamId = steamID;
                OnPropertyChanged(nameof(SteamID));
            }
        }
        public BotRunner Bot { get => bot; set => bot = value; }

        public void StartBot()
        {
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:vv0yeswj1kpcmyvi381006bl7rxaj4", TwitchChannelName);
            Bot = new BotRunner(irc);
            bot.ChannelName = TwitchChannelName;
            bot.SteamID = SteamID;
            //for Deni
            //bot.ChannelName = "denikarabenc";
            //bot.SteamID = "76561197999517010";
            bot.IsCanceled = false;
            bot.StartBot();
        }

        public void StopBot()
        {
            bot.ShutDownBot();
            bot.IsCanceled = true;
            bot = null;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}


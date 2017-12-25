using System.ComponentModel;
using System.Windows;
using TwitchBot;

namespace denikarabencBot.ViewModels
{
    public class GeneralViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private Visibility removeBotButtonVisibility;
        private bool joinButtonEnabled;

        private bool isAutoGameChanegeEnabled;
        private bool isReplayEnabled;
       // private string twitchChannelName;
        private string steamID;
        private string replayPath;

        //private BotRunner bot;

        public GeneralViewModel()
        {
            this.Bot = Bot;
            //twitchChannelName = "denikarabenc";
            //steamID = "76561197999517010";
            LoadSettings();

            joinButtonEnabled = true;
            removeBotButtonVisibility = Visibility.Collapsed;
        }

        private void LoadSettings()
        {
            isAutoGameChanegeEnabled = Properties.Settings.Default.IsAutoGameChanegeEnabled;
            isReplayEnabled = Properties.Settings.Default.IsReplayEnabled;
            //twitchChannelName = Properties.Settings.Default.TwitchUserName;
            steamID = Properties.Settings.Default.SteamId;
            replayPath = Properties.Settings.Default.ReplayPath;
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

        //public string TwitchChannelName
        //{
        //    get => twitchChannelName;
        //    set
        //    {
        //        twitchChannelName = value;
        //        Properties.Settings.Default.TwitchUserName = twitchChannelName;
        //        OnPropertyChanged(nameof(TwitchChannelName));
        //    }
        //}

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

        public bool IsAutoGameChanegeEnabled
        {
            get => isAutoGameChanegeEnabled;
            set
            {
                isAutoGameChanegeEnabled = value;
                Properties.Settings.Default.IsAutoGameChanegeEnabled = isAutoGameChanegeEnabled;
                OnPropertyChanged(nameof(IsAutoGameChanegeEnabled));
            }
        }

        public bool IsReplayEnabled
        {
            get => isReplayEnabled;
            set
            {
                isReplayEnabled = value;
                Properties.Settings.Default.IsReplayEnabled = isReplayEnabled;
                OnPropertyChanged(nameof(IsReplayEnabled));
            }
        }

        public string ReplayPath
        {
            get => replayPath;
            set
            {
                replayPath = value;
                Properties.Settings.Default.ReplayPath = replayPath;
                OnPropertyChanged(nameof(ReplayPath));
            }
        }        

        //public BotRunner Bot { get => bot; set => bot = value; }        

        public void StartBot()
        {
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:vv0yeswj1kpcmyvi381006bl7rxaj4", TwitchChannelName);
            Bot = new BotRunner(irc);
            Bot.ChannelName = TwitchChannelName;
            Bot.SteamID = SteamID;
            Bot.IsReplayEnabled = IsReplayEnabled;
            Bot.ReplayPath = ReplayPath;
            Bot.IsAutoGameChangeEnabled = IsAutoGameChanegeEnabled;
            //for Deni
            //bot.ChannelName = "denikarabenc";
            //bot.SteamID = "76561197999517010";
            Bot.IsCanceled = false;
            Bot.StartBot();
        }

        public void StopBot()
        {
            Bot.ShutDownBot();
            Bot.IsCanceled = true;
            Bot = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}


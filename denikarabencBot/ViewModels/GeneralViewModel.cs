using BotLogger;
using Common.Helpers;
using Common.Models;
using Common.WPFCommand;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TwitchBot;

namespace denikarabencBot.ViewModels
{
    public class GeneralViewModel : INotifyPropertyChanged
    {
        private bool removeBotButtonVisibility;
        private bool joinButtonEnabled;

        private bool isAutoGameChanegeEnabled;
        private bool isReplayEnabled;
        private string steamID;
        private string replayPath;
        
        private string twitchChannelName;

        private Thread botThread;

        private ICommand joinBotCommand;
        private ICommand removeBotCommand;

        private BotRunner bot;

        private CommandsViewModel commandsViewModel;
        private YoutubeViewModel youtubeViewModel;

        //private BotRunner bot;

        public GeneralViewModel()
        {
            //twitchChannelName = "denikarabenc";
            //steamID = "76561197999517010";
            LoadSettings();
            commandsViewModel = new CommandsViewModel();
            YoutubeViewModel = new YoutubeViewModel();
            joinButtonEnabled = true;

            removeBotButtonVisibility = false;
        }
      
        private void LoadSettings()
        {
            isAutoGameChanegeEnabled = Properties.Settings.Default.IsAutoGameChanegeEnabled;
            isReplayEnabled = Properties.Settings.Default.IsReplayEnabled;
            twitchChannelName = Properties.Settings.Default.TwitchUserName;
            steamID = Properties.Settings.Default.SteamId;
            replayPath = Properties.Settings.Default.ReplayPath;
        }

        public ICommand JoinBotCommand
        {
            get => joinBotCommand ?? (joinBotCommand = new RelayCommand(param => JoinBotCommandExecute(), param => JoinBotCommandCanExecute()));
        }

        public ICommand RemoveBotCommand
        {
            get => removeBotCommand ?? (removeBotCommand = new RelayCommand(param => RemoveBotCommandExecute(), param => RemoveBotCommandCanExecute()));
        }

        private void RemoveBotCommandExecute()
        {
            Logger.Log(LoggingType.Info, "[MainWindow] -> Remove clicked");
            if (botThread.IsAlive)
            {
                StopBot();
                botThread.Abort();
            }

            joinButtonEnabled = true;
            removeBotButtonVisibility = false;
            
        }        

        private bool RemoveBotCommandCanExecute()
        {
            return RemoveBotButtonVisibility;
        }

        private void JoinBotCommandExecute()
        {
            Logger.Log(LoggingType.Info, "[MainWindow] -> Join clicked");
            removeBotButtonVisibility = true;
            botThread = new Thread(() =>
            {
                StartBot();
            });
            botThread.Start();
            joinButtonEnabled = false;
            OnPropertyChanged(nameof(JoinButtonEnabled));
        }

        private bool JoinBotCommandCanExecute()
        {
            return JoinButtonEnabled;
        }

        public bool RemoveBotButtonVisibility
        {
            get => removeBotButtonVisibility;           
        }

        public bool JoinButtonEnabled
        {
            get => joinButtonEnabled;            
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

        public CommandsViewModel CommandsViewModel { get => commandsViewModel; set => commandsViewModel = value; }
        public YoutubeViewModel YoutubeViewModel { get => youtubeViewModel; set => youtubeViewModel = value; }

        private void StartBot()
        {
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:agjzfjjarinmxy46lc9zzae9r4e967", TwitchChannelName);
            bot = new BotRunner(irc, null);
            bot.ChannelName = TwitchChannelName;
            bot.SteamID = SteamID;
            bot.IsReplayEnabled = IsReplayEnabled;
            bot.ReplayPath = ReplayPath;
            bot.IsAutoGameChangeEnabled = IsAutoGameChanegeEnabled;
            //for Deni
            //bot.ChannelName = "denikarabenc";
            //bot.SteamID = "76561197999517010";
            bot.IsCanceled = false;
            bot.StartBot();
        }

        public void StopBot()
        {
            if (botThread != null && botThread.IsAlive && bot != null)
            {
                bot.ShutDownBot();
                bot.IsCanceled = true;
                bot = null;
                botThread.Abort();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}


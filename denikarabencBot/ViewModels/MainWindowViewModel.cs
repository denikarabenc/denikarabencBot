using BotLogger;
using Common.Commands;
using Common.Models;
using Common.Reminders;
using Common.WPFCommand;
using denikarabencBot.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TwitchBot;
using TwitchBot.TwitchStream;

namespace denikarabencBot.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {         
        private readonly CommandSaver commandSaver;
        private readonly CommandReader commandReader;
        private readonly CommandConditioner commandConditioner;
        private readonly ReminderService reminderService;

        private bool isTimed;
        private bool removeBotButtonVisibility;
        private bool joinButtonEnabled;
        private bool isAutoGameChanegeEnabled;
        private bool isReplayEnabled;
        private bool hasNewReminder;
        private string message;
        private string command;
        private string steamID;
        private string replayPath;
        private string twitchChannelName;
        private UserType permission;
        private BotCommand selectedCommand;
        private List<BotCommand> commandList;
        private List<Reminder> reminderList;
        private List<UserType> permissionSource;
        
        private Thread botThread;

        private ICommand addCommandCommand;
        private ICommand removeSelectedCommandCommand;
        private ICommand joinBotCommand;
        private ICommand removeBotCommand;
        private ICommand openReminderListCommand;

        private Action reminderCallback;

        private BotRunner bot;

        private YoutubeViewModel youtubeViewModel;
        
        public MainWindowViewModel()
        {
            //twitchChannelName = "denikarabenc";
            //steamID = "76561197999517010";
            LoadSettings();
            YoutubeViewModel = new YoutubeViewModel();
            joinButtonEnabled = true;

            removeBotButtonVisibility = false;

            commandSaver = new CommandSaver();
            commandReader = new CommandReader();
            commandConditioner = new CommandConditioner();
            reminderService = new ReminderService();

            IsTimed = false;
            commandList = new List<BotCommand>();
            RefreshCommandList();

            reminderList = reminderService.GetAllReminders();
            //hasNewReminder = true;

            permissionSource = new List<UserType>();
            permissionSource.Add(UserType.Regular);
            permissionSource.Add(UserType.Follower);
            permissionSource.Add(UserType.Mod);
            permissionSource.Add(UserType.King);
            reminderCallback = ReminderRefresh;
        }

        private void ReminderRefresh()
        {
            hasNewReminder = true;
            reminderList = reminderService.GetAllReminders();
            OnPropertyChanged(nameof(ReminderList));
            OnPropertyChanged(nameof(HasNewReminder));
        }

        public ICommand OpenReminderListCommand
        {
            get => openReminderListCommand ?? (openReminderListCommand = new RelayCommand(param => OpenReminderList(), param => OpenReminderListCanExecute()));
        }

        public ICommand AddCommandCommand
        {
            get => addCommandCommand ?? (addCommandCommand = new RelayCommand(param => SaveCommand(), param => CanAddCommandCommandExecute()));
        }

        public ICommand RemoveSelectedCommandCommand
        {
            get => removeSelectedCommandCommand ?? (removeSelectedCommandCommand = new RelayCommand(param => RemoveSelectedCommandCommandExecute(), param => CanRemoveSelectedCommandCommandExecute()));
        }

        public string Message { get => message; set => message = value; }
        public string Command { get => command; set => command = value; }
        public UserType Permission { get => permission; set => permission = value; }
        public List<UserType> PermissionSource { get => permissionSource; set => permissionSource = value; }
        public bool IsTimed { get => isTimed; set => isTimed = value; }
        public List<BotCommand> CommandList { get => commandList; }
        public BotCommand SelectedCommand { get => selectedCommand; set => selectedCommand = value; }

        private bool OpenReminderListCanExecute()
        {
            return true;
        }

        private void OpenReminderList()
        {
            hasNewReminder = false;
            OnPropertyChanged(nameof(HasNewReminder));

            ReminderWindow rw = new ReminderWindow(new ReminderWindowViewModel(reminderList));

            rw.Show();
        }

        private bool CanAddCommandCommandExecute()
        {
            return commandConditioner.CanSave(Command, Message);
        }

        private void SaveCommand()
        {
            Logger.Log(LoggingType.Info, $"Command {Command} added");

            if (!commandConditioner.CanSave(command, message))
            {
                MessageBox.Show("Command has to have field command and message populated!");
                return;
            }

            commandSaver.AddCommandToXML(Command, Message, Permission, IsTimed);

            RefreshCommandList();

            bot?.CommandPool.UpdatePredefinedCommandsFromXML(true); //TODO
        }

        private bool CanRemoveSelectedCommandCommandExecute()
        {
            return SelectedCommand != null;
        }

        private void RemoveSelectedCommandCommandExecute()
        {
            commandSaver.RemoveCommandFromXML(SelectedCommand);
            Logger.Log(LoggingType.Info, string.Format("[CommandsViewModel] -> Command {0} removed!", SelectedCommand.Command));
            RefreshCommandList();

            bot?.CommandPool.UpdatePredefinedCommandsFromXML(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<BotCommand> FilterCommandsList(List<BotCommand> commandList)
        {
            foreach (string commandName in CommandsNotToShowInList())
            {
                BotCommand botCommand = commandList.Where(bc => bc.Command == commandName).FirstOrDefault();
                commandList.Remove(botCommand);
            }

            return commandList;
        }

        private List<string> CommandsNotToShowInList()
        {
            return new List<string>
            {
                "!sr",
                "!replay",
                "!addcommand",
                "!editcommand",
                "!clip",
            };
        }

        private void RefreshCommandList()
        {
            commandList = commandReader.GetAllCommandsFromXML();
            commandList = FilterCommandsList(commandList);
            OnPropertyChanged(nameof(CommandList));
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
            OnPropertyChanged(nameof(JoinButtonEnabled));
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

        public bool HasNewReminder => hasNewReminder;

        public YoutubeViewModel YoutubeViewModel { get => youtubeViewModel; set => youtubeViewModel = value; }
        public List<Reminder> ReminderList => reminderList;

        private void StartBot()
        {
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:agjzfjjarinmxy46lc9zzae9r4e967", TwitchChannelName);
            bot = new BotRunner(irc, reminderCallback);
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
    }
}

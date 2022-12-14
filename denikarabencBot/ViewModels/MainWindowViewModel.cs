using BotCore;
using BotLogger;
using Common.Commands;
using Common.Creators;
using Common.Helpers;
using Common.Interfaces;
using Common.Models;
using Common.Reminders;
using Common.Voting;
using Common.WPFCommand;
using denikarabencBot.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TweeterProvider;
using TwitchBot;
using TwitchBot.Helpers;
using TwitchBot.TwitchStream;

namespace denikarabencBot.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {         
        private readonly CommandSaver commandSaver;
        private readonly CommandReader commandReader;
        private readonly CommandConditioner commandConditioner;
        private readonly ReminderService reminderService;
        private readonly VotingService votingService;

        private bool removeBotButtonVisibility;
        private bool joinButtonEnabled;
        private bool isAutoGameChanegeEnabled;
        private bool isReplayEnabled;
        private bool hasNewReminder;
        private string steamID;
        private string replayPath;
        private string twitchChannelName;
        private string defaultVotingCategory;
        private DateTime lastVoteResetTime;
        private BotCommand selectedCommand;

        private List<BotCommand> commandList;
        private List<Vote> voteList;
        
        private Thread botThread;

        private ICommand addCommandCommand;
        private ICommand editCommandCommand;
        private ICommand removeSelectedCommandCommand;
        private ICommand joinBotCommand;
        private ICommand removeBotCommand;
        private ICommand openReminderListCommand;
        private ICommand clearVotesCommand;
        private ICommand openVoteReportCommand;
        private ICommand setDefaultVotingCategoryCommand;

        private Action reminderCallback;
        private Action refreshCommandsCallback;
        private Action votingCallback;

        private BotRunner bot;

        private YoutubeViewModel youtubeViewModel;
        
        public MainWindowViewModel()
        {
            //twitchChannelName = "denikarabenc";
            //steamID = "76561197999517010";
            
            CreateFiles();
            YoutubeViewModel = new YoutubeViewModel();
            joinButtonEnabled = true;

            removeBotButtonVisibility = false;

            commandSaver = new CommandSaver();
            commandReader = new CommandReader();
            commandConditioner = new CommandConditioner();
            reminderService = new ReminderService();
            votingService = new VotingService();

            commandList = new List<BotCommand>();
            RefreshCommandList();
            FillVoteList();

            reminderCallback = ReminderRefresh;
            refreshCommandsCallback = RefreshCommandList;
            votingCallback = FillVoteList;

            LoadSettings();
        }

        private void CreateFiles()
        {
            string clipPath = Directory.GetCurrentDirectory() + "/" + "Clips";
            Directory.CreateDirectory(clipPath);
            FileCreator fc = new FileCreator();
            fc.CreateFileIfNotExist(clipPath, "clipHTML", "html");

            using (StreamWriter writer = new StreamWriter(clipPath + "/" + "clipHTML.html", false))
            {
                writer.Write(@"<!DOCTYPE html><html><head><title> Clip </title></head><body><iframe src=""https://clips.twitch.tv/embed?clip=" + "EmpathicViscousSushiYouDontSay" + @"&autoplay=true"" width=""1024"" height=""575"" frameborder=""0"" scrolling=""no"" allowfullscreen=""false"" ></iframe></body></html>");
            }
        }

        private void ReminderRefresh()
        {
            hasNewReminder = true;
            OnPropertyChanged(nameof(HasNewReminder));
        }      

        public ICommand OpenReminderListCommand
        {
            get => openReminderListCommand ?? (openReminderListCommand = new RelayCommand(param => OpenReminderList(), param => OpenReminderListCanExecute()));
        }

        public ICommand AddCommandCommand
        {
            get => addCommandCommand ?? (addCommandCommand = new RelayCommand(param => AddCommand(), param => CanAddCommandCommandExecute()));
        }

        public ICommand EditCommandCommand
        {
            get => editCommandCommand ?? (editCommandCommand = new RelayCommand(param => EditCommand(), param => CanEditCommandCommandExecute()));
        }

        public ICommand RemoveSelectedCommandCommand
        {
            get => removeSelectedCommandCommand ?? (removeSelectedCommandCommand = new RelayCommand(param => RemoveSelectedCommandCommandExecute(), param => CanRemoveSelectedCommandCommandExecute()));
        }

        public ICommand JoinBotCommand
        {
            get => joinBotCommand ?? (joinBotCommand = new RelayCommand(param => JoinBotCommandExecute(), param => JoinBotCommandCanExecute()));
        }

        public ICommand RemoveBotCommand
        {
            get => removeBotCommand ?? (removeBotCommand = new RelayCommand(param => RemoveBotCommandExecute(), param => RemoveBotCommandCanExecute()));
        }

        public ICommand ClearVotesCommand
        {
            get => clearVotesCommand ?? (clearVotesCommand = new RelayCommand(param => ClearVotesCommandExecite(), param => VoteCommandsCanExecute()));
        }

        public ICommand OpenVoteReportCommand
        {
            get => openVoteReportCommand ?? (openVoteReportCommand = new RelayCommand(param => OpenVoteReportCommandExecute(), param => VoteCommandsCanExecute()));
        }

        public ICommand SetDefaultVotingCategoryCommand
        {
            get => setDefaultVotingCategoryCommand ?? (setDefaultVotingCategoryCommand = new RelayCommand(param => SetDefaultVotingCategoryCommandExecute()));
        }

        public bool HasNewReminder => hasNewReminder;
        public string DefaultVotingCategory { get => defaultVotingCategory; set => defaultVotingCategory = value; }

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
                if (Bot != null)
                {
                    Bot.IsAutoGameChangeEnabled = isAutoGameChanegeEnabled;
                    Bot.ApplySettings();
                }
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
                if (Bot != null)
                {
                    Bot.IsReplayEnabled = isReplayEnabled;
                    Bot.ApplySettings();
                }
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

        public DateTime LastVoteResetTime => lastVoteResetTime;

        public BotRunner Bot => bot;
        public YoutubeViewModel YoutubeViewModel { get => youtubeViewModel; set => youtubeViewModel = value; }
        public BotCommand SelectedCommand { get => selectedCommand; set => selectedCommand = value; }
        public List<BotCommand> CommandList { get => commandList; }
        public List<Vote> VoteList
        {
            get
            {
                return voteList;
            }
            set
            {
                voteList = value;
                OnPropertyChanged(nameof(VoteList));
            }
        }

        private void SetDefaultVotingCategoryCommandExecute()
        {
            bot?.VoteService.SetDefaultVoteCategory(DefaultVotingCategory);
            votingService.SetDefaultVoteCategory(DefaultVotingCategory);
            Properties.Settings.Default.DefaultVotingCategory = DefaultVotingCategory;
            Logger.Log(LoggingType.Info, String.Format("[MainWindowViewModel] -> Default voting category set to {0}", DefaultVotingCategory));
        }

        private bool VoteCommandsCanExecute()
        {
            return VoteList.Count > 0;
        }

        private void ClearVotesCommandExecite()
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.denikarabencBot_ARE_YOU_SURE_YOU_WANT_TO_REMOVE_ALL_VOTES, Properties.Resources.denikarabencBot_CONFORMATION, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            votingService.ClearVotes();
            VoteList = votingService.GetAllVotes();
            lastVoteResetTime = DateTime.Now;
            Properties.Settings.Default.LastVotesResetTime = lastVoteResetTime;
            OnPropertyChanged(nameof(LastVoteResetTime));
        }

        private void OpenVoteReportCommandExecute()
        {
            VoteReportWindow vrw = new VoteReportWindow(new VoteReportViewModel(VoteList));
            vrw.ShowDialog();
        }

        private bool OpenReminderListCanExecute()
        {
            return true;
        }

        private void OpenReminderList()
        {
            hasNewReminder = false;
            OnPropertyChanged(nameof(HasNewReminder));

            ReminderWindow rw = new ReminderWindow(new ReminderWindowViewModel(reminderService));

            rw.ShowDialog();
        }

        private bool CanAddCommandCommandExecute()
        {
            return true;
        }

        private void AddCommand()
        {
            CommandWindowViewModel cwvm = new CommandWindowViewModel(SelectedCommand, commandConditioner, RefreshCommandList , false);
            CommandWindow cw = new CommandWindow(cwvm);

            cw.ShowDialog();

            Bot?.CommandPool.UpdateCommandsFromXML(); //TODO
        }

        private bool CanEditCommandCommandExecute()
        {
            return commandConditioner.CanEdit(SelectedCommand);
        }

        private void EditCommand()
        {
            CommandWindowViewModel cwvm = new CommandWindowViewModel(SelectedCommand, commandConditioner, RefreshCommandList, true);
            CommandWindow cw = new CommandWindow(cwvm);

            cw.ShowDialog();

            Bot?.CommandPool.UpdateCommandsFromXML(); //TODO
        }

        private bool CanRemoveSelectedCommandCommandExecute()
        {
            return SelectedCommand != null;
        }

        private void RemoveSelectedCommandCommandExecute()
        {
            commandSaver.RemoveCommandFromXML(SelectedCommand);
            Logger.Log(LoggingType.Info, string.Format("[MainWindowViewModel] -> Command {0} removed!", SelectedCommand.Command));
            RefreshCommandList();

            Bot?.CommandPool.UpdateCommandsFromXML();
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
            commandList = commandReader.GetAllCommandsFromXML(); //TODO perserve order in list on refresh for edit
            commandList = FilterCommandsList(commandList);
            OnPropertyChanged(nameof(CommandList));
        }

        private void FillVoteList()
        {
            VoteList = votingService.GetAllVotes();
        }

        private void LoadSettings()
        {
            isAutoGameChanegeEnabled = Properties.Settings.Default.IsAutoGameChanegeEnabled;
            isReplayEnabled = Properties.Settings.Default.IsReplayEnabled;
            twitchChannelName = Properties.Settings.Default.TwitchUserName;
            steamID = Properties.Settings.Default.SteamId;
            replayPath = Properties.Settings.Default.ReplayPath;
            votingService.SetDefaultVoteCategory(Properties.Settings.Default.DefaultVotingCategory);
            defaultVotingCategory = Properties.Settings.Default.DefaultVotingCategory;
            lastVoteResetTime = Properties.Settings.Default.LastVotesResetTime;
        }     

        private void RemoveBotCommandExecute()
        {
            Logger.Log(LoggingType.Info, "[MainWindowViewModel] -> Remove clicked");
            if (botThread.IsAlive)
            {
                StopBot();
                //botThread.Abort();
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
            Logger.Log(LoggingType.Info, "[MainWindowViewModel] -> Join clicked");

            try
            {
                botThread = new Thread(() =>
                {
                    StartBot();
                });
            
                removeBotButtonVisibility = true;
                botThread.Start();
                joinButtonEnabled = false;
                OnPropertyChanged(nameof(JoinButtonEnabled));
            }
            catch (Exception e)
            {
                Logger.Log(LoggingType.Error, "Could not start bot", e);
                MessageBox.Show(string.Format("{0} {1}", Properties.Resources.denikarabencBot_COULD_NOT_START_BOT, Properties.Resources.denikarabencBot_PLEASE_TRY_AGAIN_LATER), Properties.Resources.denikarabencBot_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void StartBot()
        {
            string channelId = TwitchIntegration.Helpers.TwitchUserInformationHelper.GetChannelId(TwitchChannelName); //TODO make IStreamChannelInfoProvider
            IIrcClient irc = new TwitchIrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:agjzfjjarinmxy46lc9zzae9r4e967", TwitchChannelName);
            IStreamClipProvider clipProvider = new TwitchStreamClipProvider(TwitchChannelName);
            IStreamInfoProvider streamInfoProvider = new TwitchStreamInfoProvider(channelId);
            IStreamUpdater streamUpdater = new TwitchStreamUpdater(channelId);
            IMessageParser messageParser = new TwitchMessageParser();
            ITweeterProvider tweeterProvider = new TweeterProvider.TweeterProvider();
            bot = new BotRunner(irc, clipProvider, streamInfoProvider, streamUpdater, messageParser, tweeterProvider, reminderCallback, refreshCommandsCallback, votingCallback);
            bot.ChannelName = TwitchChannelName;
            bot.SteamID = SteamID;
            bot.IsReplayEnabled = IsReplayEnabled;
            bot.ReplayPath = ReplayPath;
            bot.IsAutoGameChangeEnabled = IsAutoGameChanegeEnabled;
            bot.VoteService.SetDefaultVoteCategory(votingService.GetDefaultCategory()); //Budz da bi moglo da se dodaje i kad nije bot aktiviran
                                                                                        //for Deni
                                                                                        //bot.ChannelName = "denikarabenc";
                                                                                        //bot.SteamID = "76561197999517010";
            bot.IsCanceled = false;
            bot.StartBot();
        }

        public void StopBot()
        {
            if (botThread != null && botThread.IsAlive && bot != null && !bot.IsCanceled)
            {
                bot.ShutDownBot();
                bot.IsCanceled = true;
                bot = null;
                //botThread.Abort();
            }
        }
    }
}

using BotLogger;
using Common.Helpers;
using Common.Models;
using Common.Reminders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using TwitchBot.BotCommands;
using TwitchBot.Helpers;
using TwitchBot.Interfaces;
using TwitchBot.TwitchStream;
using Youtube;

namespace TwitchBot.CommandHandlers
{
    public class BotMessageHandler
    {
        private readonly IIrcClient irc;
        private readonly string channelName;
        private readonly BotCommandsRepository botCommands;
        private readonly MessageRepository messageRepository;
        private readonly ReminderService reminderService;

        private bool mediaCommandAllowed;
        private TwitchStreamInfoProvider twitchStreamInfoProvider;
        private TwitchStreamUpdater twitchStreamUpdater;
        private TwitchStreamClipProvider twitchStreamClipProvider;
        private YoutubeBotService youtubeProvider;
        private List<string> modsList;
        private Timer modRefreshTimer;
        private Timer mediaCommandTimer;
        private Random radnomIndex;

        private Action reminderCallback;
        private Action refreshCommandListCallback;

        public BotMessageHandler(BotCommandsRepository botCommands, ReminderService reminderService, IIrcClient irc, TwitchStreamInfoProvider twitchStreamInfoProvider, TwitchStreamClipProvider twitchStreamClipProvider, string channelName, Action reminderCallback, Action refreshCommandListCallback)
        {
            irc.ThrowIfNull(nameof(irc));
            botCommands.ThrowIfNull(nameof(botCommands));
            twitchStreamInfoProvider.ThrowIfNull(nameof(twitchStreamInfoProvider));
            twitchStreamClipProvider.ThrowIfNull(nameof(twitchStreamClipProvider));
            reminderService.ThrowIfNull(nameof(reminderService));
            this.twitchStreamInfoProvider = twitchStreamInfoProvider;
            this.twitchStreamClipProvider = twitchStreamClipProvider;
            this.reminderService = reminderService;
            this.irc = irc;
            this.botCommands = botCommands;
            this.channelName = channelName;
            this.reminderCallback = reminderCallback;
            this.refreshCommandListCallback = refreshCommandListCallback;

            youtubeProvider = new YoutubeBotService();

            modsList = new List<string>();
           // twitchStreamClipProvider = new TwitchStreamClipProvider(channelName);
            twitchStreamUpdater = new TwitchStreamUpdater(channelName);
            mediaCommandAllowed = true;

            messageRepository = new MessageRepository();

            radnomIndex = new Random();

            modRefreshTimer = new Timer(3000);
            modRefreshTimer.AutoReset = false;
            modRefreshTimer.Enabled = true;
            modRefreshTimer.Elapsed += RefresMods;

            mediaCommandTimer = new Timer(60000);
            mediaCommandTimer.AutoReset = false;
            mediaCommandTimer.Enabled = false;
            mediaCommandTimer.Elapsed += MediaCommandTimer_Elapsed;
        }

        private string SelectRandomItem(List<string> items)
        {
            int i = radnomIndex.Next(items.Count);
            return items[i];
        }

        private void MediaCommandTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            mediaCommandAllowed = true;
        }

        private void RefresMods(object sender, ElapsedEventArgs e)
        {
            GetModsListRequest();
        }

        private void GetModsListRequest()
        {
            irc.SendInformationChatMessage(".mods");
        }

        public void HadleMessage(string message)
        {
            string parsedMessage = MessageParser.GetParsedMessage(message);
            string userWhoSentMessage = MessageParser.GetMessageSenderUserName(message);
            CommandType commandType = botCommands.GetCommandType(parsedMessage.Split(' ')[0]);
            switch (commandType)
            {
                case CommandType.Ping:
                    HandlePingCommand();
                    break;
                case CommandType.ModsRequest:
                    HandleModsRequestCommand(parsedMessage);
                    break;
                case CommandType.AddCommand:
                    HandleAddCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.EditCommand:
                    HandleEditCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.ReadCommand:
                    HandleReadCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.TwitchStatusCommand:
                    HandleTwitchStatusCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.ChangeTitleCommand:
                    HandleChangeTitleCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.UserInputCommand:
                    HandleUserInputCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.MediaCommand:
                    HandleMediaCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.SongRequestCommand:
                    HandleSongRequestCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.CreateClip:
                    HandleCreateClipCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.CommandList:
                    HandleCommandListCommand();
                    break;
                case CommandType.Reminder:
                    HandleRemindersCommand(parsedMessage, userWhoSentMessage);
                    break;
                case CommandType.NotExist:
                    CommandType commandTypeReChecked = botCommands.GetCommandType(message);
                    if (commandTypeReChecked == CommandType.ModsRequest)
                    {
                        HandleModsRequestCommand(parsedMessage);
                    }
                    return;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void HandleRemindersCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (parsedMessage.Length > 10)
            {
                reminderService.AddReminder(new Reminder(userWhoSentMessage, parsedMessage.Substring(10)));
                reminderCallback?.Invoke();
                irc.SendInformationChatMessage(String.Format("{0} will be reminded!", channelName));
            }
        }

        private void HandlePingCommand()
        {
            irc.PongMessage();
        }

        private void HandleCreateClipCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (!mediaCommandAllowed)
            {
                irc.SendChatMessage("Wow, do not spam it FeelsBadMan");
                return;
            }

            if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage)))
            {                
                string clipId = twitchStreamClipProvider.CreateTwitchClip();

                string loadingMessage = SelectRandomItem(messageRepository.LoadingMessages);
                irc.SendInformationChatMessage(String.Format(loadingMessage, userWhoSentMessage));

                Timer clipTimer = new Timer(10000);
                clipTimer.AutoReset = false;
                clipTimer.Enabled = true;
                clipTimer.Elapsed += (sender, e) => ClipTimer_Elapsed(sender, e, clipId);              
            }
        }

        private void ClipTimer_Elapsed(object sender, ElapsedEventArgs e, string clipId)
        {
            if (clipId != "Failed to generate clip FeelsBadMan")
            {
                irc.SendInformationChatMessage("Clip url is https://clips.twitch.tv/" + clipId);
                botCommands.CreateClipCommandFileNameAndPath(clipId);

                mediaCommandAllowed = false;
                mediaCommandTimer.Enabled = true;
                mediaCommandTimer.Start();
            }
            else
            {
                irc.SendInformationChatMessage(clipId);
            }
        }

        private void HandleSongRequestCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage)))
            {
                if (parsedMessage.Split(' ').Length > 1)
                {
                    if (parsedMessage.Split(' ')[1].Contains("youtube.com"))
                    {
                        youtubeProvider.AddSongToPlaylist(parsedMessage.Split(' ')[1].Split('=')[1], userWhoSentMessage, CallbackFromSongRequest);
                    }
                    else
                    {
                        irc.SendInformationChatMessage("For now only full link to the song works FeelsBadMan");
                    }
                }
                else
                {
                    irc.SendChatMessage("Use !sr [link]");
                }
            }
        }

        private void HandleCommandListCommand()
        {
            irc.SendChatMessage(botCommands.GetAllCommands());
        }

        private void HandleMediaCommand(string parsedMessage, string userWhoSentMessage)
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Logger.Log(LoggingType.Info, "[BotMessageHandler] -> Replay command handled");
                if (!mediaCommandAllowed)
                {
                    irc.SendChatMessage("Wow, do not spam it FeelsBadMan");
                    return;
                }

                string loadingMessage = SelectRandomItem(messageRepository.LoadingMessages);
                irc.SendInformationChatMessage(String.Format(loadingMessage, userWhoSentMessage));
                
                string filename = botCommands.GetMediaCommandFileNameAndPath();

                if (filename == string.Empty)
                {
                    string error = SelectRandomItem(messageRepository.ErrorMessages);
                    irc.SendInformationChatMessage(String.Format(error, userWhoSentMessage));
                    //irc.SendChatMessage($"/w {channelName} Check the OBS replay");
                    return;
                }

                mediaCommandAllowed = false;

                mediaCommandTimer.Enabled = true;
                mediaCommandTimer.Start();

                irc.SendInformationChatMessage("Here it goes! FeelsGoodMan");

                VideoWindow vw = new VideoWindow();
                vw.mediaPlayer.Source = new Uri(filename);
                vw.Show();
            });
        }

        private void HandleUserInputCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage.Split(' ')[0])))
            {
                int index = parsedMessage.IndexOf(" ");
                if (index != -1 && parsedMessage.Length > index + 1)
                {
                    irc.SendChatMessage(botCommands.GetUserInputCommandMessage(parsedMessage.Split(' ')[0], userWhoSentMessage, parsedMessage.Substring(parsedMessage.IndexOf(" ") + 1)));
                }
            }
        }

        private void HandleChangeTitleCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (parsedMessage != "!title")
            {
                if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage)))
                {
                    string report = twitchStreamUpdater.SetTwitchStatiusAndReturnWhichStatusIsSet(parsedMessage.Substring(parsedMessage.IndexOf(" ") + 1));
                    irc.SendChatMessage(botCommands.GetUserInputCommandMessage(parsedMessage.Split(' ')[0], userWhoSentMessage, report));
                }
            }
            else
            {
                string title = twitchStreamInfoProvider.GetTitle();
                if (title != null)
                {
                    irc.SendChatMessage("Current title is: " + title);
                }
                else
                {
                    irc.SendChatMessage("Could not read title");
                }
            }
        }

        private void HandleModsRequestCommand(string parsedMessage)
        {
            string parsedMods = MessageParser.GetParsedModsMessage(parsedMessage);
            modsList = parsedMods.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            modsList.Add(channelName);
        }

        private void HandleReadCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage)))
            {
                irc.SendChatMessage(botCommands.GetReadCommandMessage(parsedMessage, userWhoSentMessage));
            }
        }

        private void HandleTwitchStatusCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage)))
            {
                string param = string.Empty;

                //foreach (TwitchGame tg in twitchStreamInfoProvider.GamesPlayed)
                //{
                //    param += tg.GameName + " for " + tg.TimePlayed.Elapsed.ToString("hh\\:mm\\:ss");
                //}

                for (int i = 0; i < twitchStreamInfoProvider.GamesPlayed.Count; i++)
                {
                    param += twitchStreamInfoProvider.GamesPlayed[i].GameName + " for " + twitchStreamInfoProvider.GamesPlayed[i].TimePlayed.Elapsed.ToString("hh\\:mm\\:ss");

                    if (i != twitchStreamInfoProvider.GamesPlayed.Count - 1)
                    {
                        param += ", ";
                    }
                }

                //string param = String.Join(", ", (twitchStreamInfoProvider.GamesPlayed.Select(x => x.GameName)));

                irc.SendChatMessage(botCommands.GetTwitchStatusCommandMessage(parsedMessage, param));
            }
        }

        private void HandleAddCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage)))
            {
                if (parsedMessage.Split(' ').Length > 1)
                {
                    string newCommand = parsedMessage.Split(' ')[1];
                    if (newCommand.Contains(":"))
                    {
                        irc.SendChatMessage("Invalid format. Use !addcommand [command] : [command message]");
                        return;
                    }
                    string newCommandMessage = MessageParser.GetParsedMessage(parsedMessage).Split(':').Last();

                    irc.SendChatMessage(botCommands.AddCommandAndGetFeedback(newCommand, newCommandMessage, refreshCommandListCallback));
                }
                else
                {
                    irc.SendChatMessage("Use !addcommand [command] : [command message]");
                }
            }
        }

        private void HandleEditCommand(string parsedMessage, string userWhoSentMessage)
        {
            if (ContainsPermissions(userWhoSentMessage, botCommands.GetCommandPermissions(parsedMessage)))
            {
                if (parsedMessage.Split(' ').Length > 1)
                {
                    string commandForEdit = parsedMessage.Split(' ')[1];
                    string newCommandMessage = MessageParser.GetParsedMessage(parsedMessage).Split(':').Last();

                    irc.SendChatMessage(botCommands.EditCommandAndGetFeedback(commandForEdit, newCommandMessage, refreshCommandListCallback));
                }
                else
                {
                    irc.SendChatMessage("Use !editcommand [command] : [new command message]");
                }
            }
        }

        private bool ContainsPermissions(string user, UserType commandPermision)
        {
            switch (commandPermision)
            {
                case UserType.Regular:
                    return true;
                case UserType.Follower:
                case UserType.Sub:
                    return true;
                case UserType.Mod:
                    if (modsList.Contains(user) || user == channelName)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case UserType.Editor:
                case UserType.King:
                    if (user == channelName)
                    {
                        return true;
                    }
                    else
                    {
                        return false; //TODO
                    }
                default: return false;
            }

        }

        private void CallbackFromSongRequest(string message)
        {
            irc.SendInformationChatMessage(message);
        }
    }
}

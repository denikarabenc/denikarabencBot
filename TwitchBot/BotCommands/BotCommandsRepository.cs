using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Serialization; //TODO ovo treba skinuti odavde i napraviti tool za prebacivanje komandi
using Common.Creators;
using Common.Models;
using WindowsInput;
using Common.Commands;
using Common.Helpers;
using System.Timers;

namespace TwitchBot.BotCommands
{
    [Serializable]
    public class BotCommandsRepository
    {
        private readonly string botCommandPool_PINGCOMMAND = "PING :tmi.twitch.tv";
        private readonly string replayPath;
        private readonly string clipPath;
        private readonly CommandSaver commandSaver;

        private readonly List<string> specialCommands; //special commands need some special command parsing so it can read command and input user typed

        private Dictionary<string, BotCommand> commandPool;


        public BotCommandsRepository(bool isReplayEnabled, string replayPath)
        {
            this.replayPath = (replayPath == null) ? string.Empty : replayPath;
            this.clipPath = Directory.GetCurrentDirectory() + "/" + "Clips";
            specialCommands = GetSpecialCommandNames();
            commandPool = new Dictionary<string, BotCommand>();
            commandSaver = new CommandSaver();
            AddBuiltInCommands(commandPool);
           // AddPredefinedCommands(commandPool); //This should be tool method
            AddPredefinedCommandsFromXML(isReplayEnabled);

            AddAllCommandsCommand(commandPool);
        }

        private void AddPredefinedCommandsFromXML(bool isReplayEnabled)
        {
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";
            
            if (!File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                return;
            }

            List<BotCommand> botCommands = new List<BotCommand>();

            var serializer = new XmlSerializer(botCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamReader reader = new StreamReader(serializablesFolderPath + "/" + filename + ".xml"))
            {
                botCommands = (List<BotCommand>)serializer.Deserialize(reader);
            }

            filename = "buildInCommands";

            if (!File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                return;
            }

            using (StreamReader reader = new StreamReader(serializablesFolderPath + "/" + filename + ".xml"))
            {
                botCommands.AddRange((List<BotCommand>)serializer.Deserialize(reader));
            }


            foreach (BotCommand bc in botCommands)
            {
                if (bc.Command == "!replay" && !isReplayEnabled)
                {
                    continue;
                }

                commandPool[bc.Command] = bc;
            }
        }

        private void AddBuiltInCommands(Dictionary<string, BotCommand> commandPools) //Make gamesplayed command which will contain game that has been played and for how long
        {            
            commandPool.Add("!replayfeature", new BotCommand("!replayfeature", "You can use '!replay' to get an instant replay PogChamp", UserType.Regular, false, true));
            commandPool.Add("!follow", new BotCommand("!follow", "Hey you! If you haven't already followed, now is a good chance! Come over to the twitch.tv/{1} and give that cool streamer some love! <3 ", UserType.Mod, true, false, CommandType.UserInputCommand));
            commandPool.Add("!gamesplayed", new BotCommand("!gamesplayed", "Games played this stream are: {0}", UserType.Regular, true, false, CommandType.TwitchStatusCommand));
            commandPool.Add("!addcommand", new BotCommand("!addcommand", "", UserType.Mod, false, false, CommandType.AddCommand));
            commandPool.Add("!editcommand", new BotCommand("!editcommand", "", UserType.Mod, false, false, CommandType.EditCommand));
            commandPool.Add("!title", new BotCommand("!title", "{0} changed title to: {1}", UserType.Mod, true, false, CommandType.ChangeTitleCommand));
            //commandPool.Add("!sr", new BotCommand("!sr", "", UserType.King, false, false, CommandType.SongRequestCommand));
            commandPool.Add("!replay", new BotCommand("!replay", "", UserType.Regular, false, false, CommandType.MediaCommand));
            commandPool.Add("!clip", new BotCommand("!clip", "", UserType.Regular, false, false, CommandType.CreateClip));
            commandPool.Add("!remindme", new BotCommand("!remindme", "", UserType.Regular, false, false, CommandType.Reminder));
            commandPool.Add("!vote", new BotCommand("!vote", "", UserType.Regular, false, false, CommandType.Vote));

            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "buildInCommands";
            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                //File.Delete(serializablesFolderPath + "/" + filename + ".xml");
                return;
            }

            Directory.CreateDirectory(serializablesFolderPath);

            FileCreator fileCreator = new FileCreator();
            fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");

            List<BotCommand> listBotCommands = new List<BotCommand>(commandPool.Values);

            var serializer = new XmlSerializer(listBotCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, listBotCommands);
            }
        }

        private void AddPredefinedCommands(Dictionary<string, BotCommand> commandPool) //Make gamesplayed command which will contain game that has been played and for how long
        {      
            commandPool.Add("!hello", new BotCommand("!hello", "Hello to you, {0}! <3", UserType.Regular, true));
            commandPool.Add("Kappa", new BotCommand("Kappa", "Kappa"));
            commandPool.Add("PogChamp", new BotCommand("PogChamp", "PogChamp"));
            commandPool.Add("LUL", new BotCommand("LUL", "LUL indeed"));
            commandPool.Add("LuL", new BotCommand("LuL", "LUL > LuL Kappa"));
            commandPool.Add("!dance", new BotCommand("!dance", "PepePls PepePls PepePls", UserType.Regular, false, true));
            commandPool.Add("!streamer", new BotCommand("!streamer", "My name is Deni, I like video games :)", UserType.Regular, false, true));
            commandPool.Add("!twitter", new BotCommand("!twitter", "Follow me on twitter for schedule, updates, everything you want to know! www.twitter.com/denikarabenc", UserType.Regular, false, true));
            commandPool.Add("!discord", new BotCommand("!discord", "Join our little discord server in the making! discordapp.com/invite/ct6XeDD", UserType.Regular, false, true));
            commandPool.Add("!games", new BotCommand("!games", "This is variety stream, but our main games are Rocket league, Starcraft, PUBG, Overwatch... Usually Kappa"));
            commandPool.Add("!cam", new BotCommand("!cam", "Deni is using experimental feature where you can control if camera is on or off. Use !camON or !camOFF to control it"));
            commandPool.Add("!camON", new BotCommand("!camON", "Jebaited"));
            commandPool.Add("!camOFF", new BotCommand("!camOFF", "Ok FeelsBadMan"));
            commandPool.Add("!schedule", new BotCommand("!schedule", "There is no strong schedule, but I'll try to stream every work day from 18h CET (17h GMT, 12 EST)"));           

            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";
            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.Delete(serializablesFolderPath + "/" + filename + ".xml");
            }

            Directory.CreateDirectory(serializablesFolderPath);

            FileCreator fileCreator = new FileCreator();
            fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");

            List<BotCommand> listBotCommands = new List<BotCommand>(commandPool.Values);

            var serializer = new XmlSerializer(listBotCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, listBotCommands);
            }

            AddAllCommandsCommand(commandPool);
        }

        private void AddAllCommandsCommand(Dictionary<string, BotCommand> commandPool) //obsolette
        {
            string allCommands = string.Empty;

            var enumerator = commandPool.GetEnumerator();

            allCommands += "List of commands are: " + enumerator.Current.Key + " ";

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key.StartsWith("!"))
                    allCommands += enumerator.Current.Key + " ";
            }

            commandPool.Add("!commands", new BotCommand("!commands", allCommands));
        }
        
        private List<string> GetSpecialCommandNames()
        {
            List<string> specialCommandNamesList = new List<string>(5);

            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_ADDCOMMAND);
            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_EDITCOMMAND);
            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_TITLE);
            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_FOLLOW);
            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_SONGREQUESTCOMMAND);

            return specialCommandNamesList;
        }

        private bool CommandExists(string command)
        {
            if (commandPool.ContainsKey(command))
            {
                return true;
            }

            return false;
        }

        private BotCommand GetBotCommand(string command)
        {
            if (!CommandExists(command))
            {
                return null;
            }
            return commandPool[command];
        }

        public void UpdatePredefinedCommandsFromXML(bool isReplayEnabled)
        {
            BotLogger.Logger.Log(LoggingType.Info, "[BotCommandRepository] -> Updating commands from XML");
            commandPool = new Dictionary<string, BotCommand>();
            AddPredefinedCommandsFromXML(isReplayEnabled);
        }

        public List<BotCommand> GetTimedCommands()
        {
            List<BotCommand> bc = new List<BotCommand>();
            foreach (BotCommand botCommand in commandPool.Values)
            {
                if (botCommand.IsTimed)
                {
                    bc.Add(botCommand);
                }
            }

            return bc;
        }

        public CommandType GetCommandType(string message)
        {
            if (message.Contains(botCommandPool_PINGCOMMAND))
            {
                return CommandType.Ping;
            }

            if (message.Contains("NOTICE") && message.Contains("The moderators of this room are"))
            {
                return CommandType.ModsRequest;
            }

            if (message == "!commands")
            {
                return CommandType.CommandList;
            }

            if (CommandExists(message))
            {
                return GetBotCommand(message).Type;
            }
            else
            {
                return CommandType.NotExist;
            }
        }

        public UserType GetCommandPermissions(string parsedMessage)
        {
            string command = parsedMessage.Split(' ')[0];

            if (specialCommands.Contains(command))
            {
                return GetBotCommand(command).UserPermission;
            }

            if (CommandExists(parsedMessage))
            {
                return GetBotCommand(parsedMessage).UserPermission;
            }

            return UserType.Invalid;
        }

        public string GetAllCommands()
        {
            string allCommands = string.Empty;

            var enumerator = commandPool.GetEnumerator();

            allCommands += "List of commands are: " + enumerator.Current.Key + " ";

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key.StartsWith("!"))
                    allCommands += enumerator.Current.Key + " ";
            }

            return allCommands;
        }

        public string AddCommandAndGetFeedback(string command, string message, Action callback)
        {
            if (commandPool.ContainsKey(command))
            {
                return string.Format("{0} {1} {2}. {3} {4} {5}",
                    Properties.Resources.botCommandPool_COMMAND, command, Properties.Resources.botCommandPool_ALREADY_EXIST, Properties.Resources.botCommandPool_USE, Properties.Resources.botCommandPool_EDITCOMMAND, Properties.Resources.botCommandPool_INSTEAD);
            }

            commandPool.Add(command, new BotCommand(command, message, CommandType.UserInputCommand));
            commandSaver.AddCommandToXML(command, message);
            StringBuilder sb = new StringBuilder();
            callback?.Invoke();
            return string.Format("{0} {1} {2}", Properties.Resources.botCommandPool_COMMAND, command, TwitchBot.Properties.Resources.botCommandPool_ADDED);
        }

        public string EditCommandAndGetFeedback(string command, string newMessage, Action callback)
        {
            if (!commandPool.ContainsKey(command))
            {
                return string.Format("{0} {1} {2}. {3} {4} {5}", Properties.Resources.botCommandPool_COMMAND, command, Properties.Resources.botCommandPool_DOES_NOT_EXIST, Properties.Resources.botCommandPool_USE, Properties.Resources.botCommandPool_ADDCOMMAND, TwitchBot.Properties.Resources.botCommandPool_INSTEAD);
            }

            BotCommand oldCommand = commandPool[command];

            commandPool[command] = new BotCommand(command, newMessage, oldCommand.UserPermission, oldCommand.UseAppendedStrings, oldCommand.IsTimed, oldCommand.Type);
            commandSaver.RemoveCommandFromXML(command);
            commandSaver.AddCommandToXML(command, newMessage);
            callback?.Invoke();
            StringBuilder sb = new StringBuilder();
            return string.Format("{0} {1} {2} {3}", Properties.Resources.botCommandPool_COMMAND, command, Properties.Resources.botCommandPool_EDITED_TO, newMessage);
        }

        public string GetReadCommandMessage(string command, string userWhoSentTheCommand) //Make string[] params
        {
            if (commandPool[command].UseAppendedStrings)
            {
                return string.Format(commandPool[command].Message, userWhoSentTheCommand);
            }
            else
            {
                return commandPool[command].Message;
            }
        }

        public string GetUserInputCommandMessage(string command, string userWhoSentTheCommand, string userInput) //Make string[] params
        {
            if (commandPool[command].UseAppendedStrings)
            {
                return string.Format(commandPool[command].Message, userWhoSentTheCommand, userInput);
            }
            else
            {
                return commandPool[command].Message;
            }
        }

        public string GetTwitchStatusCommandMessage(string command, string customParams)
        {
            if (commandPool[command].UseAppendedStrings)
            {
                return string.Format(commandPool[command].Message, customParams);
            }
            else
            {
                return commandPool[command].Message;
            }
        }

        public string GetMediaCommandFileNameAndPath()
        {
            DateTime requestedTime = DateTime.Now;
            InputSimulator.SimulateKeyDown(VirtualKeyCode.F13);
            System.Threading.Thread.Sleep(200);         
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F13);

            System.Threading.Thread.Sleep(5000);

            if (!Directory.Exists(replayPath))
            {
                BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> Invalid replay path! Directory does not exist!");
                return String.Empty;
            }
            var directory = new DirectoryInfo(replayPath);
            if (directory.GetFiles().Length == 0)
            {
                BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> No files in directory!");
                return String.Empty;
            }
            
            FileInfo myFile = directory.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();

            if (requestedTime > myFile.CreationTime)
            {
                BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> No files in directory!");
                return String.Empty;
            }

            if (myFile.Extension != ".mp4")
            {              
                BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> Latest file created is not .mp4");
                return String.Empty;
            }

            if (replayPath.EndsWith("/") || replayPath.EndsWith(@"\"))
            {
                return replayPath + myFile.Name;
            }
            else
            {
                return replayPath + "/" + myFile.Name;
            }
            
        }

        public void CreateClipCommandFileNameAndPath(string clipId)
        {
            Directory.CreateDirectory(clipPath);
            FileCreator fc = new FileCreator();
            fc.CreateFileIfNotExist(clipPath, "clipHTML", "html");

            //clipId = "EphemeralAntsyChoughDeIlluminati";
            using (StreamWriter writer = new StreamWriter(clipPath+"/" + "clipHTML.html", false))
            {
                writer.Write(@"<!DOCTYPE html><html><head><title> Clip </title></head><body><iframe src=""https://clips.twitch.tv/embed?clip=" + clipId + @"&autoplay=true"" width=""1024"" height=""575"" frameborder=""0"" scrolling=""no"" allowfullscreen=""false"" ></iframe></body></html>");
            }
            
            InputSimulator.SimulateKeyDown(VirtualKeyCode.F14);
            System.Threading.Thread.Sleep(200);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F14);
            Timer clipTimer = new Timer(31000);
            clipTimer.AutoReset = false;
            clipTimer.Enabled = true;
            clipTimer.Elapsed += ClipTimer_Elapsed;
            
            //System.Threading.Thread.Sleep(5000);

            //if (!Directory.Exists(replayPath))
            //{
            //    BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> Invalid replay path! Directory does not exist!");
            //    return String.Empty;
            //}
            //var directory = new DirectoryInfo(replayPath);
            //if (directory.GetFiles().Length == 0)
            //{
            //    BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> No files in directory!");
            //    return String.Empty;
            //}

            //FileInfo myFile = directory.GetFiles()
            // .OrderByDescending(f => f.LastWriteTime)
            // .First();

            //if (requestedTime > myFile.CreationTime)
            //{
            //    BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> No files in directory!");
            //    return String.Empty;
            //}

            //if (myFile.Extension != ".mp4")
            //{
            //    BotLogger.Logger.Log(LoggingType.Warning, "[GetMediaCommandFileName] -> Latest file created is not .mp4");
            //    return String.Empty;
            //}

            //if (replayPath.EndsWith("/") || replayPath.EndsWith(@"\"))
            //{
            //    return replayPath + myFile.Name;
            //}
            //else
            //{
            //    return replayPath + "/" + myFile.Name;
            //}

        }

        private void ClipTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.F14);
            System.Threading.Thread.Sleep(200);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F14);
        }
    }
}

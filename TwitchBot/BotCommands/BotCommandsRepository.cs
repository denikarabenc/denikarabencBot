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

namespace TwitchBot.BotCommands
{
    [Serializable]
    public class BotCommandsRepository
    {
        private readonly string botCommandPool_PINGCOMMAND = "PING :tmi.twitch.tv";

        private readonly List<string> specialCommands; //special commands need some special command parsing so it can read command and input user typed

        private Dictionary<string, BotCommand> commandPool;        

        public BotCommandsRepository()
        {            
            specialCommands = GetSpecialCommandNames();
            commandPool = new Dictionary<string, BotCommand>();
           // AddPredefinedCommands(commandPool); //This should be tool method
            AddPredefinedCommandsFromXML();
        }

        private void AddPredefinedCommandsFromXML()
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

            foreach (BotCommand bc in botCommands)
            {
                commandPool.Add(bc.Command, bc);
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
            commandPool.Add("!replayfeature", new BotCommand("!replayfeature", "You can use '!replay' to get an instant replay PogChamp",UserType.Regular,false, true));

            commandPool.Add("!follow", new BotCommand("!follow", "Hey you! If you haven't already followed, now is a good chance! Come over to the twitch.tv/{1} and give that cool streamer some love! <3 ", UserType.Mod, true, false, CommandType.UserInputCommand));

            commandPool.Add("!gamesplayed", new BotCommand("!gamesplayed", "Games played this stream are: {0}", UserType.Regular, true, false, CommandType.TwitchStatusCommand));

            commandPool.Add("!addcommand", new BotCommand("!addcommand", "", UserType.Mod, false, false, CommandType.AddCommand));

            commandPool.Add("!editcommand", new BotCommand("!editcommand", "", UserType.Mod, false, false, CommandType.EditCommand));

            commandPool.Add("!title", new BotCommand("!title", "{0} changed title to: {1}", UserType.Mod, true, false, CommandType.ChangeTitleCommand));

            commandPool.Add("!replay", new BotCommand("!replay", "", UserType.Regular, false, false, CommandType.MediaCommand));

            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";
            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.Delete(serializablesFolderPath + "/" + filename + ".xml");
            }

            Directory.CreateDirectory(serializablesFolderPath);

            FileCreator fileCreator = new FileCreator();
            fileCreator.CreateFile(serializablesFolderPath, filename, "xml");

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
            List<string> specialCommandNamesList = new List<string>(4);

            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_ADDCOMMAND);
            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_EDITCOMMAND);
            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_TITLE);
            specialCommandNamesList.Add(TwitchBot.Properties.Resources.botCommandPool_FOLLOW);

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

        public string AddCommandAndGetFeedback(string command, string message)
        {
            if (commandPool.ContainsKey(command))
            {
                return string.Format("{0} {1} {2}. {3} {4} {5}",
                    Properties.Resources.botCommandPool_COMMAND, command, Properties.Resources.botCommandPool_ALREADY_EXIST, Properties.Resources.botCommandPool_USE, Properties.Resources.botCommandPool_EDITCOMMAND, Properties.Resources.botCommandPool_INSTEAD);
            }

            commandPool.Add(command, new BotCommand(command, message));
            CommandSaver cs = new CommandSaver();
            cs.AddCommandToXML(command, message);
            StringBuilder sb = new StringBuilder();
            return string.Format("{0} {1} {2}", Properties.Resources.botCommandPool_COMMAND, command, TwitchBot.Properties.Resources.botCommandPool_ADDED);
        }

        public string EditCommandAndGetFeedback(string command, string newMessage)
        {
            if (!commandPool.ContainsKey(command))
            {
                return string.Format("{0} {1} {2}. {3} {4} {5}", Properties.Resources.botCommandPool_COMMAND, command, Properties.Resources.botCommandPool_DOES_NOT_EXIST, Properties.Resources.botCommandPool_USE, Properties.Resources.botCommandPool_ADDCOMMAND, TwitchBot.Properties.Resources.botCommandPool_INSTEAD);
            }

            commandPool[command] = new BotCommand(command, newMessage);
            CommandSaver cs = new CommandSaver();
            cs.RemoveCommandFromXML(command);
            cs.AddCommandToXML(command, newMessage);
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

        public string GetMediaCommandFileNameAndPath(string command)
        {
            DateTime requestedTime = DateTime.Now;
            InputSimulator.SimulateKeyDown(VirtualKeyCode.F13);
            System.Threading.Thread.Sleep(200);         
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F13);

            System.Threading.Thread.Sleep(5000);

            var directory = new DirectoryInfo(@"d:/Video/ReplayClips");
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

            return "d:/Video/ReplayClips/" + myFile.Name;    
            
        }
    }
}

using Common.Creators;
using Common.Models;
using denikarabencBot.Helpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace denikarabencBot.ViewModels
{
    public class CommandsViewModel
    {
        private bool isTimed;
        private string message;
        private string command;
        private UserType permission;
        private List<BotCommand> commandList;
        private List<UserType> permissionSource;

        private readonly CommandReader commandReader;

        public CommandsViewModel()
        {
            IsTimed = false;
            commandList = new List<BotCommand>();
            permissionSource = new List<UserType>();
            permissionSource.Add(UserType.Regular);
            permissionSource.Add(UserType.Follower);
            permissionSource.Add(UserType.Mod);
            permissionSource.Add(UserType.King);

            commandReader = new CommandReader();
        }

        public string Message { get => message; set => message = value; }
        public string Command { get => command; set => command = value; }
        public UserType Permission { get => permission; set => permission = value; }
        public List<UserType> PermissionSource { get => permissionSource; set => permissionSource = value; }
        public bool IsTimed { get => isTimed; set => isTimed = value; }

        private BotCommand AddCurrentCommand()
        {
            CommandType ct = CommandType.ReadCommand;
            //bool useAppendedString = false;
            if (Message.Contains("{1}"))
            {
                ct = CommandType.UserInputCommand;

            }
            return new BotCommand(Command, Message, Permission, true, IsTimed, ct);
        }

        public void SaveCommand()
        {
            if (String.IsNullOrEmpty(Message) || String.IsNullOrWhiteSpace(Message) || String.IsNullOrWhiteSpace(command) || String.IsNullOrWhiteSpace(command))
            {
                MessageBox.Show("Message and command cannot be empty");
                return;
            }

            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";

            var currentCommands = commandReader.GetAllCommandsFromXML();
            if (currentCommands.Count == 0)
            {
                FolderCreator folderCreator = new FolderCreator();
                folderCreator.CreateFolder(serializablesFolderPath);

                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateFile(serializablesFolderPath, filename, "xml");
            }

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
            }

            //FolderCreator folderCreator = new FolderCreator();
            //folderCreator.CreateFolder(serializablesFolderPath);

            //FileCreator fileCreator = new FileCreator();
            //fileCreator.CreateFile(serializablesFolderPath, filename, "xml");

            currentCommands.Add(AddCurrentCommand());

            var serializer = new XmlSerializer(currentCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, currentCommands);
            }
        }
    }
}

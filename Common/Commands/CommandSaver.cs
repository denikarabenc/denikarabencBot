using Common.Creators;
using Common.Models;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Common.Commands
{
    public class CommandSaver
    {
        private readonly CommandReader commandReader;

        public CommandSaver()
        {
            commandReader = new CommandReader();
        }
        
        private BotCommand CreateCurrentCommand(string command, string message, UserType permission, bool isTimed, bool isActive)
        {
            CommandType ct = CommandType.ReadCommand;
            //bool useAppendedString = false;
            if (message.Contains("{1}"))
            {
                ct = CommandType.UserInputCommand;

            }
            return new BotCommand(command, message, permission, command.Contains("{0}"), isTimed, isActive, ct);
        }

        public void EditCommand(BotCommand oldCommand, BotCommand newCommand)
        {
            RemoveCommandFromXML(oldCommand);
            AddCommandToXML(newCommand.Command, newCommand.Message, newCommand.UserPermission, newCommand.IsTimed, newCommand.IsActive);
        }

        public void AddCommandToXML(string command, string message)
        {
            AddCommandToXML(command, message, UserType.Regular, false, true);
        }

        public void AddCommandToXML(string command, string message, bool isTimed, bool isActive)
        {
            AddCommandToXML(command, message, UserType.Regular, isTimed, isActive);
        }

        public void AddCommandToXML(string command, string message, UserType permission, bool isTimed, bool isActive)
        {          
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";

            var currentCommands = commandReader.GetAllCommandsFromXML();
            if (currentCommands.Count == 0)
            {
                Directory.CreateDirectory(serializablesFolderPath);

                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");
            }

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
            }

            //FolderCreator folderCreator = new FolderCreator();
            //folderCreator.CreateFolder(serializablesFolderPath);

            //FileCreator fileCreator = new FileCreator();
            //fileCreator.CreateFile(serializablesFolderPath, filename, "xml");

            currentCommands.Add(CreateCurrentCommand(command, message, permission, isTimed, isActive));

            var serializer = new XmlSerializer(currentCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, currentCommands);
            }
        }

        public void RemoveCommandFromXML(string command)
        {
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";

            var currentCommands = commandReader.GetAllCommandsFromXML();

            var commandForRemoval = currentCommands.Where(c => c.Command == command).First();

            if (!currentCommands.Contains(commandForRemoval))
            {
                return;
            }

            if (currentCommands.Count == 0)
            {
                Directory.CreateDirectory(serializablesFolderPath);

                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");
            }

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
            }

            //FolderCreator folderCreator = new FolderCreator();
            //folderCreator.CreateFolder(serializablesFolderPath);

            //FileCreator fileCreator = new FileCreator();
            //fileCreator.CreateFile(serializablesFolderPath, filename, "xml");                       

            currentCommands.Remove(commandForRemoval);

            var serializer = new XmlSerializer(currentCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, currentCommands);
            }
        }

        public void RemoveCommandFromXML(BotCommand botCommand)
        {
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";

            var currentCommands = commandReader.GetAllCommandsFromXML();

            var commandForRemoval = currentCommands.Where(bc => bc.Command == botCommand.Command && bc.IsTimed == botCommand.IsTimed && bc.Message == botCommand.Message && bc.Type == botCommand.Type && bc.UseAppendedStrings == botCommand.UseAppendedStrings && bc.UserPermission == botCommand.UserPermission).FirstOrDefault();
            //var commandsForRemoval = currentCommands.Where(c => c.Command == botCommand.Command);
            //BotCommand commandForRemoval;

            if (commandForRemoval == null)
            {
                return; //TODO logging in common project
            }
            if (!currentCommands.Contains(commandForRemoval))
            {
                return;
            }

            if (currentCommands.Count == 0)
            {
                Directory.CreateDirectory(serializablesFolderPath);

                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");
            }

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
            }

            //FolderCreator folderCreator = new FolderCreator();
            //folderCreator.CreateFolder(serializablesFolderPath);

            //FileCreator fileCreator = new FileCreator();
            //fileCreator.CreateFile(serializablesFolderPath, filename, "xml");                       

            currentCommands.Remove(commandForRemoval);

            var serializer = new XmlSerializer(currentCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, currentCommands);
            }
        }
    }
}

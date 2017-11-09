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
        
        private BotCommand CreateCurrentCommand(string command, string message, UserType permission, bool isTimed)
        {
            CommandType ct = CommandType.ReadCommand;
            //bool useAppendedString = false;
            if (message.Contains("{1}"))
            {
                ct = CommandType.UserInputCommand;

            }
            return new BotCommand(command, message, permission, true, isTimed, ct);
        }

        public void AddCommandToXML(string command, string message)
        {
            AddCommandToXML(command, message, UserType.Regular, false);
        }

        public void AddCommandToXML(string command, string message, UserType permission, bool isTimed)
        {          
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";

            var currentCommands = commandReader.GetAllCommandsFromXML();
            if (currentCommands.Count == 0)
            {
                Directory.CreateDirectory(serializablesFolderPath);

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

            currentCommands.Add(CreateCurrentCommand(command, message, permission, isTimed));

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

            currentCommands.Remove(commandForRemoval);

            var serializer = new XmlSerializer(currentCommands.GetType(), new XmlRootAttribute("commands"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, currentCommands);
            }
        }
    }
}

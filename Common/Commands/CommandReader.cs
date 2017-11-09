using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace Common.Commands
{
    public class CommandReader
    {
        public List<BotCommand> GetAllCommandsFromXML()
        {
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "commands";

            List<BotCommand> botCommands = new List<BotCommand>();

            if (!File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                return botCommands;
            }

            var serializer = new XmlSerializer(botCommands.GetType(), new XmlRootAttribute("commands"));

            try
            {
                using (StreamReader reader = new StreamReader(serializablesFolderPath + "/" + filename + ".xml"))
                {
                    botCommands = (List<BotCommand>)serializer.Deserialize(reader);
                }
            }
            catch(Exception)
            {
                
                return botCommands;
            }

            return botCommands;
        }
    }
}

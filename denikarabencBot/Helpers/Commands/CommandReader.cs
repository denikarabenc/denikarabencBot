using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TwitchBot1.BotCommands;

namespace denikarabencBot.Helpers.Commands
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
            catch
            {
                return botCommands;
            }

            return botCommands;
        }
    }
}

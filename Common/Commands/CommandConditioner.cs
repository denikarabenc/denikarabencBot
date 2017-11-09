using System;
using System.Linq;

namespace Common.Commands
{
    public class CommandConditioner
    {
        public bool CanSave(string command, string message)
        {
            return !((String.IsNullOrEmpty(message) || String.IsNullOrWhiteSpace(message) || String.IsNullOrWhiteSpace(command) || String.IsNullOrWhiteSpace(command)));            
        }

        public bool CanRemove(string command)
        {
            CommandReader commandReader = new CommandReader();
            var currentCommands = commandReader.GetAllCommandsFromXML();

            var commandForRemoval = currentCommands.Where(c => c.Command == command).First();

            if (!currentCommands.Contains(commandForRemoval))
            {
                return false;
            }

            return true;
        }
    }
}

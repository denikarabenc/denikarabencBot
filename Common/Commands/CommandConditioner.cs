using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Commands
{
    public class CommandConditioner
    {
        public bool CanAdd(string command, string message)
        {
            return !((string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(command) || string.IsNullOrWhiteSpace(command)));            
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

        //public bool CanEdit(string command, string message, List<BotCommand> commandList)
        //{
        //    return (!(string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) && commandList.Any(x => x.Command == command));
        //}

        public bool CanEdit(BotCommand selectedCommand)
        {
            return selectedCommand != null;
        }
    }
}

using Common.Commands;
using Common.Models;
using System.Collections.Generic;
using System.Windows;

namespace denikarabencBot.ViewModels
{
    public class CommandsViewModel : BaseViewModel
    {
        private readonly CommandSaver commandSaver;
        private readonly CommandConditioner commandConditioner;

        private bool isTimed;
        private string message;
        private string command;
        private UserType permission;
        private List<BotCommand> commandList;
        private List<UserType> permissionSource;

        public CommandsViewModel()
        {
            this.Bot = Bot;
            commandSaver = new CommandSaver();
            commandConditioner = new CommandConditioner();

            IsTimed = false;
            commandList = new List<BotCommand>();

            permissionSource = new List<UserType>();
            permissionSource.Add(UserType.Regular);
            permissionSource.Add(UserType.Follower);
            permissionSource.Add(UserType.Mod);
            permissionSource.Add(UserType.King);
        }

        public string Message { get => message; set => message = value; }
        public string Command { get => command; set => command = value; }
        public UserType Permission { get => permission; set => permission = value; }
        public List<UserType> PermissionSource { get => permissionSource; set => permissionSource = value; }
        public bool IsTimed { get => isTimed; set => isTimed = value; }

        public void SaveCommand()
        {
            if (!commandConditioner.CanSave(command, message))
            {
                MessageBox.Show("Command has to have field command and message populated!");
                return;
            }

            commandSaver.AddCommandToXML(Command, Message, Permission, IsTimed);

            Bot?.CommandPool.UpdatePredefinedCommandsFromXML(true); //TODO
        }
    }
}

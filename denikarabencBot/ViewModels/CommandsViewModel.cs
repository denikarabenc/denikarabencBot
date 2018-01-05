using BotLogger;
using Common.Commands;
using Common.Models;
using Common.WPFCommand;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace denikarabencBot.ViewModels
{
    public class CommandsViewModel : INotifyPropertyChanged
    {
        private readonly CommandSaver commandSaver;
        private readonly CommandReader commandReader;
        private readonly CommandConditioner commandConditioner;

        private bool isTimed;
        private string message;
        private string command;
        private UserType permission;
        private BotCommand selectedCommand;
        private List<BotCommand> commandList;
        private List<UserType> permissionSource;

        public ICommand addCommandCommand;
        public ICommand removeSelectedCommandCommand;

        public CommandsViewModel()
        {
            commandSaver = new CommandSaver();
            commandReader = new CommandReader();
            commandConditioner = new CommandConditioner();

            IsTimed = false;
            commandList = new List<BotCommand>();
            RefreshCommandList();

            permissionSource = new List<UserType>();
            permissionSource.Add(UserType.Regular);
            permissionSource.Add(UserType.Follower);
            permissionSource.Add(UserType.Mod);
            permissionSource.Add(UserType.King);
        }

        public ICommand AddCommandCommand
        {
            get => addCommandCommand ?? (addCommandCommand = new RelayCommand(param => SaveCommand(), param => CanAddCommandCommandExecute()));
        }

        public ICommand RemoveSelectedCommandCommand
        {
            get => removeSelectedCommandCommand ?? (removeSelectedCommandCommand = new RelayCommand(param => RemoveSelectedCommandCommandExecute(), param => CanRemoveSelectedCommandCommandExecute()));
        }

        public string Message { get => message; set => message = value; }
        public string Command { get => command; set => command = value; }
        public UserType Permission { get => permission; set => permission = value; }
        public List<UserType> PermissionSource { get => permissionSource; set => permissionSource = value; }
        public bool IsTimed { get => isTimed; set => isTimed = value; }
        public List<BotCommand> CommandList { get => commandList; }
        public BotCommand SelectedCommand { get => selectedCommand; set => selectedCommand = value; }

        private bool CanAddCommandCommandExecute()
        {
            return commandConditioner.CanSave(Command, Message);
        }

        private void SaveCommand()
        {
            Logger.Log(LoggingType.Info, $"Command {Command} added");

            if (!commandConditioner.CanSave(command, message))
            {
                MessageBox.Show("Command has to have field command and message populated!");
                return;
            }

            commandSaver.AddCommandToXML(Command, Message, Permission, IsTimed);

            RefreshCommandList();

            //Bot?.CommandPool.UpdatePredefinedCommandsFromXML(true); //TODO
        }

        private bool CanRemoveSelectedCommandCommandExecute()
        {
            return SelectedCommand != null;
        }

        private void RemoveSelectedCommandCommandExecute()
        {
            commandSaver.RemoveCommandFromXML(SelectedCommand);
            Logger.Log(LoggingType.Info, string.Format("[CommandsViewModel] -> Command {0} removed!", SelectedCommand.Command));
            RefreshCommandList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<BotCommand> FilterCommandsList(List<BotCommand> commandList)
        {
            foreach (string commandName in CommandsNotToShowInList())
            {
                BotCommand botCommand = commandList.Where(bc => bc.Command == commandName).FirstOrDefault();
                commandList.Remove(botCommand);
            }

            return commandList;
        }

        private List<string> CommandsNotToShowInList()
        {
            return new List<string>
            {
                "!sr",
                "!replay",
                "!addcommand",
                "!editcommand",
            };
        }

        private void RefreshCommandList()
        {
            commandList = commandReader.GetAllCommandsFromXML();
            commandList = FilterCommandsList(commandList);
            OnPropertyChanged(nameof(CommandList));
        }
    }
}

using BotCore.BotCommands;
using BotLogger;
using Common.Commands;
using Common.Helpers;
using Common.Models;
using Common.WPFCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace denikarabencBot.ViewModels
{
    public class CommandWindowViewModel : INotifyPropertyChanged
    {
        private ICommand okCommand;

        private readonly bool isEdit;
        private readonly CommandConditioner commandConditioner;
        private readonly CommandSaver commandSaver;        
        private readonly Action okFinished;

        private BotCommand selectedCommandForEdit;
        private string buttonClickedMessage;

        public CommandWindowViewModel(BotCommand selectedCommandForEdit, CommandConditioner commandConditioner, Action okFinished, bool isEdit)
        {
            commandSaver = new CommandSaver();

            commandConditioner.ThrowIfNull(nameof(commandConditioner));
            this.commandConditioner = commandConditioner;
            this.selectedCommandForEdit = selectedCommandForEdit;
            this.okFinished = okFinished;
            this.isEdit = isEdit;

            IsActive = true;
            PermissionSource = GetSupportedPermissions();
            InitializeCommandValues();
        }

        public bool IsTimed { get; set; }
        public bool IsActive { get; set; }

        public string Message { get; set; }
        public string Command { get; set; }

        public string ButtonClickedMessage
        {
            get
            {
                return buttonClickedMessage;
            }
            set
            {
                buttonClickedMessage = value;
                OnPropertyChanged(nameof(ButtonClickedMessage));
            }
        }

        public UserType Permission { get; set; }
        public List<UserType> PermissionSource { get; private set; }

        public ICommand OKCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(param => OKCommandExecute(), param => OKCommandCanExecute()));
        }

        private void OKCommandExecute()
        {
            if (isEdit)
            {
                EditCommand();
                
            }
            else
            {
                AddCommand();
            }

            okFinished?.Invoke();
        }

        private void AddCommand()
        {
            if (!commandConditioner.CanAdd(Command, Message))
            {
                MessageBox.Show("Command has to have field command and message populated!");
                return;
            }

            commandSaver.AddCommandToXML(Command, Message, Permission, IsTimed, IsActive);

            ButtonClickedMessage = $"Command {Command} added!";

            Logger.Log(LoggingType.Info, $"Command {Command} added");
        }

        private void EditCommand()
        {
            if (selectedCommandForEdit == null)
            {
                Logger.Log(LoggingType.Warning, "No selected command for edit!");
                ButtonClickedMessage = "Error occured!";
                return;
            }

            if (!commandConditioner.CanEdit(selectedCommandForEdit))
            {
                MessageBox.Show("Command has to have field command and message populated!");
                return;
            }

            commandSaver.EditCommand(selectedCommandForEdit, new BotCommand(Command, Message, Permission, Command.Contains("{0}"), IsTimed, IsActive));

            selectedCommandForEdit = new BotCommand(Command, Message, Permission, Command.Contains("{0}"), IsTimed, IsActive);

            ButtonClickedMessage = "Command seccessfully edited!";
        }

        private bool OKCommandCanExecute()
        {
            return commandConditioner.CanAdd(Command, Message);
        }

        private void InitializeCommandValues()
        {
            if (selectedCommandForEdit != null && isEdit)
            {
                IsTimed = selectedCommandForEdit.IsTimed;
                IsActive = selectedCommandForEdit.IsActive;
                Command = selectedCommandForEdit.Command;
                Message = selectedCommandForEdit.Message;
                Permission = selectedCommandForEdit.UserPermission;
            }
        }

        private List<UserType> GetSupportedPermissions()
        {
            List<UserType> permissionSource = new List<UserType>(4);
            permissionSource.Add(UserType.Regular);
            permissionSource.Add(UserType.Follower);
            permissionSource.Add(UserType.Mod);
            permissionSource.Add(UserType.King);

            return permissionSource;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

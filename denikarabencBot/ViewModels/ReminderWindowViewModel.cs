using Common.Helpers;
using Common.Models;
using Common.Reminders;
using Common.WPFCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace denikarabencBot.ViewModels
{
    public class ReminderWindowViewModel : INotifyPropertyChanged
    {
        private ReminderService reminderService;
        private List<Reminder> reminderList;
        private Reminder selectedReminder;
        private ICommand removeReminderCommand;

        public ReminderWindowViewModel(ReminderService reminderService)
        {
            reminderService.ThrowIfNull(nameof(reminderService));
            this.reminderService = reminderService;
            this.reminderList = reminderService.GetAllReminders();
        }

        public List<Reminder> ReminderList
        {
            get
            {
                return reminderList;
            }
            set
            {
                reminderList = value;
                OnPropertyChanged(nameof(ReminderList));
            }
        }

        public ICommand RemoveReminderCommand
        {
            get => removeReminderCommand ?? (removeReminderCommand = new RelayCommand(param => RemoveReminderExecute(), param => RemoveReminderCanExecute()));
        }
        public Reminder SelectedReminder { get => selectedReminder; set => selectedReminder = value; }

        private void RemoveReminderExecute()
        {
            reminderService.RemoveReminder(SelectedReminder);
            ReminderList = reminderService.GetAllReminders();
            OnPropertyChanged(nameof(ReminderList));
        }

        private bool RemoveReminderCanExecute()
        {
            return SelectedReminder!= null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

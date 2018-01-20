using Common.Models;
using Common.WPFCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace denikarabencBot.ViewModels
{
    public class ReminderWindowViewModel
    {
        private List<Reminder> reminderList;
        private ICommand removeReminderCommand;

        public ReminderWindowViewModel(List<Reminder> reminderList)
        {
            this.reminderList = reminderList;
        }

        public List<Reminder> ReminderList { get => reminderList; set => reminderList = value; }

        public ICommand RemoveReminderCommand
        {
            get => removeReminderCommand ?? (removeReminderCommand = new RelayCommand(param => RemoveReminderExecute(), param => RemoveReminderCanExecute()));
        }

        private void RemoveReminderExecute()
        {
            //TODO
        }

        private bool RemoveReminderCanExecute()
        {
            return false; //TODO
        }
    }
}

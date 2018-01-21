using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Reminders
{
    public class ReminderService
    {
        private ReminderRepository reminders;

        public ReminderService()
        {
            reminders = new ReminderRepository();
        }

        public void AddReminder(Reminder reminder)
        {
            reminders.ReminderPool.Add(reminder);
            reminders.AddReminderToXML(reminder);
        }

        public void RemoveReminder(Reminder reminder)
        {
            //reminders.ReminderPool.Remove(reminder); //TODO
            reminders.RemoveReminderFromXML(reminder);
        }

        public List<Reminder> GetAllReminders()
        {
            //return reminders.ReminderPool;
            return reminders.GetRemindersFromXML();
        }
    }
}

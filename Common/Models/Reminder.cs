using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Reminder
    {
        bool isCompleted;
        string user;
        string message;

        public Reminder()
        {
            isCompleted = false;
        }

        public Reminder(string user, string message)
        {
            this.user = user;
            this.message = message;
            isCompleted = false;
        }

        public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
        public string User { get => user; set => user = value; }
        public string Message { get => message; set => message = value; }
    }
}

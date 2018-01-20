using Common.Creators;
using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Reminders
{
    public class ReminderRepository //TODO make internal?
    {
        private List<Reminder> reminderPool;

        public List<Reminder> ReminderPool { get => reminderPool; set => reminderPool = value; }

        public ReminderRepository()
        {
            InitializeReminderXML();
            ReminderPool = GetRemindersFromXML();
        }


        public List<Reminder> GetRemindersFromXML()
        {
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "reminders";

            if (!File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                return new List<Reminder>();
            }

            List<Reminder> reminders = new List<Reminder>();

            var serializer = new XmlSerializer(reminders.GetType(), new XmlRootAttribute("reminders"));
            
            using (StreamReader reader = new StreamReader(serializablesFolderPath + "/" + filename + ".xml"))
            {
                reminders = (List<Reminder>)serializer.Deserialize(reader);
            }

            return reminders;
        }

        public void AddReminderToXML(Reminder reminder)
        {
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "reminders";

            var currentReminders = GetRemindersFromXML();
            if (currentReminders.Count == 0)
            {
                Directory.CreateDirectory(serializablesFolderPath);

                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateFile(serializablesFolderPath, filename, "xml");
            }

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
            }

            //FolderCreator folderCreator = new FolderCreator();
            //folderCreator.CreateFolder(serializablesFolderPath);

            //FileCreator fileCreator = new FileCreator();
            //fileCreator.CreateFile(serializablesFolderPath, filename, "xml");

            currentReminders.Add(reminder);

            var serializer = new XmlSerializer(currentReminders.GetType(), new XmlRootAttribute("reminders"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, currentReminders);
            }
        }

        private void InitializeReminderXML()
        {
            string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            string filename = "reminders";

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                return;
            }
            Directory.CreateDirectory(serializablesFolderPath);

            FileCreator fileCreator = new FileCreator();
            fileCreator.CreateFile(serializablesFolderPath, filename, "xml");

            List<Reminder> reminders = new List<Reminder>();
            var serializer = new XmlSerializer(reminders.GetType(), new XmlRootAttribute("reminders"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, reminders);
            }
        }
    }
}

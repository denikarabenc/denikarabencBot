using Common.Creators;
using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Voting
{
    public class VotingRepository //TODO make internal?
    {
        private readonly string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
        private readonly string filename = "votes";
        private List<Vote> votePool;

        public List<Vote> VotePool { get => votePool; set => votePool = value; }

        public VotingRepository()
        {
            InitializeVoteXML();
            votePool = GetVotesFromXML();
        }


        public List<Vote> GetVotesFromXML()
        {
          //  string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            //string filename = "votes";

            if (!File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                return new List<Vote>();
            }

            List<Vote> votes = new List<Vote>();

            var serializer = new XmlSerializer(votes.GetType(), new XmlRootAttribute("votes"));

            using (StreamReader reader = new StreamReader(serializablesFolderPath + "/" + filename + ".xml"))
            {                
                votes = (List<Vote>)serializer.Deserialize(reader);                           
            }

            return votes;
        }

        public void AddVoteToXML(Vote vote)
        {
            //string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            //string filename = "votes";

            var currentVotes = GetVotesFromXML();
            if (currentVotes.Count == 0)
            {
                Directory.CreateDirectory(serializablesFolderPath);

                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");
            }
            
            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
            }

            bool voteAlreadyAdded = false;

            for (int i = 0; i < currentVotes.Count; i++)
            {
                if (currentVotes[i].User == vote.User && currentVotes[i].VoteCategory == vote.VoteCategory)
                {
                    currentVotes[i].VoteChoice = vote.VoteChoice;
                    voteAlreadyAdded = true;
                    break;
                }
            }

            if (!voteAlreadyAdded)
            {
                currentVotes.Add(vote);
            }

            var serializer = new XmlSerializer(currentVotes.GetType(), new XmlRootAttribute("votes"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, currentVotes);
            }
        }

        //public void RemoveReminderFromXML(Vote vote)
        //{
        //    string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
        //    string filename = "vote";

        //    var currentReminders = GetVotesFromXML();

        //    var reminderForRemoval = currentReminders.Where(v => v.Message == v.Message && v.User == v.User).FirstOrDefault();
        //    //var commandsForRemoval = currentCommands.Where(c => c.Command == botCommand.Command);
        //    //BotCommand commandForRemoval;

        //    if (reminderForRemoval == null)
        //    {
        //        return; //TODO logging in common project
        //    }
        //    if (!currentReminders.Contains(reminderForRemoval))
        //    {
        //        return;
        //    }

        //    if (currentReminders.Count == 0)
        //    {
        //        Directory.CreateDirectory(serializablesFolderPath);

        //        FileCreator fileCreator = new FileCreator();
        //        fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");
        //    }

        //    if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
        //    {
        //        File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
        //    }

        //    //FolderCreator folderCreator = new FolderCreator();
        //    //folderCreator.CreateFolder(serializablesFolderPath);

        //    //FileCreator fileCreator = new FileCreator();
        //    //fileCreator.CreateFile(serializablesFolderPath, filename, "xml");                       

        //    currentReminders.Remove(reminderForRemoval);

        //    var serializer = new XmlSerializer(currentReminders.GetType(), new XmlRootAttribute("reminders"));

        //    using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
        //    {
        //        serializer.Serialize(writer, currentReminders);
        //    }
        //}

        private void InitializeVoteXML()
        {
            //string serializablesFolderPath = Directory.GetCurrentDirectory() + "/" + "Serializables";
            //string filename = "votes";

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                return;
            }
            Directory.CreateDirectory(serializablesFolderPath);

            FileCreator fileCreator = new FileCreator();
            fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");

            List<Vote> votes = new List<Vote>();
            var serializer = new XmlSerializer(votes.GetType(), new XmlRootAttribute("votes"));

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, votes);
            }
        }

        public void ClearAllVotes()
        {          
            Directory.CreateDirectory(serializablesFolderPath);

            FileCreator fileCreator = new FileCreator();
            fileCreator.CreateFileIfNotExist(serializablesFolderPath, filename, "xml");

            List<Vote> votes = new List<Vote>();
            var serializer = new XmlSerializer(votes.GetType(), new XmlRootAttribute("votes"));

            if (File.Exists(serializablesFolderPath + "/" + filename + ".xml"))
            {
                File.WriteAllText(serializablesFolderPath + "/" + filename + ".xml", string.Empty);
            }

            using (StreamWriter writer = File.AppendText(serializablesFolderPath + "/" + filename + ".xml"))
            {
                serializer.Serialize(writer, votes);
            }
        }
    }
}

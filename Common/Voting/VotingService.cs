using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Voting
{
    public class VotingService
    {
        private string defaultCategory;
        private VotingRepository votes;

        public VotingService()
        {
            defaultCategory = string.Empty;
            votes = new VotingRepository();
        }

        public void AddVote(Vote vote)
        {
            //votes.VotePool.Add(reminder);
            votes.AddVoteToXML(vote);
        }

        public List<Vote> GetAllVotes()
        {
            //return reminders.ReminderPool;
            return votes.GetVotesFromXML();
        }

        public void ClearVotes()
        {
            votes.ClearAllVotes();
        }

        public void SetDefaultVoteCategory(string category)
        {
            defaultCategory = category;
        }

        public string GetDefaultCategory()
        {
            return defaultCategory;
        }
    }
}

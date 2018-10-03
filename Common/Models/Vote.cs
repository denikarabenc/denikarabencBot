using System;

namespace Common.Models
{
    public class Vote
    {
        private string user;
        private string voteChoice;
        private string voteCategory;
        private DateTime timeVoted;

        public string VoteCategory { get => voteCategory; set => voteCategory = value; }
        public string VoteChoice { get => voteChoice; set => voteChoice = value; }
        public string User { get => user; set => user = value; }
        public DateTime TimeVoted { get => timeVoted; set => timeVoted = value; }

        public Vote(string user, string voteChoice, string voteCategory, DateTime timeVoted)
        {
            this.user = user;
            this.voteCategory = voteCategory;
            this.voteChoice = voteChoice;
            this.timeVoted = timeVoted;
        }
        public Vote()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denikarabencBot.Models.Voting
{
    public class VoteCount
    {
        private string category;
        private string earliestVoteUser;
        private string latestVoteUser;
        private DateTime earliestVoteTime;
        private DateTime latestVoteTime;
        private Dictionary<string, int> voteNumbers;

        public VoteCount()
        {
            voteNumbers = new Dictionary<string, int>();
        }

        public string Category { get => category; set => category = value; }
        public string LatestVoteUser { get => latestVoteUser; set => latestVoteUser = value; }
        public string EarliestVoteUser { get => earliestVoteUser; set => earliestVoteUser = value; }
        public DateTime EarliestVoteTime { get => earliestVoteTime; set => earliestVoteTime = value; }
        public DateTime LatestVoteTime { get => latestVoteTime; set => latestVoteTime = value; }
        public Dictionary<string, int> VoteNumbers { get => voteNumbers; set => voteNumbers = value; }      
        public string MostVotes
        {
            get
            {
                return GetVotedPick(0);
            }
        }
        public string SecondMostVotes
        {
            get
            {
                return GetVotedPick(1);
            }
        }
        public string ThirdMostVotes
        {
            get
            {
                return GetVotedPick(2);
            }
        }

        private List<KeyValuePair<string, int>> GetSortedVotes()
        {
            List<KeyValuePair<string, int>> list = voteNumbers.ToList();

            list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            list.Reverse();
            return list;
        }

        private string GetVotedPick(int index)
        {
            List<KeyValuePair<string, int>> list = GetSortedVotes();
            
            if (list.Count > index)
            {               
                return list[index].Key + " with " + list[index].Value + " votes";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

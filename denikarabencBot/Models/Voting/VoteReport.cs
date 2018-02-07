using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denikarabencBot.Models.Voting
{
    public class VoteReport
    {
        string category;
        string mostVotes;
        int numberOfVotes;

        public string Category { get => category; set => category = value; }
        public string MostVotes { get => mostVotes; set => mostVotes = value; }
        public int NumberOfVotes { get => numberOfVotes; set => numberOfVotes = value; }
    }
}

using Common.Models;
using denikarabencBot.Models.Voting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denikarabencBot.ViewModels
{
    public class VoteReportViewModel : INotifyPropertyChanged
    {
        private VoteCount voteDetails;
        private VoteReport selectedVoteReport;
        private List<Vote> voteList;
        private List<VoteCount> filteredVoteList;
        private List<VoteReport> voteReportList;

        public VoteReportViewModel(List<Vote> voteList)
        {
            this.voteList = voteList;
            filteredVoteList = GetFilteredVotes(voteList);
            voteReportList = GetVoteReport(filteredVoteList);
        }

        private List<VoteReport> GetVoteReport(List<VoteCount> filteredVotes)
        {
            List<VoteReport> voteReport = new List<VoteReport>(filteredVotes.Count);

            foreach (var vote in filteredVotes)
            {
                VoteReport vr = new VoteReport();
                vr.Category = vote.Category;
                //vr.TimePeriod = vote.EarliestVoteTime.ToLongDateString() + " - " + vote.LatestVoteTime.ToLongDateString();
                vr.MostVotes = string.Empty;
                vr.NumberOfVotes = 0;
                foreach (var numberOfVotes in vote.VoteNumbers)
                {
                    if (numberOfVotes.Value == vr.NumberOfVotes)
                    {
                        vr.MostVotes = vr.MostVotes + ", " + numberOfVotes.Key;
                    }
                    if (numberOfVotes.Value > vr.NumberOfVotes)
                    {
                        vr.NumberOfVotes = numberOfVotes.Value;
                        vr.MostVotes = numberOfVotes.Key;
                    }                    
                }

                voteReport.Add(vr);
            }
            return voteReport;
        }

        private List<VoteCount> GetFilteredVotes(List<Vote> votes)
        {
            List<VoteCount> categories = new List<VoteCount>();
            foreach (Vote vote in votes)
            {
                if (categories.Any(c => c.Category == vote.VoteCategory))
                {
                    int index = categories.FindIndex(v => v.Category == vote.VoteCategory);

                    if (categories[index].EarliestVoteTime > vote.TimeVoted)
                    {
                        categories[index].EarliestVoteTime = vote.TimeVoted;
                        categories[index].EarliestVoteUser = vote.User;
                    }
                    if (categories[index].LatestVoteTime < vote.TimeVoted)
                    {
                        categories[index].LatestVoteTime = vote.TimeVoted;
                        categories[index].LatestVoteUser = vote.User;
                    }
                    if (categories[index].VoteNumbers.Keys.Contains(vote.VoteChoice))
                    {
                        categories[index].VoteNumbers[vote.VoteChoice]++;
                    }
                    else
                    {
                        categories[index].VoteNumbers[vote.VoteChoice] = 1;
                    }

                }
                else
                {
                    //init vote category
                    VoteCount v = new VoteCount();
                    v.Category = vote.VoteCategory;
                    v.EarliestVoteTime = vote.TimeVoted;
                    v.LatestVoteTime = vote.TimeVoted;
                    v.EarliestVoteUser = vote.User;
                    v.LatestVoteUser = vote.User;
                    v.VoteNumbers[vote.VoteChoice] = 1;
                    categories.Add(v);
                }
            }

            return categories;
        }

        private VoteCount GetSelectedVoteDetails(VoteReport vr)
        {
            return filteredVoteList.Where(x=> x.Category == vr.Category).First();
        }

        public List<VoteReport> VoteReportList { get => voteReportList; set => voteReportList = value; }
        public VoteReport SelectedVoteReport
        {
            get => selectedVoteReport;
            set
            {
                selectedVoteReport = value;
                voteDetails = GetSelectedVoteDetails(selectedVoteReport);
                OnPropertyChanged(nameof(VoteDetails));
            }
        }

        public VoteCount VoteDetails => voteDetails;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

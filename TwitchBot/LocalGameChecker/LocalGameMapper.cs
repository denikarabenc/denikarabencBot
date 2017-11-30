using System.Collections.Generic;

namespace TwitchBot.LocalGameChecker
{
    public class LocalGameMapper
    {
        private Dictionary<string, string> localGameList; //key is process main window name, and value is twitch game name

        public LocalGameMapper()
        {
            localGameList = new Dictionary<string, string>();
            PopulateLocalGameList();
        }

        public Dictionary<string, string> LocalGameList { get => localGameList; }

        private void PopulateLocalGameList()
        {
            LocalGameList.Add("Heroes of the Storm", "Heroes of the Storm");
            LocalGameList.Add("StarCraft II", "StarCraft II");
            LocalGameList.Add("Brood War", "StarCraft");
            LocalGameList.Add("Overwatch", "Overwatch");
            LocalGameList.Add("Diablo III", "Diablo III: Reaper of Souls");
            LocalGameList.Add("League of Legends (TM) Client", "League of Legends");
            LocalGameList.Add("Hearthstone", "Hearthstone");
            LocalGameList.Add("Fortnite ", "Fortnite");
        }
    }
}

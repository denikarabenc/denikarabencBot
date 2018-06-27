using System.Collections.Generic;
using TwitchBot.TwitchStream;

namespace Common.Interfaces
{
    public interface IStreamInfoProvider
    {
        void AddPlayingGame(string game = null);
        string GetTitle();
        string GetCurrentStreamGame();
        IList<string> GetStreamGamesWhichWouldNotBeChanged();
        
        

        List<StreamGame> GamesPlayed { get;}
    }
}

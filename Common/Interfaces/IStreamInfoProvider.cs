using System.Collections.Generic;
using Common.Models;

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

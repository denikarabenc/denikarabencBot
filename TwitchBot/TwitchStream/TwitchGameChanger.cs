using Common.Helpers;
using System.Timers;
using TwitchBot.Helpers;
using TwitchBot.Interfaces;
using TwitchBot.LocalGameChecker;
using TwitchBot.Steam;
using TwitchBot.TwitchStream;

namespace TwitchBot
{
    public class TwitchGameChanger
    {
        private readonly int waitInMinutesBeforeTwitchIsUpdated;
        private readonly IIrcClient irc;
        private readonly LocalGameMapper localGameMapper;
        private readonly LocalRunningGameChecker localRunningGameChecker;
        private readonly TwitchStreamInfoProvider twitchStreamInfoProvider;
        private readonly TwitchStreamUpdater twitchStreamUpdater;
        private readonly SteamInfoProvider steamInfoProvider;
        private Timer gameTimer;
        private Timer gameChangePossibleTimer;
        private bool gameChangePossible;

        public TwitchGameChanger(IIrcClient irc, TwitchStreamInfoProvider twitchStreamInfoProvider, SteamInfoProvider steamInfoProvider, string channelName)
        {
            irc.ThrowIfNull(nameof(irc));
            twitchStreamInfoProvider.ThrowIfNull(nameof(twitchStreamInfoProvider));
            steamInfoProvider.ThrowIfNull(nameof(steamInfoProvider));
            this.irc = irc;
            this.twitchStreamInfoProvider = twitchStreamInfoProvider;
            this.steamInfoProvider = steamInfoProvider;

            localGameMapper = new LocalGameMapper();
            localRunningGameChecker = new LocalRunningGameChecker(localGameMapper);
            twitchStreamUpdater = new TwitchStreamUpdater(channelName);
            gameChangePossible = true;
            waitInMinutesBeforeTwitchIsUpdated = 3;

            twitchStreamInfoProvider.AddPlayingGame();

            gameTimer = new Timer(1000 * 10);
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;
            gameTimer.Elapsed += CheckIfGameIsCorrect;

            gameChangePossibleTimer = new Timer(1000 * 60 * waitInMinutesBeforeTwitchIsUpdated); //This is necessary so twitch web API is updated. Also, it is not realistic to have someone updates the game every 10 sec. 3 minutes is just enough.
            gameChangePossibleTimer.AutoReset = false;
            gameChangePossibleTimer.Enabled = true;
            gameChangePossibleTimer.Elapsed += GameChangePossibleTimer_Elapsed; ;
        }

        private void GameChangePossibleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            gameChangePossible = true;
        }

        private void CheckIfGameIsCorrect(object sender, ElapsedEventArgs e)
        {
            if (!gameChangePossible)
            {
                return;
            }

            string twitchCurrentGame = twitchStreamInfoProvider.GetCurrentTwitchGame();

            if (twitchCurrentGame == null)
            {
                return;
            }

            if (twitchStreamInfoProvider.GamesPlayed.Count == 0)
            {
                twitchStreamInfoProvider.AddPlayingGame(twitchCurrentGame);
            }

            if (twitchStreamInfoProvider.GetTwitchGamesWhichWouldNotBeChanged().Contains(twitchCurrentGame))
            {
                return;
            }

            string gameCurrentlyRunningOnMachine;

            gameCurrentlyRunningOnMachine = GetSteamInfoGame();

            if (gameCurrentlyRunningOnMachine != null)
            {
                gameCurrentlyRunningOnMachine = gameCurrentlyRunningOnMachine.Replace("â„¢", "");
            }

            if (gameCurrentlyRunningOnMachine != null && twitchCurrentGame != gameCurrentlyRunningOnMachine)
            {
                string reportMessage = twitchStreamUpdater.SetTwitchGameAndReturnWhichGameIsSet(gameCurrentlyRunningOnMachine);
                
                if (reportMessage != "Game change failed FeelsBadMan")
                {
                    twitchStreamInfoProvider.AddPlayingGame(gameCurrentlyRunningOnMachine);
                    gameChangePossible = false;
                    gameChangePossibleTimer.Start();
                }

                irc.SendChatMessage(reportMessage);

                return;
            }

            gameCurrentlyRunningOnMachine = localRunningGameChecker.GetWhichKnownGameIsRunning();

            if (gameCurrentlyRunningOnMachine != null && gameCurrentlyRunningOnMachine != twitchCurrentGame)
            {
                string reportMessage = twitchStreamUpdater.SetTwitchGameAndReturnWhichGameIsSet(gameCurrentlyRunningOnMachine);
                
                if (reportMessage != "Game change failed FeelsBadMan")
                {
                    twitchStreamInfoProvider.AddPlayingGame(gameCurrentlyRunningOnMachine);
                    gameChangePossible = false;
                    gameChangePossibleTimer.Start();
                }

                irc.SendChatMessage(reportMessage);
            }
        }

        private string GetSteamInfoGame()
        {
            //SteamInfoProvider sip = new SteamInfoProvider();
            steamInfoProvider.UpdateSteamInfo();
            if (steamInfoProvider.GameName != null)
            {
                return steamInfoProvider.GameName;
            }
            else
            {
                return null;
            }
        }
    }
}

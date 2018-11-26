using BotCore.LocalGameChecker;
using BotCore.Steam;
using Common.Helpers;
using Common.Interfaces;
using System;
using System.Timers;

namespace BotCore
{
    public class AutoStreamGameChanger : IDisposable
    {
        private readonly int waitInMinutesBeforeTwitchIsUpdated;
        private readonly IIrcClient irc;
        private readonly LocalGameMapper localGameMapper;
        private readonly LocalRunningGameChecker localRunningGameChecker;
        private readonly IStreamInfoProvider streamInfoProvider;
        private readonly IStreamUpdater streamUpdater;
        private readonly SteamInfoProvider steamInfoProvider;
        private string streamCurrentGame;
        private Timer gameTimer;
        private Timer gameChangePossibleTimer;
        private bool gameChangePossible;

        public AutoStreamGameChanger(IIrcClient irc, IStreamInfoProvider streamInfoProvider, IStreamUpdater streamUpdater, SteamInfoProvider steamInfoProvider)
        {
            irc.ThrowIfNull(nameof(irc));
            streamInfoProvider.ThrowIfNull(nameof(streamInfoProvider));
            steamInfoProvider.ThrowIfNull(nameof(steamInfoProvider));
            streamUpdater.ThrowIfNull(nameof(streamUpdater));
            this.irc = irc;
            this.streamInfoProvider = streamInfoProvider;
            this.steamInfoProvider = steamInfoProvider;
            this.streamUpdater = streamUpdater;

            localGameMapper = new LocalGameMapper();
            localRunningGameChecker = new LocalRunningGameChecker(localGameMapper);
            gameChangePossible = true;
            waitInMinutesBeforeTwitchIsUpdated = 3;

            streamInfoProvider.AddPlayingGame();

            streamCurrentGame = streamInfoProvider.GetCurrentStreamGame();

            //gameTimer = new Timer(1000 * 10);
            //gameTimer.AutoReset = true;
            //gameTimer.Enabled = true;
            //gameTimer.Elapsed += CheckIfGameIsCorrect;

            //gameChangePossibleTimer = new Timer(1000 * 60 * waitInMinutesBeforeTwitchIsUpdated); //This is necessary so twitch web API is updated. Also, it is not realistic to have someone updates the game every 10 sec. 3 minutes is just enough.
            //gameChangePossibleTimer.AutoReset = false;
            //gameChangePossibleTimer.Enabled = true;
            //gameChangePossibleTimer.Elapsed += GameChangePossibleTimer_Elapsed;
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

            //twitchCurrentGame = twitchStreamInfoProvider.GetCurrentTwitchGame();

            if (streamCurrentGame == null)
            {
                return;
            }

            if (streamInfoProvider.GamesPlayed.Count == 0)
            {
                streamInfoProvider.AddPlayingGame(streamCurrentGame);
            }

            if (streamInfoProvider.GetStreamGamesWhichWouldNotBeChanged().Contains(streamCurrentGame))
            {
                return;
            }

            string gameCurrentlyRunningOnMachine;

            gameCurrentlyRunningOnMachine = GetSteamInfoGame();
            
            if (steamInfoProvider.GetSteamGamesWhichWouldNotBeChangedTo().Contains(gameCurrentlyRunningOnMachine))
            {
                gameCurrentlyRunningOnMachine = null;
            }

            if (gameCurrentlyRunningOnMachine != null)
            {
                gameCurrentlyRunningOnMachine = ParseGameName(gameCurrentlyRunningOnMachine);                
            }

            string gameForStream = string.Empty;
            if (gameCurrentlyRunningOnMachine != null)
            {
               // gameForTwitch = GetGameCurrentlyRunningInFormatForTwitch(gameCurrentlyRunningOnMachine); //TODO
                if (string.IsNullOrEmpty(gameForStream))
                {
                    gameForStream = gameCurrentlyRunningOnMachine;
                }
            }

            if (!string.IsNullOrEmpty(gameForStream) && streamCurrentGame != gameForStream)
            {
                //Ovde nadji koju igri igras sa giant bomb-a, i ne treba da se setuje koja se igra igra na twitch-u
                //string gameForTwitch = GetGameCurrentlyRunningInFormatForTwitch(gameCurrentlyRunningOnMachine);
                //if (string.IsNullOrEmpty(gameForTwitch))
                //{
                //    gameForTwitch = gameCurrentlyRunningOnMachine;
                //}
                BotLogger.Logger.Log(Common.Models.LoggingType.Info, $"[AutoStreamGameChanger] -> Game name for twitch is: {gameForStream}");
                string reportMessage = streamUpdater.SetStreamGameAndReturnWhichGameIsSet(gameForStream);
                
               // if (reportMessage != "Game change failed FeelsBadMan")
                {   
                    streamCurrentGame = gameForStream;
                    streamInfoProvider.AddPlayingGame(gameCurrentlyRunningOnMachine);
                    gameChangePossible = false;
                    gameChangePossibleTimer.Start();
                }

                //This should be uncommented with the if clause. It is set like this only because of twitch internal error
                //gameChangePossible = false;
                //gameChangePossibleTimer.Start();
                irc.SendChatMessage(reportMessage);

                return;
            }

            gameCurrentlyRunningOnMachine = localRunningGameChecker.GetWhichKnownGameIsRunning();

            if (gameCurrentlyRunningOnMachine != null && gameCurrentlyRunningOnMachine != streamCurrentGame)
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Info, $"[AutoStreamGameChanger] -> Game name from PC is: {gameCurrentlyRunningOnMachine}");
                string reportMessage = streamUpdater.SetStreamGameAndReturnWhichGameIsSet(gameCurrentlyRunningOnMachine);
                
                if (reportMessage != "Game change failed FeelsBadMan")
                {
                    streamCurrentGame = streamInfoProvider.GetCurrentStreamGame();
                    streamInfoProvider.AddPlayingGame(gameCurrentlyRunningOnMachine);
                    gameChangePossible = false;
                    gameChangePossibleTimer.Start();
                }

                gameChangePossible = false;
                gameChangePossibleTimer.Start();
                irc.SendChatMessage(reportMessage);
            }
        }

        private string GetSteamInfoGame()
        {
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

        private string ParseGameName(string gameCurrentlyRunningOnMachine)
        {
            gameCurrentlyRunningOnMachine = gameCurrentlyRunningOnMachine.Replace("â„¢", "");
            gameCurrentlyRunningOnMachine = gameCurrentlyRunningOnMachine.Replace("\u00ae", "");
            gameCurrentlyRunningOnMachine = gameCurrentlyRunningOnMachine.Replace("\u2122", "");

            return gameCurrentlyRunningOnMachine;
        }

        public bool IsActive()
        {
            if (gameTimer?.Enabled == true && gameChangePossibleTimer.Enabled == true)
            {
                return true;
            }
            return false;
        }

        public void Start()
        {
            BotLogger.Logger.Log(Common.Models.LoggingType.Info, "AutoStreamGameChanger started");
            gameTimer = new Timer(1000 * 10);
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;
            gameTimer.Elapsed += CheckIfGameIsCorrect;

            gameChangePossibleTimer = new Timer(1000 * 60 * waitInMinutesBeforeTwitchIsUpdated); //This is necessary so twitch web API is updated. Also, it is not realistic to have someone updates the game every 10 sec. 3 minutes is just enough.
            gameChangePossibleTimer.AutoReset = false;
            gameChangePossibleTimer.Enabled = true;
            gameChangePossibleTimer.Elapsed += GameChangePossibleTimer_Elapsed;
        }

        public void Stop()
        {
            gameTimer.ThrowIfNull("AutoStreamGameChanger already stopped");
            gameChangePossibleTimer.ThrowIfNull("AutoStreamGameChanger already stopped");
            gameTimer.Stop();
            gameChangePossibleTimer.Stop();
            BotLogger.Logger.Log(Common.Models.LoggingType.Info, "AutoStreamGameChanger stopped");
        }

        //private List<string> GetResultsFromGiantBomb(string game)
        //{
        //    List<string> results = new List<string>();
        //    string json;

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.giantbomb.com/api/search/?api_key=4d2b9d82b5a7c34f441f027482d9c3f8525dffae&format=json&query=%27" + game + "%27&resources=game&field_list=name&limit=20");
        //    request.Method = "GET";
        //    request.UserAgent = "TwitchDenikarabencBot"; //vv0yeswj1kpcmyvi381006bl7rxaj4

        //    try
        //    {
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //        using (var reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            string jsonString = reader.ReadToEnd();
        //            GameBombInfoJsonRootObject jsonGamesResult = JsonConvert.DeserializeObject<GameBombInfoJsonRootObject>(jsonString);
        //            foreach (GameBombInfo gbi in jsonGamesResult.Results)
        //            {
        //                results.Add(gbi.Name);
        //            }
        //        }     
        //    }
        //    catch (WebException e)
        //    {
        //        BotLogger.Logger.Log(Common.Models.LoggingType.Warning, e);
        //        json = string.Empty;
        //    }
         
        //    return results;
        //}

        //private string GetGameCurrentlyRunningInFormatForTwitch(string gameCurrentlyRunningOnMachine)
        //{
        //    if (string.IsNullOrEmpty(gameCurrentlyRunningOnMachine))
        //    {
        //        return string.Empty;
        //    }

        //    string game = string.Empty;

        //    List<string> giantBombResults = GetResultsFromGiantBomb(gameCurrentlyRunningOnMachine);

        //    for (int i = 0; i < giantBombResults.Count; i++)
        //    {
        //        if (giantBombResults[i].ToLower() == gameCurrentlyRunningOnMachine.ToLower())
        //        {
        //            game = giantBombResults[i];
        //        }
        //    }

        //    return game;
        //}

        public void Dispose()
        {
            if (IsActive())
            {
                Stop();
            }
            gameTimer?.Dispose();
            gameChangePossibleTimer?.Dispose();
        }
    }
}

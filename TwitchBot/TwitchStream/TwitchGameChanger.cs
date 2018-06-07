using Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Timers;
using TwitchBot.Helpers;
using TwitchBot.Interfaces;
using TwitchBot.LocalGameChecker;
using TwitchBot.Steam;
using TwitchBot.TwitchStream;
using TwitchBot.TwitchStream.Json;

namespace TwitchBot
{
    public class TwitchGameChanger : IDisposable
    {
        private readonly int waitInMinutesBeforeTwitchIsUpdated;
        private readonly IIrcClient irc;
        private readonly LocalGameMapper localGameMapper;
        private readonly LocalRunningGameChecker localRunningGameChecker;
        private readonly TwitchStreamInfoProvider twitchStreamInfoProvider;
        private readonly TwitchStreamUpdater twitchStreamUpdater;
        private readonly SteamInfoProvider steamInfoProvider;
        private string twitchCurrentGame;
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

            twitchCurrentGame = twitchStreamInfoProvider.GetCurrentTwitchGame();

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

            //twitchCurrentGame = twitchStreamInfoProvider.GetCurrentTwitchGame();

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
            
            if (steamInfoProvider.GetSteamGamesWhichWouldNotBeChangedTo().Contains(gameCurrentlyRunningOnMachine))
            {
                gameCurrentlyRunningOnMachine = null;
            }

            if (gameCurrentlyRunningOnMachine != null)
            {
                gameCurrentlyRunningOnMachine = ParseGameName(gameCurrentlyRunningOnMachine);                
            }

            string gameForTwitch = string.Empty;
            if (gameCurrentlyRunningOnMachine != null)
            {
               // gameForTwitch = GetGameCurrentlyRunningInFormatForTwitch(gameCurrentlyRunningOnMachine); //TODO
                if (string.IsNullOrEmpty(gameForTwitch))
                {
                    gameForTwitch = gameCurrentlyRunningOnMachine;
                }
            }

            if (gameForTwitch != null && gameForTwitch != string.Empty && twitchCurrentGame != gameForTwitch)
            {
                //Ovde nadji koju igri igras sa giant bomb-a, i ne treba da se setuje koja se igra igra na twitch-u
                //string gameForTwitch = GetGameCurrentlyRunningInFormatForTwitch(gameCurrentlyRunningOnMachine);
                //if (string.IsNullOrEmpty(gameForTwitch))
                //{
                //    gameForTwitch = gameCurrentlyRunningOnMachine;
                //}

                BotLogger.Logger.Log(Common.Models.LoggingType.Info, $"[TwitchGameChanger] -> Game name from steam is: {gameCurrentlyRunningOnMachine}");
                string reportMessage = twitchStreamUpdater.SetTwitchGameAndReturnWhichGameIsSet(gameForTwitch);
                
                if (reportMessage != "Game change failed FeelsBadMan")
                {
                    //var a = GetResultsFromGiantBomb(gameCurrentlyRunningOnMachine);
                    //twitchCurrentGame = twitchStreamInfoProvider.GetCurrentTwitchGame();                    
                    //twitchStreamInfoProvider.AddPlayingGame(gameCurrentlyRunningOnMachine);
                    //gameChangePossible = false;
                    //gameChangePossibleTimer.Start();
                    
                    twitchCurrentGame = gameForTwitch;
                    twitchStreamInfoProvider.AddPlayingGame(gameCurrentlyRunningOnMachine);
                    gameChangePossible = false;
                    gameChangePossibleTimer.Start();
                }

                gameChangePossible = false;
                gameChangePossibleTimer.Start();
                irc.SendChatMessage(reportMessage);

                return;
            }

            gameCurrentlyRunningOnMachine = localRunningGameChecker.GetWhichKnownGameIsRunning();

            if (gameCurrentlyRunningOnMachine != null && gameCurrentlyRunningOnMachine != twitchCurrentGame)
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Info, $"[TwitchGameChanger] -> Game name from PC is: {gameCurrentlyRunningOnMachine}");
                string reportMessage = twitchStreamUpdater.SetTwitchGameAndReturnWhichGameIsSet(gameCurrentlyRunningOnMachine);
                
                if (reportMessage != "Game change failed FeelsBadMan")
                {
                    twitchCurrentGame = twitchStreamInfoProvider.GetCurrentTwitchGame();
                    twitchStreamInfoProvider.AddPlayingGame(gameCurrentlyRunningOnMachine);
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

        private List<string> GetResultsFromGiantBomb(string game)
        {
            List<string> results = new List<string>();
            string json;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.giantbomb.com/api/search/?api_key=4d2b9d82b5a7c34f441f027482d9c3f8525dffae&format=json&query=%27" + game + "%27&resources=game&field_list=name&limit=20");
            request.Method = "GET";
            request.UserAgent = "TwitchDenikarabencBot"; //vv0yeswj1kpcmyvi381006bl7rxaj4

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    GameBombInfoJsonRootObject jsonGamesResult = JsonConvert.DeserializeObject<GameBombInfoJsonRootObject>(jsonString);
                    foreach (GameBombInfo gbi in jsonGamesResult.Results)
                    {
                        results.Add(gbi.Name);
                    }
                }     
            }
            catch (WebException e)
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Warning, e);
                json = string.Empty;
            }
         
            return results;
        }

        private string GetGameCurrentlyRunningInFormatForTwitch(string gameCurrentlyRunningOnMachine)
        {
            if (string.IsNullOrEmpty(gameCurrentlyRunningOnMachine))
            {
                return string.Empty;
            }

            string game = string.Empty;

            List<string> giantBombResults = GetResultsFromGiantBomb(gameCurrentlyRunningOnMachine);

            for (int i = 0; i < giantBombResults.Count; i++)
            {
                if (giantBombResults[i].ToLower() == gameCurrentlyRunningOnMachine.ToLower())
                {
                    game = giantBombResults[i];
                }
            }

            return game;
        }

        public void Dispose()
        {
            gameTimer.Stop();
            gameChangePossibleTimer.Stop();
            gameTimer.Dispose();
            gameChangePossibleTimer.Dispose();
        }
    }
}

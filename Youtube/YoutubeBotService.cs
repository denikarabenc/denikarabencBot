using Common.Youtube;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Youtube
{
    public class YoutubeBotService
    {
        public YoutubeBotService()
        {
            //UpdateSongRequestList().Wait();
        }

        public static Action SongRequestCallback;

        public async void AddSongToPlaylist(string songVideoId, string userRequested, Action<string> callback)
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream(@"C:\Users\denik\Documents\Visual Studio 2017\Projects\denikarabencBot\client_id.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube, YouTubeService.Scope.YoutubeForceSsl },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
                }

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "denikarabencBot"
                });

                Playlist playlist = new Playlist();
                var playlistListRequest = youtubeService.Playlists.List("snippet");
                playlistListRequest.Mine = true;
                var playlistListResponse = await playlistListRequest.ExecuteAsync();

                var songRequestPlaylist = playlistListResponse.Items.FirstOrDefault(x => x.Snippet.Title == "Song Requests");

                if (songRequestPlaylist == null)
                {
                    // Create a new, private playlist in the authorized user's channel.
                    songRequestPlaylist = new Playlist();
                    songRequestPlaylist.Snippet = new PlaylistSnippet();
                    songRequestPlaylist.Snippet.Title = "Song Requests";
                    songRequestPlaylist.Snippet.Description = "Song requests for denikarabencBot";
                    songRequestPlaylist.Status = new PlaylistStatus();
                    songRequestPlaylist.Status.PrivacyStatus = "public";
                    songRequestPlaylist = await youtubeService.Playlists.Insert(songRequestPlaylist, "snippet,status").ExecuteAsync();
                }

                // Add a video to the newly created playlist.
                var newPlaylistItem = new PlaylistItem();
                newPlaylistItem.Snippet = new PlaylistItemSnippet();
                newPlaylistItem.Snippet.PlaylistId = songRequestPlaylist.Id;
                newPlaylistItem.Snippet.ResourceId = new ResourceId();
                newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                newPlaylistItem.Snippet.ResourceId.VideoId = songVideoId;
                newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();

                BotLogger.Logger.Log(Common.Models.LoggingType.Info, String.Format("Playlist item id {0} was added to playlist id {1}.", newPlaylistItem.Id, songRequestPlaylist.Id));

                callback(String.Format("Song {0} was added to playlist.", newPlaylistItem.Snippet.Title));
                SongRequestCallback.Invoke();
            }

            catch
            {
                callback(String.Format("Error, check url and try again later"));
            }

            //return new SongItem(newPlaylistItem.Snippet.Title, userRequested, "www.youtube.com/watch?v=" + newPlaylistItem.Snippet.ResourceId.VideoId);
        }

        public async void RemoveSong(string songVideoId)
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream(@"C:\Users\denik\Documents\Visual Studio 2017\Projects\denikarabencBot\client_id.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube, YouTubeService.Scope.YoutubeForceSsl },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
                }

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "denikarabencBot"
                });

                Playlist playlist = new Playlist();
                var playlistListRequest = youtubeService.Playlists.List("snippet");
                playlistListRequest.Mine = true;
                var playlistListResponse = await playlistListRequest.ExecuteAsync();

                var songRequestPlaylist = playlistListResponse.Items.FirstOrDefault(x => x.Snippet.Title == "Song Requests");

                ObservableCollection<SongItem> songList = new ObservableCollection<SongItem>();
                if (songRequestPlaylist == null)
                {
                    return;
                }

                await youtubeService.PlaylistItems.Delete(songVideoId).ExecuteAsync();
            }

            catch(Exception e)
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Warning, e);
            }

            //return new SongItem(newPlaylistItem.Snippet.Title, userRequested, "www.youtube.com/watch?v=" + newPlaylistItem.Snippet.ResourceId.VideoId);
        }

        public async void RemoveFirstSongFromPlaylist()
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream(@"C:\Users\denik\Documents\Visual Studio 2017\Projects\denikarabencBot\client_secret_885348159968-suq96qe1o2qr89r7asrihff3chehje4b.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube, YouTubeService.Scope.YoutubeForceSsl },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
                }

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "denikarabencBot"
                });

                Playlist playlist = new Playlist();
                var playlistListRequest = youtubeService.Playlists.List("snippet, player");
                playlistListRequest.Mine = true;
                var playlistListResponse = await playlistListRequest.ExecuteAsync();

                var songRequestPlaylist = playlistListResponse.Items.FirstOrDefault(x => x.Snippet.Title == "Song Requests");

                ObservableCollection<SongItem> songList = new ObservableCollection<SongItem>();
                if (songRequestPlaylist == null)
                {
                    return;
                }
                string videoIdForRemove = String.Empty;
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                    playlistItemsListRequest.PlaylistId = songRequestPlaylist.Id;
                    playlistItemsListRequest.MaxResults = 50;
                    playlistItemsListRequest.PageToken = nextPageToken;

                    // Retrieve the list of videos uploaded to the authenticated user's channel playlist.
                    var playlistItemsListResponse = playlistItemsListRequest.Execute();

                    
                    foreach (var playlistItem in playlistItemsListResponse.Items)
                    {
                        videoIdForRemove = playlistItem.Snippet.ResourceId.VideoId;
                        nextPageToken = null; //TODO sve ovo, ovo samo test da li ce da brise ili nece
                        break;
                        
                    }
                    if (nextPageToken != null)
                    {
                        nextPageToken = playlistItemsListResponse.NextPageToken;
                    }
                }

                await youtubeService.PlaylistItems.Delete(videoIdForRemove).ExecuteAsync();
                
            }

            catch(Exception e)
            {
                object o = e;
                return;
            }

            //return new SongItem(newPlaylistItem.Snippet.Title, userRequested, "www.youtube.com/watch?v=" + newPlaylistItem.Snippet.ResourceId.VideoId);
        }

        public async Task<ObservableCollection<SongItem>> UpdateSongRequestList()
        {
            UserCredential credential;
            using (var stream = new FileStream(@"C:\Users\denik\Documents\Visual Studio 2017\Projects\denikarabencBot\client_id.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                // This OAuth 2.0 access scope allows for full read/write access to the
                // authenticated user's account.
                new[] { YouTubeService.Scope.Youtube},
                "user",
                CancellationToken.None,
                new FileDataStore(this.GetType().ToString())
            );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "denikarabencBot"
            });


            Playlist playlist = new Playlist();
            var playlistListRequest = youtubeService.Playlists.List("snippet, player");
            playlistListRequest.Mine = true;
            var playlistListResponse = await playlistListRequest.ExecuteAsync();

            var songRequestPlaylist = playlistListResponse.Items.FirstOrDefault(x => x.Snippet.Title == "Song Requests");

            ObservableCollection<SongItem> songList = new ObservableCollection<SongItem>();
            if (songRequestPlaylist == null)
            {
                return songList;
            }

            var nextPageToken = "";
            while (nextPageToken != null)
            {
                var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                playlistItemsListRequest.PlaylistId = songRequestPlaylist.Id;
                playlistItemsListRequest.MaxResults = 50;
                playlistItemsListRequest.PageToken = nextPageToken;

                // Retrieve the list of videos uploaded to the authenticated user's channel playlist.
                var playlistItemsListResponse = playlistItemsListRequest.Execute();

                foreach (var playlistItem in playlistItemsListResponse.Items)
                {
                    // Print information about each video.
                    songList.Add(new SongItem(playlistItem.Snippet.Title, "www.youtube.com/watch?v=" + playlistItem.Snippet.ResourceId.VideoId, playlistItem.Id));                    
                }

                nextPageToken = playlistItemsListResponse.NextPageToken;
            }
            
            BotLogger.Logger.Log(Common.Models.LoggingType.Info, String.Format("Playlist {0} is read.", "Song Requests"));

            return songList;
        }
    }
}

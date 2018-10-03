using Common.WPFCommand;
using Common.Youtube;
using SimpleWebServer;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Youtube;

namespace denikarabencBot.ViewModels
{
    public class YoutubeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SongItem> youtubeSongs;
        private SongItem selectedSong;
        private ICommand startServer;
        private ICommand stopServer;
        private WebServer ws;
        YoutubeBotService youtubeBotService;

        public YoutubeViewModel()
        {
            youtubeSongs = new ObservableCollection<SongItem>();

            YoutubeBotService = new YoutubeBotService();
            YoutubeBotService.SongRequestCallback = RefreshSongList;
               
            youtubeSongs = GetSongListFromYoutube();

            ws = new WebServer(SendResponse, "http://localhost:8080/test/");
        }

        private ObservableCollection<SongItem> GetSongListFromYoutube()
        {
            ObservableCollection<SongItem> songList = new ObservableCollection<SongItem>();

            Task<ObservableCollection<SongItem>> task = YoutubeBotService.UpdateSongRequestList();

            task.Wait();

            songList = task.Result;

            return songList;
        }

        public ObservableCollection<SongItem> YoutubeSongs { get => youtubeSongs; set => youtubeSongs = value; }
        public SongItem SelectedSong { get => selectedSong; set => selectedSong = value; }
        public YoutubeBotService YoutubeBotService { get => youtubeBotService; set => youtubeBotService = value; }

        public ICommand StartServer
        {
            get => startServer ?? (startServer = new RelayCommand(param => StartServerCommandExecute()));
        }

        public ICommand StopServer
        {
            get => stopServer ?? (stopServer = new RelayCommand(param => StopServerCommandExecute()));
        }

        private void StartServerCommandExecute()
        {
            ws.Run();
            System.Diagnostics.Process.Start(@"http://localhost:8080/test/");
        }

        private void StopServerCommandExecute()
        {
            ws.Stop();
        }

        private string SendResponse(HttpListenerRequest request)
        {
            return System.IO.File.ReadAllText(@"C:\Users\denik\Documents\htmltest.html.txt");
        }

        private void RefreshSongList()
        {
            youtubeSongs = GetSongListFromYoutube();
            OnPropertyChanged(nameof(YoutubeSongs));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

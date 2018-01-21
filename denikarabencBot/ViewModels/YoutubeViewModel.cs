using Common.Youtube;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Youtube;

namespace denikarabencBot.ViewModels
{
    public class YoutubeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SongItem> youtubeSongs;
        private SongItem selectedSong;
        YoutubeBotService youtubeBotService;

        public YoutubeViewModel()
        {
            youtubeSongs = new ObservableCollection<SongItem>();

            YoutubeBotService = new YoutubeBotService();
            YoutubeBotService.SongRequestCallback = Testiranje;
               
           // youtubeSongs = GetSongListFromYoutube();
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

        private void Testiranje()
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

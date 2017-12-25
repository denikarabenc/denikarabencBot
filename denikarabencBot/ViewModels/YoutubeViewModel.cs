using Common.Youtube;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Youtube;

namespace denikarabencBot.ViewModels
{
    public class YoutubeViewModel : BaseViewModel
    {
        private ObservableCollection<SongItem> youtubeSongs;
        private SongItem selectedSong;
        YoutubeBotService youtubeBotService;

        public YoutubeViewModel()
        {
            youtubeSongs = new ObservableCollection<SongItem>();

            youtubeBotService = new YoutubeBotService();

            youtubeSongs = GetSongListFromYoutube();
        }

        private ObservableCollection<SongItem> GetSongListFromYoutube()
        {
            ObservableCollection<SongItem> songList = new ObservableCollection<SongItem>();

            Task<ObservableCollection<SongItem>> task = youtubeBotService.UpdateSongRequestList();

            task.Wait();

            songList = task.Result;

            return songList;
        }

        public ObservableCollection<SongItem> YoutubeSongs { get => youtubeSongs; set => youtubeSongs = value; }
        public SongItem SelectedSong { get => selectedSong; set => selectedSong = value; }
    }
}

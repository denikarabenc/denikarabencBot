using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Youtube
{
    public class SongItem
    {
        private string songName;
        private string userRequested;
        private string link;

        public SongItem(string songName, string userRequested, string link)
        {
            this.songName = songName;
            this.userRequested = userRequested;
            this.link = link;
        }

        public SongItem(string songName, string userRequested, string link, Action<string> callback)
        {
            this.songName = songName;
            this.userRequested = userRequested;
            this.link = link;
        }

        public SongItem(string songName, string link)
        {
            this.songName = songName;           
            this.link = link;
        }

        public string SongName { get => songName; set => songName = value; }
        public string UserRequested { get => userRequested; set => userRequested = value; }
        public string Link { get => link; set => link = value; }

    }
}

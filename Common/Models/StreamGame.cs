using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.TwitchStream
{
    public class StreamGame
    {
        private string gameName;
        private Stopwatch timePlayed;

        public StreamGame()
        {
            timePlayed = new Stopwatch();
        }

        public string GameName { get => gameName; set => gameName = value; }
        public Stopwatch TimePlayed { get => timePlayed; set => timePlayed = value; }
    }
}

using System.Diagnostics;

namespace Common.Models
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

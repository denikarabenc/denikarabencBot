using BotLogger;
using System.Windows;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {            
            InitializeComponent();
            mediaPlayer.BufferingStarted += MediaPlayer_BufferingStarted;
            mediaPlayer.BufferingEnded += MediaPlayer_BufferingEnded;            
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        private void MediaPlayer_BufferingEnded(object sender, RoutedEventArgs e)
        {
            Logger.Log("[VideoWindow] -> Buffering ended");
        }

        private void MediaPlayer_BufferingStarted(object sender, RoutedEventArgs e)
        {
            Logger.Log("[VideoWindow] -> Buffering started");
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
            this.Close();
        }
    }
}

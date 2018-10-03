using Common.Models;
using System;
using System.Windows;
using System.Windows.Data;

namespace BotCore
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {            
            InitializeComponent();
            ShowActivated = false;
            Topmost = false;
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            mediaPlayer.SourceUpdated += MediaPlayer_SourceUpdated;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        private void MediaPlayer_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            BotLogger.Logger.Log(LoggingType.Info, String.Format("[VideoWindow] -> Media source updated to {0}!", mediaPlayer.Source));
        }

        private void MediaPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            BotLogger.Logger.Log(LoggingType.Error, String.Format("[VideoWindow] -> Media failed Source was {0}!", mediaPlayer.Source), e.ErrorException);
            Dispose();
            this.Close();
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            BotLogger.Logger.Log(LoggingType.Info,"[VideoWindow] -> Media opened!");
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            Dispose();
            this.Close();
        }

        private void Dispose()
        {
            mediaPlayer.MediaOpened -= MediaPlayer_MediaOpened;
            mediaPlayer.MediaFailed -= MediaPlayer_MediaFailed;
            mediaPlayer.SourceUpdated -= MediaPlayer_SourceUpdated;
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
        }
    }
}

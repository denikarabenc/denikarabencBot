using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            this.Close();
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            BotLogger.Logger.Log(LoggingType.Info,"[VideoWindow] -> Media opened!");
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
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

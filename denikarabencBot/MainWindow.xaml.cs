using BotLogger;
using Common.Models;
using denikarabencBot.ViewModels;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
//using System.Speech.Recognition;

namespace denikarabencBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeneralViewModel viewModel;
        //private SpeechRecognitionEngine speechRecognizer;

        public MainWindow(GeneralViewModel vm)
        {
            viewModel = vm;
            DataContext = viewModel;
            InitializeComponent();

           // InitializeSpeechRecognizer();

            Closed += MainWindow_Closed;            
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            viewModel.StopBot();
            Logger.Log(LoggingType.Info, "Exit application");
        }

        //private void InitializeSpeechRecognizer()
        //{
        //    speechRecognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
        //    speechRecognizer.SetInputToDefaultAudioDevice();

        //    Choices colors = new Choices();
        //    colors.Add(new string[] { "hello"});

        //    GrammarBuilder gb = new GrammarBuilder();
        //    gb.Append(colors);

        //    // Create the Grammar instance.
        //    Grammar g = new Grammar(gb);

        //    speechRecognizer.LoadGrammar(g);

        //    speechRecognizer.SpeechDetected += SpeechRecognizer_SpeechDetected;

        //    speechRecognizer.SpeechRecognized += SpeechRecognizer_SpeechRecognized;
        //    speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);

        //}

        //private void SpeechRecognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        //{
        //    MessageBox.Show(e.Result.Text);
        //}       

        private void ProcessLog_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogProcess();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {           
            Properties.Settings.Default.Save();
            this.Close();            
        }    

        private void YoutubeButtonRemove(object sender, RoutedEventArgs e)
        {
            //viewModel.YoutubeViewModel.YoutubeSongs.Remove(viewModel.YoutubeViewModel.SelectedSong);           
            // Test.NavigateToString(@"<html><body><iframe width=""854"" height=""480"" src=""https://www.youtube.com/embed/BfUQWIEHTG4?list=PL91KghZsDTB7dd194d0ZFOwwTTw94JRuV"" frameborder=""0"" gesture=""media"" allow=""encrypted-media"" allowfullscreen></iframe></body></html>");
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Documents.Hyperlink link = (System.Windows.Documents.Hyperlink)e.OriginalSource;
            //  System.Diagnostics.Process.Start(link.NavigateUri.OriginalString););
            Test.Navigate("https://www.youtube.com/watch?v=BfUQWIEHTG4&list=PL91KghZsDTB7dd194d0ZFOwwTTw94JRuV");
            Test.Navigating += Test_Navigating;
            Test.Navigated += Test_Navigated;
        }

        private void Test_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            object o = (sender as WebBrowser).Source;
            
        }

        private void Test_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            object o = (sender as WebBrowser).Source;
            viewModel.YoutubeViewModel.YoutubeBotService.RemoveFirstSongFromPlaylist();
        }
    }
}

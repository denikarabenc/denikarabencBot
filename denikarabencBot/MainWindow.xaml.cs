using BotLogger;
using Common.Models;
using denikarabencBot.ViewModels;
using System;
using System.Threading;
using System.Windows;
//using System.Speech.Recognition;

namespace denikarabencBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        private Thread BotThread;
        //private SpeechRecognitionEngine speechRecognizer;

        public MainWindow(MainWindowViewModel vm)
        {
            viewModel = vm;
            DataContext = viewModel;
            InitializeComponent();

            //ReadSettings(); //Uncomment this to read saved settings on startup


           // InitializeSpeechRecognizer();

            Closed += MainWindow_Closed;            
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (BotThread != null && BotThread.IsAlive)
            {
                (viewModel.Children[0] as GeneralViewModel).StopBot();
                BotThread.Abort();
            }
        }

        //private void ReadSettings()
        //{
        //    (viewModel.Children[0] as GeneralViewModel).SteamID = Properties.Settings.Default.SteamId;
        //    (viewModel.Children[0] as GeneralViewModel).TwitchChannelName = Properties.Settings.Default.TwitchUserName;
        //}

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

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {           
            Logger.Log(LoggingType.Info, "[MainWindow] -> Join clicked");
            (viewModel.Children[0] as GeneralViewModel).RemoveBotButtonVisibility = Visibility.Visible;            
            BotThread = new Thread(() =>
            {
                (viewModel.Children[0] as GeneralViewModel).StartBot();
            });            
            BotThread.Start();
            (viewModel.Children[0] as GeneralViewModel).JoinButtonEnabled = false;
        }

        private void RemoveBotButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log(LoggingType.Info, "[MainWindow] -> Remove clicked");
            if (BotThread.IsAlive)
            {
                (viewModel.Children[0] as GeneralViewModel).StopBot();
                BotThread.Abort();
            }

            (viewModel.Children[0] as GeneralViewModel).JoinButtonEnabled = true;
            (viewModel.Children[0] as GeneralViewModel).RemoveBotButtonVisibility = Visibility.Collapsed;
        }

        private void ProcessLog_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogProcess();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            //Logger.Log("Exit application");
            //Properties.Settings.Default.Save();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Logger.Log(LoggingType.Info, "Exit application");
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void AddCommand(object sender, RoutedEventArgs e)
        {
            Logger.Log(LoggingType.Info, $"Command {(viewModel.Children[1] as CommandsViewModel).Command} added");

            (viewModel.Children[1] as CommandsViewModel).SaveCommand();
        }

        private void YoutubeButtonRemove(object sender, RoutedEventArgs e)
        {
            //(viewModel.Children[2] as YoutubeViewModel).YoutubeSongs.Remove((viewModel.Children[2] as YoutubeViewModel).SelectedSong);           
           // Test.NavigateToString(@"<html><body><iframe width=""854"" height=""480"" src=""https://www.youtube.com/embed/BfUQWIEHTG4?list=PL91KghZsDTB7dd194d0ZFOwwTTw94JRuV"" frameborder=""0"" gesture=""media"" allow=""encrypted-media"" allowfullscreen></iframe></body></html>");
        }     

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Documents.Hyperlink link = (System.Windows.Documents.Hyperlink)e.OriginalSource;
            System.Diagnostics.Process.Start(link.NavigateUri.OriginalString);
        }
    }
}

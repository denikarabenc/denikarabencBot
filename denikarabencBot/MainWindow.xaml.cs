using BotLogger;
using Common.Models;
using denikarabencBot.ViewModels;
using System;
using System.Windows;
using System.Speech.Recognition;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace denikarabencBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        private SpeechRecognitionEngine speechRecognizer;

        public MainWindow(MainWindowViewModel vm)
        {
            viewModel = vm;
            DataContext = viewModel;
            InitializeComponent();

           // InitializeSpeechRecognizer();

            Closing += MainWindow_Closing;
            Closed += MainWindow_Closed;            
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewModel.HasNewReminder)
            {
                MessageBoxResult result = MessageBoxResult.None;
                result = MessageBox.Show("You have some unseen reminders, do you really want to exit?", "Warning", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    Logger.Log(LoggingType.Info, "[MainWindow] -> Closing application cancelled");
                }
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            viewModel.StopBot();
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            Logger.Log(LoggingType.Info, "Exit application");
        }

        private void InitializeSpeechRecognizer()
        {
            try
            {
                speechRecognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
                speechRecognizer.SetInputToDefaultAudioDevice();

                Choices commands = new Choices();
                commands.Add(new string[] { "bot, please be kind and play replay", "bot, please be kind and clip that" });

                GrammarBuilder gb = new GrammarBuilder();
                gb.Append(commands);

                // Create the Grammar instance.
                Grammar g = new Grammar(gb);

                speechRecognizer.LoadGrammar(g);

                //speechRecognizer.SpeechDetected += SpeechRecognizer_SpeechDetected;

                speechRecognizer.SpeechRecognized += SpeechRecognizer_SpeechRecognized;
                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch(Exception ex)
            {
                Logger.Log(LoggingType.Warning, "Error initializing Speech!", ex);
            }

        }

        //private void SpeechRecognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        //{
        //    object o = "";
        //    BotLogger.Logger.Log(LoggingType.Info, "Voice recognized!");
        //}

        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            BotLogger.Logger.Log(LoggingType.Info, "Voice command recognized!");
           // ExecuteVoiceCommand(e.Result.Text);           
        }

        private void ProcessLog_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogProcess();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {           
            Properties.Settings.Default.Save();
            this.Close();            
        }    

        private void YoutubeButtonRemove(object sender, RoutedEventArgs e)
        {
            viewModel.YoutubeViewModel.YoutubeBotService.RemoveSong(viewModel.YoutubeViewModel.SelectedSong.Id);
            viewModel.YoutubeViewModel.YoutubeSongs.Remove(viewModel.YoutubeViewModel.SelectedSong);


            // Test.NavigateToString(@"<html><body><iframe width=""854"" height=""480"" src=""https://www.youtube.com/embed/BfUQWIEHTG4?list=PL91KghZsDTB7dd194d0ZFOwwTTw94JRuV"" frameborder=""0"" gesture=""media"" allow=""encrypted-media"" allowfullscreen></iframe></body></html>");
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start(viewModel.YoutubeViewModel.SelectedSong.Link);

            //System.Windows.Documents.Hyperlink link = (System.Windows.Documents.Hyperlink)e.OriginalSource;
            //System.Diagnostics.Process.Start(link.NavigateUri.OriginalString);
            //Test.Navigate("https://www.twitch.tv/lirik");
            //Test.LoadHtml(@"<iframe src=""https://clips.twitch.tv/embed?clip=HyperPhilanthropicHamKappaWealth&autoplay=false"" width=""640"" height =""360"" frameborder =""0"" scrolling =""no"" allowfullscreen =""false"" ></iframe>");
            //Test.Address = "https://www.twitch.tv/lirik";            
            //Test.Navigate("https://www.youtube.com/watch?v=BfUQWIEHTG4&list=PL91KghZsDTB7dd194d0ZFOwwTTw94JRuV");
            //Test.Navigating += Test_Navigating;
            //Test.Navigated += Test_Navigated;           
        }

        //private void Test_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    object o = (sender as WebBrowser).Source;

        //}

        //private void Test_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        //{
        //    object o = (sender as WebBrowser).Source;
        //    viewModel.YoutubeViewModel.YoutubeBotService.RemoveFirstSongFromPlaylist();
        //}

        private void ExecuteVoiceCommand(string voiceCommand)
        {
            switch (voiceCommand)
            {
                case "bot, please be kind and play replay":
                    viewModel.Bot?.PlayReplay();
                    break;
                case "bot, please be kind and clip that":
                    viewModel.Bot?.CreateAndPlayClip();
                    break;
            }           
        }








        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
             [In] IntPtr hWnd,
             [In] int id,
             [In] uint fsModifiers,
             [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            const uint VK_F22 = 0x85;
            const uint VK_F21 = 0x84;
            const uint MOD_CTRL = 0x0002;
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_F22))
            {
                //TODO handle error
            }
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID + 1, MOD_CTRL, VK_F21))
            {
                //TODO handle error
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            PlayReplayFromHotkey();
                            handled = true;
                            break;
                        case (HOTKEY_ID + 1):
                            MakeClipFromHotkey();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void PlayReplayFromHotkey()
        {
            viewModel.Bot?.PlayReplay();
        }

        private void MakeClipFromHotkey()
        {
            viewModel.Bot?.CreateAndPlayClip();
        }
    }
}

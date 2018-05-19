using Common.Models;
using Common.WPFCommand;
using denikarabencBotOBSSetup.ConfigurationWritters.BasicConfigurationWritter;
using denikarabencBotOBSSetup.ConfigurationWritters.SceneConfigurationWritter;
using denikarabencBotOBSSetup.Models;
using denikarabencBotOBSSetup.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace denikarabencBotOBSSetup.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string obsProfileName;
        private string progressBarText;
        private string scene;

        private SourceEnum selectedSource;

        private ICommand executeConfiguration;
        private ICommand addSourceConfiguration;

        private Action<string> reportProgress;
        private BackgroundWorker configurationWorker;

        private BasicConfigurationWritter basicConfigurationWritter;
        private SceneConfigurationWritter sceneConfigurationWritter;

        private List<SourceEnum> availableSources;
        private ObservableCollection<SourceScenePair> configuredScenesList;

        ProgressBarWindow progressBarWindow;

        public MainWindowViewModel()
        {
            basicConfigurationWritter = new BasicConfigurationWritter();
            sceneConfigurationWritter = new SceneConfigurationWritter();
            progressBarText = "";
            reportProgress = ProgressReport;
            availableSources = GetAllAvailableSources();
            configurationWorker = new BackgroundWorker();
            configurationWorker.DoWork += ConfigurationWorker_DoWork;
            configurationWorker.RunWorkerCompleted += ConfigurationWorker_RunWorkerCompleted;
            configuredScenesList = new ObservableCollection<SourceScenePair>();
        }        

        private List<string> GetFiltredStringList(ObservableCollection<SourceScenePair> list, SourceEnum filter)
        {
            List<string> filtredList = new List<string>(list.Count);

            foreach (SourceScenePair ssp in list)
            {
                if (ssp.Source == filter)
                {
                    filtredList.Add(ssp.Scene);
                }
            }

            return filtredList;
        }

        public ICommand ExecuteConfiguration
        {
            get => executeConfiguration ?? (executeConfiguration = new RelayCommand(param => ExecuteConfigurationExecute(), param => CanExecuteConfigurationExecute()));
        }

        public ICommand AddSourceConfiguration
        {
            get => addSourceConfiguration ?? (addSourceConfiguration = new RelayCommand(param => AddSourceConfigurationExecute(), param => CanAddSourceConfigurationExecute()));
        }

        public string OBSProfileName { get => obsProfileName; set => obsProfileName = value; }
        public string ProgressBarText
        { get => progressBarText;
          set
            {
                progressBarText = value;
                OnPropertyChanged(nameof(ProgressBarText));
            }
        }

        public bool IsFormEnabled
        {
            get
            {
                if (progressBarWindow != null && progressBarWindow.IsVisible)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string Scene { get => scene; set => scene = value; }
        public SourceEnum SelectedSource { get => selectedSource; set => selectedSource = value; }

        public List<SourceEnum> AvailableSources => availableSources;

        public ObservableCollection<SourceScenePair> ConfiguredScenesList
        {
            get
            {
                return configuredScenesList;
            }
            set
            {
                configuredScenesList = value;
                OnPropertyChanged(nameof(ConfiguredScenesList));
            }
        }

        private void ProgressReport(string message)
        {
            ProgressBarText = message;
        }

        private bool CanAddSourceConfigurationExecute()
        {
            return !string.IsNullOrEmpty(Scene);
        }

        private void AddSourceConfigurationExecute()
        {
            ConfiguredScenesList.Add(new SourceScenePair(SelectedSource, Scene));  
        }

        private bool CanExecuteConfigurationExecute()
        {
            return !string.IsNullOrEmpty(OBSProfileName);
        }

        private List<SourceEnum> GetAllAvailableSources()
        {
            return new List<SourceEnum>() { SourceEnum.Clip, SourceEnum.Replay };
        }

        private void ConfigurationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarWindow.Close();
            object o = e.Result;
            if (ConfigurationErrorType.Error == (ConfigurationErrorType)e.Result)
            {
                System.Windows.MessageBox.Show("Error writing configuration", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else
            {
                System.Windows.MessageBox.Show("Configuration completed successful", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }

        private void ConfigurationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //basicConfigurationWritter.WriteConfiguration(OBSProfileName, reportProgress);
                sceneConfigurationWritter.WriteConfiguration(OBSProfileName, GetFiltredStringList(ConfiguredScenesList, SourceEnum.Replay), GetFiltredStringList(ConfiguredScenesList, SourceEnum.Clip), reportProgress);
                e.Result = ConfigurationErrorType.Succeed;
            }
            catch (InvalidOperationException exp)
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Error, exp);
                e.Result = ConfigurationErrorType.Error;
            }
        }

        private void ExecuteConfigurationExecute()
        {
            InitializeProgressBarWindow();
            progressBarWindow.Show();
            OnPropertyChanged(nameof(IsFormEnabled));
            progressBarWindow.Owner = Application.Current.MainWindow;
            configurationWorker.RunWorkerAsync();
        }

        private void InitializeProgressBarWindow()
        {
            progressBarWindow = new ProgressBarWindow();
            progressBarWindow.DataContext = this;
            progressBarWindow.Closed += ProgressBarWindow_Closed;
            ProgressBarText = Properties.Resources.OBSConfiguration_STARTING_CONFIGURATION;
        }

        private void ProgressBarWindow_Closed(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsFormEnabled));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

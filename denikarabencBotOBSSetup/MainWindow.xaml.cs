using denikarabencBotOBSSetup.ViewModels;
using System.Windows;

namespace denikarabencBotOBSSetup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow(MainWindowViewModel vm)
        {
            viewModel = vm;
            DataContext = viewModel;
            InitializeComponent();

            //Closing += MainWindow_Closing;
            //Closed += MainWindow_Closed;
        }
    }
}

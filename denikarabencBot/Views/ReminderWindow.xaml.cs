using denikarabencBot.ViewModels;
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

namespace denikarabencBot.Views
{
    /// <summary>
    /// Interaction logic for ReminderWindow.xaml
    /// </summary>
    public partial class ReminderWindow : Window
    {
        ReminderWindowViewModel viewModel;
        public ReminderWindow(ReminderWindowViewModel vm)
        {
            viewModel = vm;
            DataContext = vm;
            InitializeComponent();
        }
    }
}

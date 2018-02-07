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
    /// Interaction logic for VoteReportWindow.xaml
    /// </summary>
    public partial class VoteReportWindow : Window
    {
        VoteReportViewModel viewModel;
        public VoteReportWindow(VoteReportViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            InitializeComponent();
        }
    }
}

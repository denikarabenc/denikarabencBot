using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace denikarabencBot.ViewModels
{
    public class MainWindowViewModel
    {
        ObservableCollection<object> children;

        public MainWindowViewModel()
        {
            children = new ObservableCollection<object>();
            children.Add(new GeneralViewModel());
            children.Add(new CommandsViewModel());
        }

        public ObservableCollection<object> Children => children;
    }
}

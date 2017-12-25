using System.Collections.ObjectModel;

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
            //children.Add(new YoutubeViewModel());
        }

        public ObservableCollection<object> Children => children;
    }
}

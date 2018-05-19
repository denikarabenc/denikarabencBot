using System.Windows;
using System.Windows.Controls;

namespace denikarabencBotOBSSetup.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class CustomProgressBar : UserControl
    {
        public CustomProgressBar()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ProgressBarTextProperty =
         DependencyProperty.Register("ProgressBarText", typeof(string), typeof(CustomProgressBar), new
            PropertyMetadata("", new PropertyChangedCallback(OnProgressBarTextChanged)));

        public string ProgressBarText
        {
            get { return (string)GetValue(ProgressBarTextProperty); }
            set { SetValue(ProgressBarTextProperty, value); }
        }

        private static void OnProgressBarTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomProgressBar customProgressBar = d as CustomProgressBar;
            customProgressBar.OnProgressBarTextChanged(e);
        }

        private void OnProgressBarTextChanged(DependencyPropertyChangedEventArgs e)
        {
            TextBlock.Text = e.NewValue.ToString();
        }
    }
}

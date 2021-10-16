namespace Lette.Wpf.AppCommands.Example
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindowViewModel ViewModel
        {
            get => (MainWindowViewModel)DataContext;
            set => DataContext = value;
        }
    }
}

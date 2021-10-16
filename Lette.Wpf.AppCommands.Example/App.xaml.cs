using System.Windows;

namespace Lette.Wpf.AppCommands.Example
{
    public partial class App
    {
        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            var dataContext = new MainWindowViewModel();
            mainWindow.ViewModel = dataContext;
            mainWindow.Show();
        }
    }
}

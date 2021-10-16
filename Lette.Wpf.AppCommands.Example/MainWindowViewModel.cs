using System.ComponentModel;
using System.Windows.Input;

namespace Lette.Wpf.AppCommands.Example
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            SetInformationCommand = new DelegateCommand<string>(SetInformation);
        }

        public ICommand SetInformationCommand { get; }

        private string _information = "";

        public string Information
        {
            get => _information;
            set
            {
                _information = value;
                RaisePropertyChanged(nameof(Information));
            }
        }

        private void SetInformation(string data)
        {
            Information = data;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var e = PropertyChanged;
            e?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Windows.Input;

namespace Lette.Wpf.AppCommands.Example
{
    public class DelegateCommand<T> : ICommand
    {
        public DelegateCommand(Action<T> executeMethod)
        {
            _executeMethod = executeMethod;
        }

        private readonly Action<T> _executeMethod;

        public void Execute(object parameter) => _executeMethod((T)parameter);
        public bool CanExecute(object _) => true;

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

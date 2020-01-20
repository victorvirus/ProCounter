using System;
using System.Windows.Input;

namespace ProCounter
{
    public class Command : ICommand
    {
        private readonly Action<object> _executeMethod;
        private readonly Func<bool> _canExecuteMethod;

        public Command(Action<object> executeMethod, Func<bool> canExecuteMethod)
        {
            this._executeMethod = executeMethod;
            this._canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}

using System;
using System.Windows.Input;

namespace kun.Infrastructure
{
    public class DelegatedCommand<T> : ICommand where T:class
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public DelegatedCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegatedCommand(Action<T> execute) : this(execute, null) { }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (_execute != null) _execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute((T)parameter);
            return true;
        }
    }
}

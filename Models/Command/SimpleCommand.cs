using System;
using System.Windows.Input;

namespace WPFApp.Models.Command
{
    public sealed class SimpleCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _func;

        public SimpleCommand(Action<object> action, Func<object, bool> func = null)
        {
            _action = action;
            _func = func;
        }

        public bool CanExecute(object parameter)
        {
            if (_func != default)
                return _func.Invoke(arg: parameter);

            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke(obj: parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
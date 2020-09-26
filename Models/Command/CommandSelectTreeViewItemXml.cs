using System;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.View;

namespace WPFApp.Models.Command
{
    public sealed class CommandSelectTreeViewItemXml : ICommand
    {
        private bool _flag;

        public bool CanExecute(object parameter)
        {
            return !_flag;
        }

        public void Execute(object parameter)
        {
            Frame frame = parameter as Frame;

            // ReSharper disable once PossibleNullReferenceException
            _flag = frame.NavigationService.Navigate(root: new SpecificationPage());
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
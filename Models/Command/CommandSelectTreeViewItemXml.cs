using System;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.View;

namespace WPFApp.Models.Command
{
    public sealed class CommandSelectTreeViewItemXml : ICommand
    {
        private bool flag;
        public bool CanExecute(object parameter)
        {
            return !flag;
        }

        public void Execute(object parameter)
        {
            Frame frame = parameter as Frame;
            flag = frame.NavigationService.Navigate(new SpecificationPage());
        }

        public event EventHandler CanExecuteChanged;
    }
}
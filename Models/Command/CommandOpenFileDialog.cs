using Microsoft.Win32;
using System;
using System.Windows.Input;
using WPFApp.ViewModels;

namespace WPFApp.Models.Command
{
    public sealed class CommandOpenFileDialog : ICommand
    {
        private OpenFileDialog _openFileDialog;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SpecificationPageViewModel specificationPageViewModel = parameter as SpecificationPageViewModel;
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.DefaultExt = ".xml";
            _openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            _openFileDialog.Filter = "XML File (*.xml)|*.xml";
            _openFileDialog.ShowDialog();
            specificationPageViewModel.Path = _openFileDialog.FileName;
        }

        public event EventHandler CanExecuteChanged;
    }
}
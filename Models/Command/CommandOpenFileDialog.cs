using System;
using System.Windows.Input;
using Microsoft.Win32;
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
            SpecificationPageViewModel specificationPageViewModel =
                parameter as SpecificationPageViewModel;
            _openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".xml",
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "XML File (*.xml)|*.xml"
            };
            _openFileDialog.ShowDialog();

            // ReSharper disable once PossibleNullReferenceException
            specificationPageViewModel.Path = _openFileDialog.FileName;
        }

        public event EventHandler CanExecuteChanged;
    }
}
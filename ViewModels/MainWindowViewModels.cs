using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPFApp.Annotations;
using WPFApp.Models.Command;
using WPFApp.Properties;

namespace WPFApp.ViewModels
{
    public class MainWindowViewModels : INotifyPropertyChanged
    {
        private bool _selectedXmlTreeView;
        private string _statusMessage;

        public MainWindowViewModels()
        {
            RelayCommand = new CommandSelectTreeViewItemXml();
            ClearLogCommand = new SimpleCommand(action: ob => Log.Log.ClearLog());
            StatusMessage = Resources.StatusBar_AppRunToWork;
        }

        public bool SelectedXmlTreeView
        {
            get { return _selectedXmlTreeView; }
            set
            {
                _selectedXmlTreeView = value;
                OnPropertyChanged(propertyName: nameof(SelectedXmlTreeView));
            }
        }

        public ICommand RelayCommand { get; }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(propertyName: nameof(StatusMessage));
            }
        }

        public ICommand ClearLogCommand { get; }

#region Интерфейс

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke
                (sender: this, e: new PropertyChangedEventArgs(propertyName: propertyName));
        }

#endregion
    }
}
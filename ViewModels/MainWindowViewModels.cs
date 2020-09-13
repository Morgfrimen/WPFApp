using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPFApp.Annotations;
using WPFApp.Models.Command;

namespace WPFApp.ViewModels
{
    public class MainWindowViewModels : INotifyPropertyChanged
    {
        private bool _selectedXmlTreeView;
        private readonly ICommand _relayCommand;
        private readonly ICommand _clearLogCommand;
        private string _statusMessage;

        public MainWindowViewModels()
        {
            _relayCommand = new CommandSelectTreeViewItemXml();
            _clearLogCommand = new SimpleCommand((ob) => Log.Log.ClearLog());
            StatusMessage = Properties.Resources.StatusBar_AppRunToWork;
        }

        public bool SelectedXmlTreeView
        {
            get { return _selectedXmlTreeView; }
            set
            {
                _selectedXmlTreeView = value;
                OnPropertyChanged(nameof(SelectedXmlTreeView));
            }
        }


        public ICommand RelayCommand
        {
            get { return _relayCommand; }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        public ICommand ClearLogCommand
        {
            get { return _clearLogCommand; }
        }

#region Интерфейс

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
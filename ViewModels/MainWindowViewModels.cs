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
        private ICommand _relayCommand;
        private string _statusMessage;
        private string _statusLoad;

        public MainWindowViewModels()
        {
            _relayCommand = new CommandSelectTreeViewItemXml();
            StatusMessage = WPFApp.Properties.Resources.StatusBar_AppRunToWork;
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
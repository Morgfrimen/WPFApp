using LogicalWork.ManagerSpecification;
using System.Collections.Generic;
using System.Windows.Input;
using WPFApp.Models.Command;
using WPFApp.Models.Command.SpecificationPageCommand;
using WPFApp.Properties;

namespace WPFApp.ViewModels
{
    public sealed class SpecificationPageViewModel : MainWindowViewModels
    {
        private readonly ICommand _commandOpenFileDialog;
        private readonly ICommand _downloadXmlCommand;
        private string _path;
        private IEnumerable<ResultSpecification> _resultSpecificationsItems;
        private string _buttonDownLoadContent;

        public SpecificationPageViewModel()
        {
            _commandOpenFileDialog = new CommandOpenFileDialog();
            _downloadXmlCommand = new CommandDownLoadXml();
            ButtonDownLoadContent = Resources.SpecificationPage_Button_Download;
        }

        public ICommand OpenFileDialog
        {
            get { return _commandOpenFileDialog; }
        }

        public string ButtonDownLoadContent
        {
            get { return _buttonDownLoadContent; }
            set { _buttonDownLoadContent = value; OnPropertyChanged(nameof(ButtonDownLoadContent)); }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; OnPropertyChanged(nameof(Path)); }
        }

        public ICommand DownloadXmlCommand
        {
            get { return _downloadXmlCommand; }
        }

        public IEnumerable<ResultSpecification> ResultSpecificationsItems
        {
            get { return _resultSpecificationsItems; }
            set { _resultSpecificationsItems = value; OnPropertyChanged(nameof(ResultSpecificationsItems)); }
        }
    }
}
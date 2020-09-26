using System.Collections.Generic;
using System.Windows.Input;
using LogicalWork.ManagerSpecification;
using WPFApp.Models.Command;
using WPFApp.Models.Command.SpecificationPageCommand;
using WPFApp.Properties;

namespace WPFApp.ViewModels
{
    public sealed class SpecificationPageViewModel : MainWindowViewModels
    {
        private string _buttonDownLoadContent;
        private string _path;
        private IEnumerable<ResultSpecification> _resultSpecificationsItems;
        private string _statusTime;

        public SpecificationPageViewModel()
        {
            OpenFileDialog = new CommandOpenFileDialog();
            DownloadXmlCommand = new CommandDownLoadXml();
            ButtonDownLoadContent = Resources.SpecificationPage_Button_Download;
        }

        public ICommand OpenFileDialog { get; }

        public string StatusTime
        {
            get { return _statusTime; }
            set
            {
                _statusTime = value;
                OnPropertyChanged(propertyName: nameof(StatusTime));
            }
        }

        public string ButtonDownLoadContent
        {
            get { return _buttonDownLoadContent; }
            set
            {
                _buttonDownLoadContent = value;
                OnPropertyChanged(propertyName: nameof(ButtonDownLoadContent));
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged(propertyName: nameof(Path));
            }
        }

        public ICommand DownloadXmlCommand { get; }

        public IEnumerable<ResultSpecification> ResultSpecificationsItems
        {
            get { return _resultSpecificationsItems; }
            set
            {
                _resultSpecificationsItems = value;
                OnPropertyChanged(propertyName: nameof(ResultSpecificationsItems));
            }
        }
    }
}
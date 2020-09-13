using LoadData;
using LogicalWork;
using LogicalWork.ManagerSpecification;
using ModelsData.XML;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WPFApp.ViewModels;

namespace WPFApp.Models.Command.SpecificationPageCommand
{
    public sealed class CommandDownLoadXml : ICommand
    {
        private const string Tag = nameof(CommandDownLoadXml);
        private bool _loadFile;
        private const int PauseAnimation = 200;
        private readonly Stopwatch _stopwatch;

        private ILoader _loaderXml;
        private IWork _specificationWork;

        public CommandDownLoadXml()
        {
            _stopwatch = new Stopwatch();
        }

        public bool CanExecute(object parameter)
        {
            return !_loadFile;
        }

        public void Execute(object parameter)
        {
            SpecificationPageViewModel specificationPageViewModel = parameter as SpecificationPageViewModel;
            // ReSharper disable once PossibleNullReferenceException
            if (string.IsNullOrEmpty(specificationPageViewModel.Path))
            {
                Log.Log.SetLog(Tag, "Параметр команды оказался пустым!");
                return;
            }

            _loaderXml = new LoaderXml(specificationPageViewModel.Path, new Specification());
            _loaderXml.LoadData();
            _specificationWork = new SpecificationXmlWork(_loaderXml.Model);
            ArrayList arrayListresult;
            Task task = new Task(() =>
            {
                //TODO: Если файл небольшой, то потребности в ногопотоке нет
                arrayListresult = _specificationWork.GetResultAsync().ArrayList;
                specificationPageViewModel.ResultSpecificationsItems = null;
                specificationPageViewModel.ResultSpecificationsItems = arrayListresult.Cast<ResultSpecification>();
                _loadFile = false;
                specificationPageViewModel.StatusTime = "Время операци: " + $"{_stopwatch.Elapsed:g}";
                _stopwatch.Stop();
                _stopwatch.Reset();
            });
            Task animationTask = new Task(() => Animation(specificationPageViewModel));
            _stopwatch.Start();
            _loadFile = true;
            task.Start();
            animationTask.Start();
        }

        private void Animation(SpecificationPageViewModel specificationPageViewModel)
        {
            string defaultStr = specificationPageViewModel.ButtonDownLoadContent;
            while (CheckLoadStatus())
            {
                string newStr = specificationPageViewModel.ButtonDownLoadContent;
                for (int index = 0; index < specificationPageViewModel.ButtonDownLoadContent.Length; index++)
                {
                    if (!CheckLoadStatus())
                        break;

                    var chars = newStr.ToCharArray();
                    chars[index] = '.';
                    newStr = new string(chars);
                    Thread.Sleep(PauseAnimation);
                    specificationPageViewModel.ButtonDownLoadContent = newStr;
                }

                if (!CheckLoadStatus())
                    break;
                var maxindex = specificationPageViewModel.ButtonDownLoadContent.Length - 1;
                for (int index = maxindex; index >= 0; index--)
                {
                    if (!CheckLoadStatus())
                        break;
                    var charsOldStr = newStr.ToCharArray();
                    var charsNewStr = defaultStr.ToCharArray();
                    charsOldStr[maxindex - index] = charsNewStr[maxindex - index];
                    newStr = new string(charsOldStr);
                    Thread.Sleep(PauseAnimation);
                    specificationPageViewModel.ButtonDownLoadContent = newStr;
                }
            }

            specificationPageViewModel.ButtonDownLoadContent = defaultStr;
        }

        private bool CheckLoadStatus()
        {
            return _loadFile;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
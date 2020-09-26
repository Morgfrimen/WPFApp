using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using LoadData;
using LogicalWork;
using LogicalWork.ManagerSpecification;
using ModelsData.XML;
using WPFApp.ViewModels;

namespace WPFApp.Models.Command.SpecificationPageCommand
{
    public sealed class CommandDownLoadXml : ICommand
    {
        private const string Tag = nameof(CommandDownLoadXml);
        private const int PauseAnimation = 200;
        private readonly Stopwatch _stopwatch;

        private ILoader _loaderXml;
        private bool _loadFile;
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
            SpecificationPageViewModel specificationPageViewModel =
                parameter as SpecificationPageViewModel;

            // ReSharper disable once PossibleNullReferenceException
            if (string.IsNullOrEmpty(value: specificationPageViewModel.Path))
            {
                Log.Log.SetLog(tag: Tag, message: "Параметр команды оказался пустым!");

                return;
            }

            _loaderXml = new LoaderXml
                (pathXml: specificationPageViewModel.Path, model: new Specification());
            _loaderXml.LoadData();
            _specificationWork = new SpecificationXmlWork(model: _loaderXml.Model);
            ArrayList listResult;
            Task task = new Task
            (
                action: () =>
                {
                    //TODO: Если файл небольшой, то потребности в ногопотоке нет
                    listResult = _specificationWork.GetResultAsync().ArrayList;
                    specificationPageViewModel.ResultSpecificationsItems = null;
                    specificationPageViewModel.ResultSpecificationsItems =
                        listResult.Cast<ResultSpecification>();
                    _loadFile = false;
                    specificationPageViewModel.StatusTime =
                        "Время операци: " + $"{_stopwatch.Elapsed:g}";
                    _stopwatch.Stop();
                    _stopwatch.Reset();
                }
            );
            Task animationTask = new Task
                (action: () => Animation(specificationPageViewModel: specificationPageViewModel));
            _stopwatch.Start();
            _loadFile = true;
            task.Start();
            animationTask.Start();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private void Animation(SpecificationPageViewModel specificationPageViewModel)
        {
            string defaultStr = specificationPageViewModel.ButtonDownLoadContent;

            while (CheckLoadStatus())
            {
                string newStr = specificationPageViewModel.ButtonDownLoadContent;

                for (int index = 0; index < specificationPageViewModel.ButtonDownLoadContent.Length;
                    index++)
                {
                    if (!CheckLoadStatus())
                        break;

                    char[] chars = newStr.ToCharArray();
                    chars[index] = '.';
                    newStr = new string(value: chars);
                    Thread.Sleep(millisecondsTimeout: PauseAnimation);
                    specificationPageViewModel.ButtonDownLoadContent = newStr;
                }

                if (!CheckLoadStatus())
                    break;
                int maxIndex = specificationPageViewModel.ButtonDownLoadContent.Length - 1;

                for (int index = maxIndex; index >= 0; index--)
                {
                    if (!CheckLoadStatus())
                        break;
                    char[] charsOldStr = newStr.ToCharArray();
                    char[] charsNewStr = defaultStr.ToCharArray();
                    charsOldStr[maxIndex - index] = charsNewStr[maxIndex - index];
                    newStr = new string(value: charsOldStr);
                    Thread.Sleep(millisecondsTimeout: PauseAnimation);
                    specificationPageViewModel.ButtonDownLoadContent = newStr;
                }
            }

            specificationPageViewModel.ButtonDownLoadContent = defaultStr;
        }

        private bool CheckLoadStatus()
        {
            return _loadFile;
        }
    }
}
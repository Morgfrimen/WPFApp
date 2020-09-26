using System;
using System.IO;
using System.Xml.Serialization;
using ModelsData;
using ModelsData.XML;

namespace LoadData
{
    /// <summary>
    ///     Класс, реализующий работу загрузки
    /// </summary>
    public sealed class LoaderXml : ILoader
    {
        private const string Tag = nameof(LoaderXml);

        private readonly XmlSerializer _xmlSerializer;

        /// <summary>
        ///     Путь к файлу XML
        /// </summary>
        private string _pathXml;

        /// <summary>
        ///     Модель спецификации, соответсвующая формату XML файлу
        /// </summary>
        private IModel _specification;

        public LoaderXml(string pathXml, IModel model)
        {
            SetPathXml(value: pathXml);
            _xmlSerializer = new XmlSerializer(type: model.GetType());
        }

        /// <summary>
        ///     Модель спецификации, соответсвующая формату XML файлу
        /// </summary>
        public IModel Model
        {
            get { return _specification; }
        }

        public void LoadData()
        {
            SetLog(messageLog: "Начало чтения файла файла");

            using (FileStream fileStream = new FileStream(path: GetPathXml(), mode: FileMode.Open))
            {
                try
                {
                    _specification = _xmlSerializer.Deserialize
                        (stream: fileStream) as Specification;

                    if (Model == null)
                        throw new ExceptionLoader
                            (tag: Tag, message: "Неудалось десерелизовать xml файл");
                }
                catch (Exception e)
                {
                    SetLog(messageLog: e.Message);

                    throw;
                }
            }
        }

        /// <summary>
        ///     Изменить путь к файлу XML
        /// </summary>
        public void SetPathXml(string value)
        {
            _pathXml = value;
        }

        /// <summary>
        ///     Получить путь к файлу XML
        /// </summary>
        public string GetPathXml()
        {
            return _pathXml;
        }

        private void SetLog(string messageLog)
        {
            Log.Log.SetLog(tag: Tag, message: messageLog);
        }
    }
}
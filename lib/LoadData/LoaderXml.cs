using ModelsData;
using ModelsData.XML;
using System.IO;
using System.Xml.Serialization;


namespace LoadData
{
    /// <summary>
    /// Класс, реализующий работу загрузки
    /// </summary>
    public sealed class LoaderXml : ILoader
    {
        private const string Tag = nameof(LoaderXml);

        /// <summary>
        /// Путь к файлу XML
        /// </summary>
        private string _pathXml;

        /// <summary>
        /// Модель спецификации, соответсвующая формату XML файлу
        /// </summary>
        private IModel _specification;

        private readonly XmlSerializer _xmlSerializer;

        public LoaderXml(string pathXml, IModel model)
        {
            SetPathXml(pathXml);
            _xmlSerializer = new XmlSerializer(model.GetType());
        }

        /// <summary>
        /// Модель спецификации, соответсвующая формату XML файлу
        /// </summary>
        public IModel Model
        {
            get { return _specification; }
        }

        /// <summary>
        /// Изменить путь к файлу XML
        /// </summary>
        public void SetPathXml(string value)
        {
            _pathXml = value;
        }

        /// <summary>
        /// Получить путь к файлу XML
        /// </summary>
        public string GetPathXml()
        {
            return _pathXml;
        }

        public void LoadData()
        {
            SetLog("Начало чтения файла файла");


            using (FileStream fileStream = new FileStream(GetPathXml(), FileMode.Open))
            {
                try
                {
                    _specification = _xmlSerializer.Deserialize(fileStream) as Specification;
                    if (this.Model == null)
                    {
                        throw new ExceptionLoader(Tag, "Неудалось десерелизовать xml файл");
                    }
                }
                catch (System.Exception e)
                {
                    SetLog(e.Message);
                    throw e;
                }
            }
        }


        private void SetLog(string messageLog)
        {
            Log.Log.SetLog(Tag, messageLog);
        }
    }
}
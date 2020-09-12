using ModelsData;

namespace LoadData
{
    /// <summary>
    /// Интерфейс для загрузчиков
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        /// Загрузка данных
        /// </summary>
        void LoadData();

        IModel Model { get; }
    }
}
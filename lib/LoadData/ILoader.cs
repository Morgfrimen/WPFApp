using ModelsData;

namespace LoadData
{
    /// <summary>
    ///     Интерфейс для загрузчиков
    /// </summary>
    public interface ILoader
    {
        IModel Model { get; }

        /// <summary>
        ///     Загрузка данных
        /// </summary>
        void LoadData();
    }
}
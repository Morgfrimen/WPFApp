namespace LogicalWork.ManagerSpecification
{
    /// <summary>
    ///     Контейнер с информацией об одном заказе для вывода результата в работе логики SpecificationXmlWork
    /// </summary>
    public sealed class ResultSpecification
    {
        /// <summary>
        ///     Номер заказа
        /// </summary>
        public string NumberOrder { get; set; }

        /// <summary>
        ///     Количество контейнеров
        /// </summary>
        public uint CountContainer { get; set; }

        /// <summary>
        ///     Количество негабаритных грузов
        /// </summary>
        public uint CountNotDimensional { get; set; }
    }
}
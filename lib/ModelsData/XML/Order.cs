using System;
using System.Xml.Serialization;

namespace ModelsData.XML
{
    /// <summary>
    ///     Модель данных Order (Представляют заказ)
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        ///     Индентификатор
        /// </summary>
        [XmlAttribute]
        public string Id { get; set; }

        /// <summary>
        ///     Имя элемента
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
    }
}
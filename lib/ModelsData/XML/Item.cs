using System;
using System.Xml.Serialization;

namespace ModelsData.XML
{
    /// <summary>
    /// Модель данных Item (Части заказа)
    /// </summary>
    [Serializable]
    public sealed class Item
    {
        /// <summary>
        /// Индентификатор
        /// </summary>
        [XmlAttribute]
        public string Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Вес
        /// </summary>
        [XmlAttribute]
        public uint Weight { get; set; }

        /// <summary>
        /// Индентикатор в модели Order
        /// </summary>
        [XmlAttribute]
        public string OrderId { get; set; }
    }
}
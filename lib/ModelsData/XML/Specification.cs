using System;
using System.Collections;
using System.Collections.Generic;

namespace ModelsData.XML
{
    [Serializable]
    public sealed class Specification : IModel
    {
        public List<Order> Orders { get; set; }

        public List<Item> Items { get; set; }

        public new Type GetType()
        {
            return typeof(Specification);
        }

        //public IEnumerator GetEnumerator()
        //{
        //    Dictionary<Order,IEnumerable<Item>> dictionary = new Dictionary<Order, IEnumerable<Item>>();
        //    foreach (Order order in Orders)
        //    {
        //        IEnumerable<Item> items = Items.Where(item => item.OrderId == order.Id);
        //        dictionary.
        //        yield return dictionary;
        //    }
        //}

        public ArrayList GetValueModels()
        {
            ArrayList arrayList = new ArrayList {Orders, Items};

            return arrayList;
        }
    }
}
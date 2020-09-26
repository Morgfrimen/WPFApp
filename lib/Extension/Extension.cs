using System;
using System.Collections.Generic;

namespace Extension
{
    public static class Extension
    {
        /// <summary>
        ///     Перебирает по элементном IEnumerable коллекцию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">Коллекция</param>
        /// <param name="action">Делегат Action, применяем для кажного элемента коллекции</param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T fo in enumerable)
                action.Invoke(obj: fo);

            return enumerable;
        }
    }
}
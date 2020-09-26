using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ModelsData;
using ModelsData.XML;

[assembly: InternalsVisibleTo(assemblyName: "UnitTestLib")]

namespace LogicalWork.ManagerSpecification
{
    /// <summary>
    ///     Логика работы для случая в тестовом задании
    /// </summary>
    public sealed class SpecificationXmlWork : IWork
    {
        private const string Tag = nameof(SpecificationXmlWork);

        private readonly IModel _model;
        private ViewTableData _viewTableDataResult;

        public SpecificationXmlWork(IModel model)
        {
            _model = model;
        }

        //TODO: Только для теста!
        // ReSharper disable once MemberCanBePrivate.Global
        internal uint MaxWeight { get; set; } = 5000;

        public ViewTableData GetResult()
        {
            Specification specification = _model as Specification;

            SetLog(message: "Начало работы по получению результата");

            foreach (Order order in specification.Orders)
            {
                IEnumerable<Item> collection = specification.Items.Where
                    (predicate: item => item.OrderId == order.Id);
                ResultOneOrder(order: order, items: collection);
            }

            SetLog(message: "Отработка алгоритма выдачи результата");

            return _viewTableDataResult;
        }

        public ViewTableData GetResultAsync()
        {
            Specification specification = _model as Specification;
            List<Task> tasks = new List<Task>();
            SetLog(message: "Начало работы по получению результата");

            foreach (Order order in specification.Orders)
            {
                Task task = new Task
                (
                    action: () =>
                    {
                        IEnumerable<Item> collection = specification.Items.Where
                            (predicate: item => item.OrderId == order.Id);
                        ResultOneOrder(order: order, items: collection);
                    }
                );
                tasks.Add(item: task);
                task.Start();
            }

            Task.WaitAll(tasks: tasks.ToArray());
            SetLog(message: "Отработка алгоритма выдачи результата");

            return _viewTableDataResult;
        }

        /// <summary>
        ///     Получаем информацию одного заказа
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <param name="items">Коллекция с частями указанного заказа</param>
        /// <returns></returns>
        private void ResultOneOrder(Order order, IEnumerable<Item> items)
        {
            ResultSpecification resultSpecification =
                new ResultSpecification {NumberOrder = order.Name};

            //Получили коллекцию негабаритных частей заказа
            IList<Item> notDimensionEnumerator = items.Where
                (predicate: item => item.Weight >= MaxWeight).ToList();

            //Удаляем негабаритные части заказа
            IList<Item> dimensionEnumerator = notDimensionEnumerator.ToList();
            items = items.Except(second: dimensionEnumerator).ToList();

            //Случай, когда товаров для габаритов нет -> items пустой
            if (items.Count() == default)
            {
                resultSpecification.CountNotDimensional = (uint) dimensionEnumerator.Count();
                SetViewTable(resultSpecification: resultSpecification);

                return;
            }

            uint minWeight = items.Min(selector: item => item.Weight); //минимальный вес
            IList<Item> bigContainer = items.Where
                (predicate: item => item.Weight > MaxWeight - minWeight).ToList();

            //Удаляем части с весьма большим весом, который входят только в один контейнер
            items = items.Except(second: bigContainer).ToList();

            //Случай, когда все габаритные товары занимаю по одному контейнеру
            if (items.Count() == default)
            {
                resultSpecification.CountNotDimensional = (uint) dimensionEnumerator.Count();
                resultSpecification.CountContainer = (uint) bigContainer.Count();
                SetViewTable(resultSpecification: resultSpecification);

                return;
            }

            //Записали в контейнер количество не габаритных товаров
            resultSpecification.CountNotDimensional = (uint) dimensionEnumerator.Count();

            //Добавили большие части заказа, но те, которые помещаются в один контейнер
            resultSpecification.CountContainer = (uint) bigContainer.Count();

            //Добавляем остальные контейнеры
            resultSpecification.CountContainer += AlgoritmContainer(items: items);

            SetViewTable(resultSpecification: resultSpecification);
        }

        /// <summary>
        ///     Алгоритм контейниризации
        ///     Сортируем по возрастанию веса,
        ///     потом пытаем поместить в контейнер
        /// </summary>
        /// <param name="items">Коллекция, которую нужно контейнизировать</param>
        private uint AlgoritmContainer(IEnumerable<Item> items)
        {
            SetLog(message: "Начало работа алгоритма по упаковки частей заказа");

            //Сортировка по возврастанию
            IOrderedEnumerable<Item> collection = items.OrderBy(keySelector: i => i.Weight);

            uint countConteiner = default;
            uint conteiterWeight = default;

            foreach (Item item in collection)
            {
                if (conteiterWeight < MaxWeight - item.Weight)
                {
                    conteiterWeight += item.Weight;
                }
                else
                {
                    conteiterWeight = default;
                    countConteiner++;
                }

                if (item == collection.Last() && conteiterWeight != default) countConteiner++;
            }

            SetLog(message: "Конец работы алгоритма упаковки частей заказа");

            return countConteiner;
        }

        private void SetViewTable(ResultSpecification resultSpecification)
        {
            if (_viewTableDataResult == default)
                _viewTableDataResult = new ViewTableData();

            lock (_viewTableDataResult)
            {
                SetLog(message: "Добавление строчки в результат");
                _viewTableDataResult.ArrayList.Add(value: resultSpecification);
            }
        }

        private void SetLog(string message)
        {
            Log.Log.SetLog(tag: Tag, message: message);
        }
    }
}
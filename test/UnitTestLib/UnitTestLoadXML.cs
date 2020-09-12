using LoadData;
using LogicalWork;
using LogicalWork.ManagerSpecification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelsData;
using ModelsData.XML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnitTestLib
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class UnitTestLoadXML
    {
        private ILoader _loaderXml;
        private IModel _specification;
        private IWork _specificationWork;

        [TestInitialize]
        public void Test_Initialize()
        {
            _specification = new Specification();
            _loaderXml = new LoaderXml("specification.xml", _specification);
            Assert.IsNotNull(_loaderXml);
            Assert.IsNotNull(_specification);
            try
            {
                _loaderXml.LoadData();
                _specification = ((LoaderXml)_loaderXml).Model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void BanchMark_TimeAlgoritm()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _specificationWork = new SpecificationXmlWork(_specification);
            _specificationWork.GetResult();
            stopwatch.Stop();
            Console.WriteLine($"Время обработки маленького файла в одном потоке: {stopwatch.Elapsed}");
            stopwatch.Reset();

            _specification = new Specification();
            _loaderXml = new LoaderXml("specification.xml", _specification);
            _loaderXml.LoadData();
            _specification = ((LoaderXml)_loaderXml).Model;
            stopwatch.Start();
            _specificationWork = new SpecificationXmlWork(_specification);
            _specificationWork.GetResultAsync();
            stopwatch.Stop();
            Console.WriteLine($"Время обработки в многопоточном режиме маленького файла: {stopwatch.Elapsed}");

            _specification = new Specification();
            _loaderXml = new LoaderXml("big_specification.xml", _specification);
            _loaderXml.LoadData();
            _specification = ((LoaderXml)_loaderXml).Model;
            stopwatch.Start();
            _specificationWork = new SpecificationXmlWork(_specification);
            _specificationWork.GetResult();
            stopwatch.Stop();
            Console.WriteLine($"Время обработки в одном потоке большого файла: {stopwatch.Elapsed}");
            stopwatch.Reset();

            _specification = new Specification();
            _loaderXml = new LoaderXml("big_specification.xml", _specification);
            _loaderXml.LoadData();
            _specification = ((LoaderXml)_loaderXml).Model;
            stopwatch.Start();
            _specificationWork = new SpecificationXmlWork(_specification);
            _specificationWork.GetResultAsync();
            stopwatch.Stop();
            Console.WriteLine($"Время обработки в многопоточном режиме большого файла: {stopwatch.Elapsed}");
        }

        [TestMethod]
        public void Test_SpecificationXmlWork_AlgoritmOneThread()
        {
            Specification specification = new Specification
            {
                Orders = new List<Order>()
                {
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 1"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 2"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 3"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 4"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 5"}
                }
            };
            //Определяем 5 заказов
            //Определяем части заказа
            specification.Items = new List<Item>()
            {
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 1",OrderId = specification.Orders[1-1].Id,Weight = 10},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 1",OrderId = specification.Orders[1-1].Id,Weight = 10},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 1",OrderId = specification.Orders[1-1].Id,Weight = 10},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 1",OrderId = specification.Orders[1-1].Id,Weight = 10},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 2",OrderId = specification.Orders[2-1].Id,Weight = 100},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 2",OrderId = specification.Orders[2-1].Id,Weight = 100},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 2",OrderId = specification.Orders[2-1].Id,Weight = 5},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 3",OrderId = specification.Orders[3-1].Id,Weight = 10},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 3",OrderId = specification.Orders[3-1].Id,Weight = 20},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 4",OrderId = specification.Orders[4-1].Id,Weight = 50},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 4",OrderId = specification.Orders[4-1].Id,Weight = 50},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 4",OrderId = specification.Orders[4-1].Id,Weight = 50},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 4",OrderId = specification.Orders[4-1].Id,Weight = 50},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 5",OrderId = specification.Orders[5-1].Id,Weight = 100},
                new Item(){Id = Guid.NewGuid().ToString(),Name = "Часть 5",OrderId = specification.Orders[5-1].Id,Weight = 100}
            };

            SpecificationXmlWork specificationXmlWork = new SpecificationXmlWork(specification);
            specificationXmlWork.MaxWeight = 30;

            ViewTableData expected = new ViewTableData()
            {
                ArrayList = new ArrayList()
                {
                    new ResultSpecification() {NumberOrder = "Заказ 1", CountContainer = 2, CountNotDimensional = 0},
                    new ResultSpecification() {NumberOrder = "Заказ 2", CountContainer = 1, CountNotDimensional = 2},
                    new ResultSpecification() {NumberOrder = "Заказ 3", CountContainer = 1, CountNotDimensional = 0},
                    new ResultSpecification() {NumberOrder = "Заказ 4", CountContainer = 0, CountNotDimensional = 4},
                    new ResultSpecification() {NumberOrder = "Заказ 5", CountContainer = 0, CountNotDimensional = 2},
                }
            };

            ViewTableData actual = specificationXmlWork.GetResult();

            Assert.AreEqual(expected.ArrayList.Count, actual.ArrayList.Count);
            for (int index = 0; index < actual.ArrayList.Count; index++)
            {
                var expect = expected.ArrayList[index] as ResultSpecification;
                var ac = actual.ArrayList[index] as ResultSpecification;
                Console.WriteLine(index);
                Assert.AreEqual(expect.CountContainer, ac.CountContainer);
                Assert.AreEqual(expect.CountNotDimensional, ac.CountNotDimensional);
            }
        }

        [TestMethod]
        public void Test_SpecificationXmlWork_AlgoritmAllThread()
        {
            Specification specification = new Specification
            {
                Orders = new List<Order>()
                {
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 1"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 2"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 3"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 4"},
                    new Order() {Id = Guid.NewGuid().ToString(), Name = "Заказ 5"}
                }
            };
            //Определяем 5 заказов
            //Определяем части заказа
            specification.Items = new List<Item>()
            {
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 1", OrderId = specification.Orders[1 - 1].Id, Weight = 10},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 1", OrderId = specification.Orders[1 - 1].Id, Weight = 10},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 1", OrderId = specification.Orders[1 - 1].Id, Weight = 10},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 1", OrderId = specification.Orders[1 - 1].Id, Weight = 10},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 2", OrderId = specification.Orders[2 - 1].Id, Weight = 100},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 2", OrderId = specification.Orders[2 - 1].Id, Weight = 100},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 2", OrderId = specification.Orders[2 - 1].Id, Weight = 5},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 3", OrderId = specification.Orders[3 - 1].Id, Weight = 10},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 3", OrderId = specification.Orders[3 - 1].Id, Weight = 20},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 4", OrderId = specification.Orders[4 - 1].Id, Weight = 50},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 4", OrderId = specification.Orders[4 - 1].Id, Weight = 50},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 4", OrderId = specification.Orders[4 - 1].Id, Weight = 50},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 4", OrderId = specification.Orders[4 - 1].Id, Weight = 50},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 5", OrderId = specification.Orders[5 - 1].Id, Weight = 100},
                new Item() {Id = Guid.NewGuid().ToString(), Name = "Часть 5", OrderId = specification.Orders[5 - 1].Id, Weight = 100}
            };

            SpecificationXmlWork specificationXmlWork = new SpecificationXmlWork(specification);
            specificationXmlWork.MaxWeight = 30;

            ViewTableData expected = new ViewTableData()
            {
                ArrayList = new ArrayList()
                {
                    new ResultSpecification() {NumberOrder = "Заказ 1", CountContainer = 2, CountNotDimensional = 0},
                    new ResultSpecification() {NumberOrder = "Заказ 2", CountContainer = 1, CountNotDimensional = 2},
                    new ResultSpecification() {NumberOrder = "Заказ 3", CountContainer = 1, CountNotDimensional = 0},
                    new ResultSpecification() {NumberOrder = "Заказ 4", CountContainer = 0, CountNotDimensional = 4},
                    new ResultSpecification() {NumberOrder = "Заказ 5", CountContainer = 0, CountNotDimensional = 2},
                }
            };

            ViewTableData actual = specificationXmlWork.GetResultAsync();

            Assert.AreEqual(expected.ArrayList.Count, actual.ArrayList.Count);
            foreach (Order order in specification.Orders)
            {
                var exp = expected.ArrayList.ToArray().Cast<ResultSpecification>().Where(item => item.NumberOrder == order.Name).ToArray();
                var ac = actual.ArrayList.ToArray().Cast<ResultSpecification>().Where(item => item.NumberOrder == order.Name).ToArray();
                Assert.AreEqual(exp.First().CountContainer, ac.First().CountContainer);
                Assert.AreEqual(exp.First().CountNotDimensional, ac.First().CountNotDimensional);
            }
        }
    }
}
using NUnit.Framework;
using RealEstate.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealEstate.Application.UnitTests
{
    public class IEnumerableExtensionTests
    {
        private IEnumerable<TestClass> Items;

        [SetUp]
        public void Setup()
        {
            Items = new List<TestClass>
            {
                new TestClass { Id = 1, Name = "1"},
                new TestClass { Id = 1, Name = "1"},
                new TestClass { Id = 1, Name = "1"},
                new TestClass { Id = 1, Name = "1"},
                new TestClass { Id = 2, Name = "2"},
                new TestClass { Id = 2, Name = "2"},
                new TestClass { Id = 2, Name = "2"},
                new TestClass { Id = 3, Name = "3"},
                new TestClass { Id = 3, Name = "3"},
                new TestClass { Id = 4, Name = "4"},
                new TestClass { Id = 5, Name = "5"}
            };
        }

        [Test]
        public void TakeTop1Item()
        {
            var items = Items.TakeTopNItems(1, item => item.Id);
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(1, items.First().Id);
        }

        [Test]
        public void TakeTop2Item()
        {
            var items = Items.TakeTopNItems(2, item => item.Id);
            Assert.AreEqual(2, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual(2, items.ElementAt(1).Id);
        }

        [Test]
        public void TakeTop3Item()
        {
            var items = Items.TakeTopNItems(3, item => item.Id);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual(2, items.ElementAt(1).Id);
            Assert.AreEqual(3, items.ElementAt(2).Id);
        }

        [Test]
        public void TakeTop4Item()
        {
            var items = Items.TakeTopNItems(4, item => item.Id);
            Assert.AreEqual(4, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual(2, items.ElementAt(1).Id);
            Assert.AreEqual(3, items.ElementAt(2).Id);
            Assert.AreEqual(4, items.ElementAt(3).Id);
        }

        [Test]
        public void TakeTop5Item()
        {
            var items = Items.TakeTopNItems(5, item => item.Id);
            Assert.AreEqual(5, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual(2, items.ElementAt(1).Id);
            Assert.AreEqual(3, items.ElementAt(2).Id);
            Assert.AreEqual(4, items.ElementAt(3).Id);
            Assert.AreEqual(5, items.ElementAt(4).Id);
        }

        [Test]
        public void TakeTop6Item()
        {
            Assert.Throws<ArgumentException>(() => Items.TakeTopNItems(6, item => item.Id));
        }

        private class TestClass
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
    }
}

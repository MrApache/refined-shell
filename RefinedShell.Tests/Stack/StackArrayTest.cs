using System;
using NUnit.Framework;
using RefinedShell.Stack;

namespace RefinedShell.Tests.Stack
{
    [TestFixture]
    [TestOf(typeof(StackArray<>))]
    internal sealed class StackArrayTest
    {
        [Test]
        public void Add()
        {
            StackArray<int> array = new StackArray<int>(stackalloc int[12]);
            bool add1 = array.Add(1);
            bool add2 = array.Add(2);
            bool add3 = array.Add(3);
            Assert.That(add1 && add2 && add3, Is.True);
        }

        [Test]
        public void AddExcessItems()
        {
            StackArray<int> array = new StackArray<int>(stackalloc int[3]);
            array.Add(1);
            array.Add(2);
            array.Add(3);
            bool add4 = array.Add(4);
            Assert.That(add4, Is.False);
        }

        [Test]
        public void Length()
        {
            StackArray<int> array = new StackArray<int>(stackalloc int[12]);
            array.Add(1);
            array.Add(2);
            array.Add(3);
            Assert.That(array.Length, Is.EqualTo(3));
        }

        [Test]
        public void RemoveLast()
        {
            StackArray<int> array = new StackArray<int>(stackalloc int[12]);
            array.Add(1);
            array.Add(2);
            array.Add(3);

            array.RemoveLast();
            array.RemoveLast();
            array.RemoveLast();
            Assert.That(array.Length, Is.EqualTo(0));
        }

        [Test]
        public void Clear()
        {
            StackArray<int> array = new StackArray<int>(stackalloc int[12]);
            array.Add(1);
            array.Add(2);
            array.Add(3);

            array.Clear();
            Assert.That(array.Length, Is.EqualTo(0));
        }

        [Test]
        public void Indexer()
        {
            StackArray<int> array = new StackArray<int>(stackalloc int[12]);
            array.Add(100);
            array.Add(993);
            array.Add(1007);

            Assert.That(array[0], Is.EqualTo(100));
            Assert.That(array[1], Is.EqualTo(993));
            Assert.That(array[2], Is.EqualTo(1007));
        }

        [Test]
        public void Indexer_OutOfRange_Exception()
        {
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                StackArray<int> array = new StackArray<int>(stackalloc int[3]);
                array.Add(100);
                array.Add(993);
                array.Add(1007);
                int unused = array[4];
            });
        }

        [Test]
        public void RemoveIfArrayIsEmpty()
        {
            StackArray<int> array = new StackArray<int>(stackalloc int[3]);
            bool result = array.RemoveLast();
            Assert.That(result, Is.False);
        }
    }
}
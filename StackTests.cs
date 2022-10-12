using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Wintellect.PowerCollections.Tests
{
    [TestClass]
    public class StackTests
    {
        [TestMethod]
        public void CountMustBeZero_EmptyStack_Test() // Проверка значения Count в пустом стеке (должно быть ноль)
        {
            Stack<string> tests = new Stack<string>(1);

            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void Count_NotEmptyStack_Test() // Проверка значения Count в заполненном стеке
        {
            Stack<int> tests = new Stack<int>(4);

            tests.Push(1);
            tests.Push(2);

            Assert.AreEqual(2, tests.Count); // Ожидается 2, потому что в стек передано 2 значения
        }


        [TestMethod]
        public void Capacity_Test() // Проверка значения Capacity при создании стека
        {
            Stack<bool> tests = new Stack<bool>(12);

            Assert.AreEqual(12, tests.Capacity); // Ожидается 12, потому что в конструктор передан размер - 12
        }

        [TestMethod]
        public void StackConstructor_WithParameters_Test() // Проверка работы конструктора при передаче параметров
        {
            Stack<bool> tests = new Stack<bool>(100);
        }

        [TestMethod]
        public void StackConstructor_WithNegativeParameters_Test() // Проверка работы конструктора при передаче в него отрицательного значения
        {
            try
            {
                Stack<int> tests2 = new Stack<int>(-1);
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual("Размер стека не может быть отрицательным", e.Message); // Ожидается Exception с сообщением - "Введено отрицательное значение"
            }
        }

        [TestMethod]
        public void PushMethod_Work_Test() // Проверка работы метода Push
        {
            Stack<int> tests = new Stack<int>(10);

            tests.Push(1);
            tests.Push(2);
            tests.Push(5);

            Assert.AreEqual(5, tests.Top()); // Поскольку последним было добавлено число 5, то и извлечься должно оно
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PushMethod_Overflow_Test() // Проверка метода Push при переполнении размера стека (в данном случае размер - 0)
        {
            Stack<int> tests = new Stack<int>(1);

            tests.Push(1);
            tests.Push(2);
        }

        [TestMethod]
        public void PopMethod_Work_Test() // Проверка работы метода Pop
        {
            Stack<int> tests = new Stack<int>(5);

            tests.Push(1);
            tests.Push(2);
            tests.Push(5);

            Assert.AreEqual(5, tests.Pop()); // Ожидается число 5, потому что оно на вершине
            Assert.AreEqual(2, tests.Pop()); // Ожидается число 2, потому что оно стал самым верхним
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PopMethod_EmptyStack_Test() // Проверка работы метода Pop при пустом стеке
        {
            Stack<int> tests = new Stack<int>(5);

            tests.Pop();
        }

        [TestMethod]
        public void TopMethod_Work_Test() // Проверка работы метода Top (Count не должен меняться в отличии от Pop метода)
        {
            Stack<int> tests = new Stack<int>(5);

            tests.Push(1);
            tests.Push(2);
            tests.Push(5);

            Assert.AreEqual(3, tests.Count);
            Assert.AreEqual(5, tests.Top());
            Assert.AreEqual(3, tests.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TopMethod_EmptyStack_Test() // Проверка работы метода Top при пустом стеке
        {
            Stack<int> tests = new Stack<int>(5);

            tests.Top();
        }

        [TestMethod]
        public void GetEnumerator_Work_Test() // Проверка работы GetEnumerator метода
        {
            Stack<int> tests = new Stack<int>(5);
            int[] expected = new int[] { 2, 7, 1, 75 };

            foreach (int a in expected)
            {
                tests.Push(a);
            }

            var actual = from n in tests select n;

            Assert.IsTrue(expected.Reverse().SequenceEqual(actual));
        }
    }
}

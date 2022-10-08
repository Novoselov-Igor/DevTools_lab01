using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wintellect.PowerCollections.Tests
{
    [TestClass]
    public class StackTests
    {
        [TestMethod]
        public void Count_EmptyStack_Test()
        {
            Stack<string> tests = new Stack<string>(1);

            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void Count_NotEmptyStack_Test()
        {
            Stack<int> tests = new Stack<int>(4);

            tests.Push(1);
            tests.Push(2);

            Assert.AreEqual(2, tests.Count);
        }


        [TestMethod]
        public void Capacity_Test() 
        {
            Stack<bool> tests = new Stack<bool>(12);

            Assert.AreEqual(12, tests.Capacity);
        }

        [TestMethod]
        public void StackConstructor_WithoutParameters_Test() 
        {
            Stack<int> tests = new Stack<int>();
        }

        [TestMethod]
        public void StackConstructor_WithParameters_Test()
        {
            Stack<bool> tests1 = new Stack<bool>(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StackConstructor_WithNegativeParameters_Test()
        {
            Stack<int> tests2 = new Stack<int>(-1);
        }

        [TestMethod]
        public void PushMethod_Work_Test()
        {
            Stack<int> tests = new Stack<int>(10);

            tests.Push(1);
            tests.Push(2);
            tests.Push(5);

            Assert.AreEqual(5, tests.Top());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PushMethod_Overflow_Test()
        {
            Stack<int> tests = new Stack<int>();

            tests.Push(1);
            tests.Push(2);
        }

        [TestMethod]
        public void PopMethod_Work_Test()
        {
            Stack<int> tests = new Stack<int>(5);

            tests.Push(1);
            tests.Push(2);
            tests.Push(5);

            Assert.AreEqual(3, tests.Count);
            Assert.AreEqual(5, tests.Pop());
            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PopMethod_EmptyStack_Test()
        {
            Stack<int> tests = new Stack<int>(5);

            tests.Pop();
        }

        [TestMethod]
        public void TopMethod_Work_Test()
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
        public void TopMethod_EmptyStack_Test()
        {
            Stack<int> tests = new Stack<int>(5);

            tests.Top();
        }

    }
}

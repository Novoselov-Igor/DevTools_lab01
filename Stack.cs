using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Wintellect.PowerCollections
{
    public class Stack<T> : IEnumerable<T>
    {
        public int Count { get; set; }
        public int Capacity { get; }

        T[] values;

        public Stack(int arrSize = 0)
        {
            if (arrSize < 0)
                throw new InvalidOperationException("Введено отрицательное значение");
            values = new T[arrSize];
            Capacity = arrSize;
        }

        public void Push(T value)
        {
            if (Count >= Capacity)
                throw new InvalidOperationException("Выход за пределы");
            values[Count] = value;
            Count++;
        }

        public T Pop()
        {
            if (Count <= 0)
            {
                throw new InvalidOperationException("Стэк пустой");
            }
            Count--;
            return values[Count];
        }

        public T Top()
        {
            if (Count <= 0)
            {
                throw new InvalidOperationException("Стэк пустой");
            }
            return values[Count-1];
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                yield return values [i];
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

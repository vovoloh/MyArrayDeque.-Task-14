using System;
using System.Collections;
using System.Collections.Generic;

namespace Task14
{
    public class MyArrayDeque<T> : IEnumerable<T>
    {
        private T[] elements;
        private int head;
        private int tail;
        private int count;

        public MyArrayDeque()
        {
            elements = new T[16];
            head = 0;
            tail = 0;
            count = 0;
        }

        public MyArrayDeque(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            int capacity = Math.Max(a.Length, 16);
            elements = new T[capacity];
            Array.Copy(a, elements, a.Length);
            head = 0;
            tail = a.Length;
            count = a.Length;
        }

        public MyArrayDeque(int numElements)
        {
            if (numElements < 0)
                throw new ArgumentOutOfRangeException(nameof(numElements), "Емкость не может быть отрицательной");

            int capacity = Math.Max(numElements, 4);
            elements = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }

        public void Add(T e) => AddLast(e);

        public void AddAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            foreach (var item in a)
                AddLast(item);
        }

        public void Clear()
        {
            Array.Clear(elements, 0, elements.Length);
            head = 0;
            tail = 0;
            count = 0;
        }

        public bool Contains(object o)
        {
            for (int i = 0; i < count; i++)
            {
                T element = GetElementAt(i);
                if (o == null)
                {
                    if (element == null) return true;
                }
                else if (o.Equals(element))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            foreach (var item in a)
                if (!Contains(item))
                    return false;
            return true;
        }

        public bool IsEmpty() => count == 0;

        public bool Remove(object o) => RemoveFirstOccurrence(o);

        public void RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            foreach (var item in a)
                while (Remove(item)) { }
        }

        public void RetainAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            var retained = new List<T>();
            for (int i = 0; i < count; i++)
            {
                T element = GetElementAt(i);
                if (Array.IndexOf(a, element) != -1)
                    retained.Add(element);
            }

            Clear();
            foreach (var item in retained)
                AddLast(item);
        }

        public int Size() => count;

        public T[] ToArray()
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
                result[i] = GetElementAt(i);
            return result;
        }

        public T[] ToArray(T[] a)
        {
            if (a == null)
                return ToArray();

            if (a.Length < count)
                a = new T[count];

            for (int i = 0; i < count; i++)
                a[i] = GetElementAt(i);

            if (a.Length > count)
                a[count] = default(T);

            return a;
        }

        public void AddFirst(T obj)
        {
            EnsureCapacity(count + 1);
            if (IsEmpty())
            {
                elements[head] = obj;
                tail = (tail + 1) % elements.Length;
            }
            else
            {
                head = (head - 1 + elements.Length) % elements.Length;
                elements[head] = obj;
            }

            count++;
        }

        public void AddLast(T obj)
        {
            EnsureCapacity(count + 1);
            elements[tail] = obj;
            tail = (tail + 1) % elements.Length;
            count++;
        }

        public T RemoveFirst()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");

            T item = elements[head];
            elements[head] = default(T);
            head = (head + 1) % elements.Length;
            count--;
            if (count == 0)
            {
                head = 0;
                tail = 0;
            }

            return item;
        }

        public T RemoveLast()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");

            tail = (tail - 1 + elements.Length) % elements.Length;
            T item = elements[tail];
            elements[tail] = default(T);
            count--;
            if (count == 0)
            {
                head = 0;
                tail = 0;
            }

            return item;
        }

        public T PollFirst()
        {
            if (IsEmpty())
                return default(T);

            return RemoveFirst();
        }

        public T PollLast()
        {
            if (IsEmpty())
                return default(T);

            return RemoveLast();
        }

        public T GetFirst()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");
            return elements[head];
        }

        public T GetLast()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");
            return elements[(tail - 1 + elements.Length) % elements.Length];
        }

        public T Peek()
        {
            if (IsEmpty())
                return default(T);
            return elements[head];
        }
        public T PeekFirst() => Peek();
        public T PeekLast()
        {
            if (IsEmpty())
                return default(T);
            return elements[(tail - 1 + elements.Length) % elements.Length];
        }
        public T Element() => GetFirst();
        public bool Offer(T obj)
        {
            try
            {
                AddLast(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool OfferFirst(T obj)
        {
            try
            {
                AddFirst(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool OfferLast(T obj) => Offer(obj);

        public T Pop() => RemoveFirst();

        public void Push(T obj) => AddFirst(obj);

        public bool RemoveFirstOccurrence(object obj)
        {
            if (count == 0) return false;

            for (int i = 0; i < count; i++)
            {
                T element = GetElementAt(i);
                if (IsEqual(element, obj))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveLastOccurrence(object obj)
        {
            if (count == 0) return false;

            for (int i = count - 1; i >= 0; i--)
            {
                T element = GetElementAt(i);
                if (IsEqual(element, obj))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > elements.Length)
            {
                int newCapacity = Math.Max(elements.Length * 2, minCapacity);
                T[] newArray = new T[newCapacity];
                for (int i = 0; i < count; i++)
                    newArray[i] = GetElementAt(i);

                elements = newArray;
                head = 0;
                tail = count;
            }
        }

        private T GetElementAt(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return elements[(head + index) % elements.Length];
        }

        private void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index == 0)
            {
                RemoveFirst();
                return;
            }

            if (index == count - 1)
            {
                RemoveLast();
                return;
            }
            for (int i = index; i < count - 1; i++)
            {
                int currentIdx = (head + i) % elements.Length;
                int nextIdx = (head + i + 1) % elements.Length;
                elements[currentIdx] = elements[nextIdx];
            }
            tail = (tail - 1 + elements.Length) % elements.Length;
            elements[tail] = default(T);
            count--;
        }
        private bool IsEqual(T element, object obj)
        {
            if (obj == null)
                return element == null;

            if (obj is T other)
                return element != null && element.Equals(other);

            return false;
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return GetElementAt(i);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            if (IsEmpty())
                return "Дек [пуст]";

            string result = "[";
            for (int i = 0; i < count; i++)
            {
                if (i > 0) result += ", ";
                result += GetElementAt(i);
            }
            result += "]";
            return result;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyArrayDeque<int> deque1 = new MyArrayDeque<int>();
            deque1.Add(1);
            deque1.Add(2);
            deque1.Add(3);
            Console.WriteLine($"Дек: {deque1}");
            deque1.AddFirst(23);
            Console.WriteLine($"Дек: {deque1}");

            Console.WriteLine($"Удален первый: {deque1.RemoveFirst()}");
            Console.WriteLine($"Удален последний: {deque1.RemoveLast()}");
            Console.WriteLine($"Дек после удалений: {deque1}");

            MyArrayDeque<int> deque2 = new MyArrayDeque<int>();
            deque2.Add(5);
            deque2.Add(10);
            deque2.Add(5);
            deque2.Add(15);
            deque2.Add(5);
            Console.WriteLine($"Исходный дек: {deque2}");
            Console.WriteLine($"Contains(10): {deque2.Contains(10)}");
            Console.WriteLine($"RemoveFirstOccurrence(5): {deque2.RemoveFirstOccurrence(5)}");
            Console.WriteLine($"После удаления первого вхождения 5: {deque2}");
            Console.WriteLine($"RemoveLastOccurrence(5): {deque2.RemoveLastOccurrence(5)}");
            Console.WriteLine($"После удаления последнего вхождения 5: {deque2}");
        }
    }
}

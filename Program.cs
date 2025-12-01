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
            
            int capacity = Math.Max(numElements, 16);
            elements = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }
        
        public void Add(T e)
        {
            AddLast(e);
        }
        
        public void AddAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            
            foreach (var item in a)
            {
                AddLast(item);
            }
        }
        
        public void Clear()
        {
            if (head < tail)
            {
                Array.Clear(elements, head, count);
            }
            else
            {
                Array.Clear(elements, head, elements.Length - head);
                Array.Clear(elements, 0, tail);
            }
            
            head = 0;
            tail = 0;
            count = 0;
        }
        
        public bool Contains(object o)
        {
            if (o == null)
            {
                if (head < tail)
                {
                    for (int i = head; i < tail; i++)
                        if (elements[i] == null)
                            return true;
                }
                else
                {
                    for (int i = head; i < elements.Length; i++)
                        if (elements[i] == null)
                            return true;
                    for (int i = 0; i < tail; i++)
                        if (elements[i] == null)
                            return true;
                }
                return false;
            }
            
            if (o is T item)
            {
                if (head < tail)
                {
                    for (int i = head; i < tail; i++)
                        if (item.Equals(elements[i]))
                            return true;
                }
                else
                {
                    for (int i = head; i < elements.Length; i++)
                        if (item.Equals(elements[i]))
                            return true;
                    for (int i = 0; i < tail; i++)
                        if (item.Equals(elements[i]))
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
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }
        
        public bool IsEmpty()
        {
            return count == 0;
        }
        
        public bool Remove(object o)
        {
            return RemoveFirstOccurrence(o);
        }
        
        public void RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            
            foreach (var item in a)
            {
                while (Remove(item)) { }
            }
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
                {
                    retained.Add(element);
                }
            }
            
            Clear();
            foreach (var item in retained)
            {
                AddLast(item);
            }
        }
        
        public int Size()
        {
            return count;
        }
        
        public T[] ToArray()
        {
            T[] result = new T[count];
            if (count == 0)
                return result;
            
            if (head < tail)
            {
                Array.Copy(elements, head, result, 0, count);
            }
            else
            {
                int firstPart = elements.Length - head;
                Array.Copy(elements, head, result, 0, firstPart);
                Array.Copy(elements, 0, result, firstPart, tail);
            }
            
            return result;
        }
        
        public T[] ToArray(T[] a)
        {
            if (a == null)
                return ToArray();
            
            if (a.Length < count)
            {
                a = new T[count];
            }
            
            if (count == 0)
                return a;
            
            if (head < tail)
            {
                Array.Copy(elements, head, a, 0, count);
            }
            else
            {
                int firstPart = elements.Length - head;
                Array.Copy(elements, head, a, 0, firstPart);
                Array.Copy(elements, 0, a, firstPart, tail);
            }
            
            if (a.Length > count)
            {
                a[count] = default(T);
            }
            
            return a;
        }
        
        public T Element()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");
            
            return elements[head];
        }
        
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
        
        public T Peek()
        {
            if (IsEmpty())
                return default(T);
            
            return elements[head];
        }
        
        public T Poll()
        {
            if (IsEmpty())
                return default(T);
            
            return PollFirst();
        }
        
        public void AddFirst(T obj)
        {
            EnsureCapacity(count + 1);
            head = (head - 1 + elements.Length) % elements.Length;
            elements[head] = obj;
            count++;
        }
        
        public void AddLast(T obj)
        {
            EnsureCapacity(count + 1);
            elements[tail] = obj;
            tail = (tail + 1) % elements.Length;
            count++;
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
        
        public bool OfferLast(T obj)
        {
            return Offer(obj);
        }
        
        public T Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");
            
            return RemoveFirst();
        }
        
        public void Push(T obj)
        {
            AddFirst(obj);
        }
        
        public T PeekFirst()
        {
            return Peek();
        }
        
        public T PeekLast()
        {
            if (IsEmpty())
                return default(T);
            
            return elements[(tail - 1 + elements.Length) % elements.Length];
        }
        
        public T PollFirst()
        {
            if (IsEmpty())
                return default(T);
            
            T item = elements[head];
            elements[head] = default(T);
            head = (head + 1) % elements.Length;
            count--;
            return item;
        }
        
        public T PollLast()
        {
            if (IsEmpty())
                return default(T);
            
            tail = (tail - 1 + elements.Length) % elements.Length;
            T item = elements[tail];
            elements[tail] = default(T);
            count--;
            return item;
        }
        
        public T RemoveLast()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");
            
            return PollLast();
        }
        
        public T RemoveFirst()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Дек пуст");
            
            return PollFirst();
        }
        
        public bool RemoveLastOccurrence(object obj)
        {
            if (count == 0)
                return false;
            
            int foundIndex = -1;
            
            if (head < tail)
            {
                for (int i = tail - 1; i >= head; i--)
                {
                    if (IsEqual(elements[i], obj))
                    {
                        foundIndex = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = tail - 1; i >= 0; i--)
                {
                    if (IsEqual(elements[i], obj))
                    {
                        foundIndex = i;
                        break;
                    }
                }
                
                if (foundIndex == -1)
                {
                    for (int i = elements.Length - 1; i >= head; i--)
                    {
                        if (IsEqual(elements[i], obj))
                        {
                            foundIndex = i;
                            break;
                        }
                    }
                }
            }
            
            if (foundIndex == -1)
                return false;
            
            RemoveAt(foundIndex);
            return true;
        }
        
        public bool RemoveFirstOccurrence(object obj)
        {
            if (count == 0)
                return false;
            
            int foundIndex = -1;
            
            if (head < tail)
            {
                for (int i = head; i < tail; i++)
                {
                    if (IsEqual(elements[i], obj))
                    {
                        foundIndex = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = head; i < elements.Length; i++)
                {
                    if (IsEqual(elements[i], obj))
                    {
                        foundIndex = i;
                        break;
                    }
                }
                
                if (foundIndex == -1)
                {
                    for (int i = 0; i < tail; i++)
                    {
                        if (IsEqual(elements[i], obj))
                        {
                            foundIndex = i;
                            break;
                        }
                    }
                }
            }
            
            if (foundIndex == -1)
                return false;
            
            RemoveAt(foundIndex);
            return true;
        }
        
        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > elements.Length)
            {
                int newCapacity = elements.Length * 2;
                if (newCapacity < minCapacity)
                    newCapacity = minCapacity;
                
                T[] newArray = new T[newCapacity];
                
                if (head < tail)
                {
                    Array.Copy(elements, head, newArray, 0, count);
                }
                else
                {
                    int firstPart = elements.Length - head;
                    Array.Copy(elements, head, newArray, 0, firstPart);
                    Array.Copy(elements, 0, newArray, firstPart, tail);
                }
                
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
            if (count == 0)
                return;
            
            if (count == 1)
            {
                Clear();
                return;
            }
            
            // Определяем, в какой части массива находится элемент
            int actualIndex;
            if (head <= index && index < (head < tail ? tail : elements.Length))
            {
                // Элемент в первой части
                actualIndex = index;
            }
            else if (head >= tail && index < tail)
            {
                // Элемент во второй части (когда head > tail)
                actualIndex = index;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            // Если удаляется первый элемент
            if (actualIndex == head)
            {
                elements[head] = default(T);
                head = (head + 1) % elements.Length;
                count--;
                return;
            }
            
            // Если удаляется последний элемент
            int lastIndex = (tail - 1 + elements.Length) % elements.Length;
            if (actualIndex == lastIndex)
            {
                elements[lastIndex] = default(T);
                tail = lastIndex;
                count--;
                return;
            }
            
            // Удаление из середины - сдвигаем элементы
            if (head < tail)
            {
                // Линейный массив
                Array.Copy(elements, actualIndex + 1, elements, actualIndex, tail - actualIndex - 1);
                tail--;
                elements[tail] = default(T);
            }
            else if (actualIndex >= head)
            {
                // Элемент в первой части (от head до конца массива)
                Array.Copy(elements, actualIndex + 1, elements, actualIndex, elements.Length - actualIndex - 1);
                
                // Переносим последний элемент из второй части
                if (tail > 0)
                {
                    elements[elements.Length - 1] = elements[0];
                    Array.Copy(elements, 1, elements, 0, tail - 1);
                    tail--;
                }
                else
                {
                    tail = elements.Length - 1;
                }
            }
            else
            {
                // Элемент во второй части (от 0 до tail)
                Array.Copy(elements, actualIndex + 1, elements, actualIndex, tail - actualIndex - 1);
                tail--;
            }
            
            count--;
        }
        
        private bool IsEqual(T element, object obj)
        {
            if (obj == null)
                return element == null;
            
            if (obj is T other)
                return element.Equals(other);
            
            return false;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            if (head < tail)
            {
                for (int i = head; i < tail; i++)
                    yield return elements[i];
            }
            else
            {
                for (int i = head; i < elements.Length; i++)
                    yield return elements[i];
                for (int i = 0; i < tail; i++)
                    yield return elements[i];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public override string ToString()
        {
            if (IsEmpty())
                return "Дек [пуст]";
            
            string result = "[";
            bool first = true;
            
            if (head < tail)
            {
                for (int i = head; i < tail; i++)
                {
                    if (!first) result += ", ";
                    result += elements[i];
                    first = false;
                }
            }
            else
            {
                for (int i = head; i < elements.Length; i++)
                {
                    if (!first) result += ", ";
                    result += elements[i];
                    first = false;
                }
                for (int i = 0; i < tail; i++)
                {
                    if (!first) result += ", ";
                    result += elements[i];
                    first = false;
                }
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
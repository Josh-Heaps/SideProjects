#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8601 // Possible null reference assignment.
using System.Collections;

namespace ListButWorse
{
    public class ListButWorse<T> : IList<T>
    {
        public int Count { get; set; }
        public bool IsReadOnly => false;

        private static readonly T[] _emptyArray = [];
        private T[] _values;

        public ListButWorse()
        {
            Count = 0;
            _values = _emptyArray;
        }

        public ListButWorse(IEnumerable<T> enumerable)
        {
            ArgumentNullException.ThrowIfNull(enumerable);

            Count = 0;
            _values = _emptyArray;
            using IEnumerator<T> enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
                Add(enumerator.Current);
        }

        public T this[int index]
        {
            get
            {
                if (index >= Count || index < 0)
                    throw new IndexOutOfRangeException();

                return _values[index];
            }
            set
            {
                if (index >= Count || index < 0)
                    throw new IndexOutOfRangeException();

                _values[index] = value;
            }
        }

        public static ListButWorse<T> operator +(ListButWorse<T> list, T value) { list.Add(value); return list; }
        public static ListButWorse<T> operator +(ListButWorse<T> listOne, ListButWorse<T> listTwo)
        {
            ListButWorse<T> result = new(listOne);
            result.AddRange(listTwo);

            return result;
        }

        public static ListButWorse<T> operator -(ListButWorse<T> list, T value) { list.Remove(value); return list; }

        public void Add(T item)
        {
            ArgumentNullException.ThrowIfNull(item);
            _values ??= _emptyArray;
            IncreaseSizeIfNeeded();
            _values[Count++] = item;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            if (collection is ICollection<T> c)
            {
                IncreaseSizeIfNeeded(c.Count);
                c.CopyTo(_values, Count);

                return;
            }

            var enumerator = collection.GetEnumerator();

            while (enumerator.MoveNext())
                Add(enumerator.Current);
        }

        public void Clear()
        {
            Count = 0;
            _values = _emptyArray;
        }

        public bool Contains(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            foreach (var i in _values)
                if (comparer.Equals(item, i))
                    return true;

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_values, 0, array, arrayIndex, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public bool Remove(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (!Contains(item))
                return false;

            int index = IndexOf(item);
            RemoveAt(index);

            return true;
        }

        public int IndexOf(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < Count; i++)
                if (comparer.Equals(_values[i], item))
                    return i;

            return -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            ArgumentNullException.ThrowIfNull(item);
            IncreaseSizeIfNeeded();

            if (index < Count)
                Array.Copy(_values, index, _values, index + 1, Count - index);

            _values[index] = item;
            Count++;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Count--;
            Array.Copy(_values, index + 1, _values, index, Count - index);
            _values[Count] = default(T);

            if (Count < _values.Length / 2)
            {
                T[] values = new T[Count];
                Array.Copy(_values, values, values.Length);
                _values = values;
            }
        }

        private void IncreaseSizeIfNeeded(int requiredCapacity = 1)
        {
            requiredCapacity += Count;

            if (requiredCapacity >= _values.Length)
            {
                int newSize = Count * 2 > 0 ? Count * 2 : 1;

                if (newSize > Array.MaxLength)
                    newSize = Array.MaxLength;

                newSize = Math.Max(newSize, requiredCapacity);

                T[] values = new T[newSize];
                Array.Copy(_values, values, _values.Length);
                _values = values;
            }
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private int index;
            private readonly ListButWorse<T> list;

            public Enumerator(ListButWorse<T> linkedList)
            {
                index = 0;
                Current = linkedList[index];
                list = linkedList;
            }

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                if (index == list.Count)
                    return false;

                Current = list[index++];
                return true;
            }

            public void Reset()
            {
                index = 0;
                Current = list[index];
            }

            public T Current { get; private set; }

            readonly object IEnumerator.Current => Current;
        }
    }
}

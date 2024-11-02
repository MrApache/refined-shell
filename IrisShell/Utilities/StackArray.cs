using System;

namespace IrisShell.Stack
{
    internal ref struct StackArray<T> where T : unmanaged
    {
        private readonly Span<T> _buffer;
        private short _position;

        public readonly int Length => _position;

        public T this[int index]
        {
            get
            {
                if (index >= _position)
                    throw new IndexOutOfRangeException();
                return _buffer[index];
            }
        }

        public StackArray(Span<T> buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public bool Add(T item)
        {
            if (_position >= _buffer.Length)
                return false;

            _buffer[_position++] = item;
            return true;
        }

        public bool RemoveLast()
        {
            if (_position == 0)
                return false;

            _buffer[_position--] = default;
            return true;
        }

        public void Clear()
        {
            while(_position > 0)
                RemoveLast();
        }

        public T GetLast()
        {
            if (_position <= 0)
                return default;

            return _buffer[_position - 1];
        }
    }
}
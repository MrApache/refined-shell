using System;

namespace IrisShell
{
    internal struct StringBuilder
    {
        private readonly char[] _buffer;
        private uint _position;

        public uint Length => _position;

        public StringBuilder(uint size)
        {
            _buffer = new char[size];
            _position = 0;
        }

        public bool Append(char value)
        {
            if (_position >= _buffer.Length)
                return false;

            _buffer[_position++] = value;
            return true;
        }

        public bool Append(string value)
        {
            if (_position >= _buffer.Length)
                return false;

            foreach (char character in value)
            {
                _buffer[_position++] = character;
                if (_position >= _buffer.Length)
                    return false;
            }

            return true;
        }

        public void RemoveAt(uint position)
        {
            if (_position == 0 || position > _position)
                return;

            if (position == _position)
            {
                RemoveLast();
                return;
            }

            for (uint i = position; i != _position; i++)
            {
                _buffer[i] = _buffer[i + 1];
            }
            _buffer[--_position] = '\0';
        }

        public void RemoveLast()
        {
            if (_position == 0)
                return;
            _buffer[--_position] = '\0';
        }

        public readonly ReadOnlySpan<char> AsSpan()
        {
            return _buffer;
        }

        public readonly ReadOnlyMemory<char> AsMemory()
        {
            return _buffer;
        }

        public override string ToString()
        {
            return new string(_buffer);
        }
    }
}
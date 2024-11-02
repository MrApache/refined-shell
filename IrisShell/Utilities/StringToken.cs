using System;

namespace IrisShell.Utilities
{
    public readonly struct StringToken : IEquatable<StringToken>, IEquatable<string>, IEquatable<ReadOnlyMemory<char>>
    {
        private readonly ReadOnlyMemory<char> _value;
        public int Length => _value.Length;

        private StringToken(ReadOnlyMemory<char> value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public ReadOnlyMemory<char> ToMemory()
        {
            return _value;
        }

        public ReadOnlySpan<char> ToSpan()
        {
            return _value.Span;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            ReadOnlySpan<char> span = _value.Span;
            for (int i = 0; i < _value.Length; i++)
            {
                hash += span[i] * (span[i] ^ _value.Length)
                        + (_value.Span[i] ^ i)
                        + (hash ^ _value.Length);
            }
            return hash;
        }

        public bool Equals(string? other)
        {
            return other != null && Equals(other.AsMemory());
        }

        public bool Equals(ReadOnlyMemory<char> other)
        {
            return _value.Span.SequenceEqual(other.Span);
        }

        public bool Equals(StringToken other)
        {
            return Equals(other._value);
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                null => false,
                ReadOnlyMemory<char> rom => Equals(rom),
                string str => Equals(str),
                _ => false
            };
        }

        public static bool operator ==(StringToken left, StringToken right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringToken left, StringToken right)
        {
            return !(left == right);
        }

        public static implicit operator StringToken(string value)
        {
            return new StringToken(value.AsMemory());
        }

        public static implicit operator StringToken(ReadOnlyMemory<char> value)
        {
            return new StringToken(value);
        }

        public static implicit operator ReadOnlySpan<char>(StringToken value)
        {
            return value._value.Span;
        }
    }
}
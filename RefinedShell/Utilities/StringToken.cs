using System;

namespace RefinedShell.Utilities
{
    /// <summary>
    /// A struct that serves as a wrapper for <see cref="ReadOnlyMemory{T}"/>
    /// Implicitly converts from <see cref="string"/> and <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    public readonly struct StringToken : IEquatable<StringToken>, IEquatable<string>, IEquatable<ReadOnlyMemory<char>>
    {
        private readonly ReadOnlyMemory<char> _value;

        /// <summary>
        /// Gets the length of the string.
        /// </summary>
        public int Length => _value.Length;

        private StringToken(ReadOnlyMemory<char> value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns a new instance of <see cref="String"/>
        /// </summary>
        /// <returns><see cref="String"/></returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Explicitly converts a <see cref="StringToken"/> to a <see cref="ReadOnlyMemory{T}"/>
        /// </summary>
        /// <returns><see cref="ReadOnlyMemory{T}"/></returns>
        public ReadOnlyMemory<char> ToMemory()
        {
            return _value;
        }

        /// <summary>
        /// Explicitly converts a <see cref="StringToken"/> to a <see cref="ReadOnlyMemory{T}"/> starting from the specified index.
        /// </summary>
        /// <param name="start">The zero-based starting index for the conversion.</param>
        /// <returns>
        /// <see cref="ReadOnlyMemory{T}"/>
        /// </returns>
        public ReadOnlyMemory<char> ToMemory(int start)
        {
            return _value[start..];
        }

        /// <summary>
        /// Explicitly converts a specified range of the <see cref="StringToken"/> to a <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        /// <param name="start">The zero-based starting index for the conversion.</param>
        /// <param name="length">The number of characters to include in the <see cref="ReadOnlyMemory{T}"/>.</param>
        /// <returns>
        /// <see cref="ReadOnlyMemory{T}"/></returns>
        public ReadOnlyMemory<char> ToMemory(int start, int length)
        {
            return _value.Slice(start, length);
        }

        /// <summary>
        /// Explicitly converts a <see cref="StringToken"/> to a <see cref="ReadOnlySpan{T}"/>
        /// </summary>
        /// <returns><see cref="ReadOnlySpan{T}"/></returns>
        public ReadOnlySpan<char> ToSpan()
        {
            return _value.Span;
        }

        /// <summary>
        /// Explicitly converts a <see cref="StringToken"/> to a <see cref="ReadOnlySpan{T}"/> starting from the specified index.
        /// </summary>
        /// <param name="start">The zero-based starting index for the conversion.</param>
        /// <returns>
        /// <see cref="ReadOnlySpan{T}"/>
        /// </returns>
        public ReadOnlySpan<char> ToSpan(int start)
        {
            return _value.Span[start..];
        }

        /// <summary>
        /// Explicitly converts a specified range of the <see cref="StringToken"/> to a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="start">The zero-based starting index for the conversion.</param>
        /// <param name="length">The number of characters to include in the <see cref="ReadOnlySpan{T}"/>.</param>
        /// <returns>
        /// <see cref="ReadOnlySpan{T}"/></returns>
        public ReadOnlySpan<char> ToSpan(int start, int length)
        {
            return _value.Span.Slice(start, length);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="StringToken"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="StringToken"/>.</returns>
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

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="string"/>.
        /// </summary>
        /// <param name="other">The <see cref="string"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(string? other)
        {
            return other != null && Equals(other.AsMemory());
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="ReadOnlyMemory{T}"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(ReadOnlyMemory<char> other)
        {
            return _value.Span.SequenceEqual(other.Span);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="StringToken"/>.
        /// </summary>
        /// <param name="other">The <see cref="StringToken"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(StringToken other)
        {
            return Equals(other._value);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified object.
        /// </summary>
        /// <param name="obj">The <see cref="StringToken"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Compares whether two <see cref="StringToken"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="StringToken"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="StringToken"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(StringToken a, StringToken b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares whether two <see cref="StringToken"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="StringToken"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="StringToken"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(StringToken a, StringToken b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Implicitly converts a <see cref="string"/> to a <see cref="StringToken"/>
        /// </summary>
        public static implicit operator StringToken(string value)
        {
            return new StringToken(value.AsMemory());
        }

        /// <summary>
        /// Implicitly converts a <see cref="ReadOnlyMemory{T}"/> to a <see cref="StringToken"/>
        /// </summary>
        public static implicit operator StringToken(ReadOnlyMemory<char> value)
        {
            return new StringToken(value);
        }

        /// <summary>
        /// Implicitly converts a <see cref="StringToken"/> to a <see cref="ReadOnlySpan{T}"/>
        /// </summary>
        public static implicit operator ReadOnlySpan<char>(StringToken value)
        {
            return value._value.Span;
        }
    }
}
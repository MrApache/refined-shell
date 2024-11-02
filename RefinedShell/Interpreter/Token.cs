using System;

namespace RefinedShell.Interpreter
{
    internal readonly struct Token : IEquatable<Token>
    {
        public readonly int Start;
        public readonly int Length;
        public readonly TokenType Type;

        public Token(int start, int length, TokenType type)
        {
            Start = start;
            Length = length;
            Type = type;
        }

        public bool Equals(Token other)
        {
            return Start == other.Start
                && Length == other.Length
                && Type == other.Type;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Token token)
                return Equals(token);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length, Type);
        }

        public override string ToString() => $"{Type}";

        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !(left == right);
        }
    }
}
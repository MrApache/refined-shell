using System;

namespace RefinedShell.Matching
{
    internal sealed class Token : IEquatable<string>, IEquatable<Token>
    {
        public readonly string Value;
        public readonly Modifier Modifier;

        public Token(string token, Modifier modifier = Modifier.None)
        {
            Value = token;
            Modifier = modifier;
        }

        public bool Equals(Token other)
        {
            return Equals(other.Value);
        }

        public bool Equals(string other)
        {
            return other.Equals(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is Token token
                && Equals(token.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static string CreateRange(char start, char end)
        {
            int length = end - start + 1;
            Span<char> temp = stackalloc char[length];
            for (int i = 0; i < length; i++) {
                temp[i] = (char)(start + i);
            }
            return temp.ToString();
        }
    }
}

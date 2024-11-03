using System;
using RefinedShell.Execution;

namespace RefinedShell.Interpreter
{
    internal readonly struct SemanticError : IEquatable<SemanticError>
    {
        public readonly int Start;
        public readonly int Length;
        public readonly ExecutionError Error;

        internal SemanticError(int start, int length, ExecutionError error)
        {
            Start = start;
            Length = length;
            Error = error;
        }

        public bool Equals(SemanticError other)
        {
            return Start == other.Start && Length == other.Length && Error == other.Error;
        }

        public override bool Equals(object? obj)
        {
            return obj is SemanticError other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length, (int)Error);
        }

        public override string ToString() => Error.ToString();

        internal static SemanticError None => new SemanticError(0, 0, ExecutionError.None);

        public static bool operator ==(SemanticError left, SemanticError right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SemanticError left, SemanticError right)
        {
            return !(left == right);
        }
    }
}
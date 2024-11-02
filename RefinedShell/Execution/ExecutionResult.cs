using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace RefinedShell.Execution
{
    public readonly struct ExecutionResult : IEquatable<ExecutionResult>
    {
        public readonly bool Success;
        public readonly object? ReturnValue;

        public ExecutionResult(bool success, object? returnValue)
        {
            Success = success;
            ReturnValue = returnValue;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ExecutionResult er && Equals(er);
        }

        public bool Equals(ExecutionResult other)
        {
            return Success == other.Success && CheckReturnValueEquality(ReturnValue, other.ReturnValue);
        }

        private static bool CheckReturnValueEquality(object? left, object? right)
        {
            if (left == right)
            {
                return true;
            }
            if (left == null || right == null)
            {
                return false;
            }

            if (left is ICollection leftCollection && right is ICollection rightCollection)
                return SequenceEquals(leftCollection, rightCollection);

            return Equals(left, right);
        }

        private static bool SequenceEquals(ICollection left, ICollection right)
        {
            if (left.Count != right.Count)
                return false;

            IEnumerator e1 = left.GetEnumerator();
            IEnumerator e2 = right.GetEnumerator();
            try
            {

                while (e1.MoveNext())
                {
                    if (!(e2.MoveNext() && Equals(e1.Current, e2.Current)))
                    {
                        return false;
                    }
                }

                return !e2.MoveNext();
            }
            finally
            {
                if(e1 is IDisposable disposableE1)
                    disposableE1.Dispose();
                if(e2 is IDisposable disposableE2)
                    disposableE2.Dispose();
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Success, ReturnValue);
        }

        public override string ToString()
        {
            return $"Success: {Success}, Return: {ReturnValue}";
        }

        public static bool operator ==(ExecutionResult left, ExecutionResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExecutionResult left, ExecutionResult right)
        {
            return !left.Equals(right);
        }

        public static ExecutionResult Fail => new ExecutionResult(false, null);
    }
}
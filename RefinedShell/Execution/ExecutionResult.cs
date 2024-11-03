using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace RefinedShell.Execution
{
    /// <summary>
    /// Represents the result of executing a command, indicating success and returning a value.
    /// </summary>
    public readonly struct ExecutionResult : IEquatable<ExecutionResult>
    {
        /// <summary>
        /// Gets value indicating whether the execution was successful.
        /// </summary>
        public readonly bool Success;
        public readonly int Start;
        public readonly int Length;
        public readonly ExecutionError Error;

        /// <summary>
        /// Gets value returned by the execution, if any.
        /// </summary>
        public readonly object? ReturnValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionResult"/> struct.
        /// </summary>
        /// <param name="success">Indicates whether the execution was successful.</param>
        /// <param name="returnValue">The value returned by the execution.</param>
        public ExecutionResult(bool success, int start, int length, ExecutionError error, object? returnValue)
        {
            Success = success;
            Start = start;
            Length = length;
            Error = error;
            ReturnValue = returnValue;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified object.
        /// </summary>
        /// <param name="obj">The <see cref="ExecutionResult"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ExecutionResult er && Equals(er);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="ExecutionResult"/>.
        /// </summary>
        /// <param name="other">The <see cref="ExecutionResult"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(ExecutionResult other)
        {
            return Success == other.Success
                && Error == other.Error
                && CheckReturnValueEquality(ReturnValue, other.ReturnValue);
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

        /// <summary>
        /// Gets the hash code of this <see cref="ExecutionResult"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="ExecutionResult"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Success, ReturnValue);
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="ExecutionResult"/> in the format:
        /// {Success:[bool] Return:[object]}
        /// </summary>
        /// <returns><see cref="String"/> representation of this <see cref="ExecutionResult"/>.</returns>
        public override string ToString()
        {
            return $"Success: {Success}, Return: {ReturnValue}";
        }

        /// <summary>
        /// Compares whether two <see cref="ExecutionResult"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="ExecutionResult"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="ExecutionResult"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(ExecutionResult a, ExecutionResult b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares whether two <see cref="ExecutionResult"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="ExecutionResult"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="ExecutionResult"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(ExecutionResult a, ExecutionResult b)
        {
            return !a.Equals(b);
        }
    }
}
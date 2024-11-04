using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using RefinedShell.Execution;
using RefinedShell.Utilities;

namespace RefinedShell
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

        /// <summary>
        /// Gets the segment of the input string associated with any problems encountered during execution.
        /// </summary>
        public readonly ProblemSegment Segment;

        /// <summary>
        /// Gets the execution error associated with the segment.
        /// </summary>
        public ExecutionError Error => Segment.Error;

        /// <summary>
        /// Gets value returned by the execution, if any.
        /// </summary>
        public readonly object? ReturnValue;

        /*
        /// <param name="success">Indicates whether the execution was successful.</param>
        /// <param name="returnValue">The value returned by the execution.</param>
        */

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionResult"/> struct.
        /// </summary>
        public ExecutionResult(bool success, int start, int length, ExecutionError error, object? returnValue)
        {
            Success = success;
            Segment = new ProblemSegment(start, length, error);
            ReturnValue = returnValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionResult"/> struct.
        /// </summary>
        public ExecutionResult(bool success, object? returnValue, ProblemSegment segment)
        {
            Success = success;
            ReturnValue = returnValue;
            Segment = segment;
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
                && Segment == other.Segment
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
                return leftCollection.SequenceEqual(rightCollection);

            return Equals(left, right);
        }



        /// <summary>
        /// Gets the hash code of this <see cref="ExecutionResult"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="ExecutionResult"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Success, Segment, ReturnValue);
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
        /// Deconstructs the current instance into its constituent parts.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful.</param>
        /// <param name="segment">Contains information about the problem segment, if any.</param>
        /// <param name="returnValue">The value returned by the operation. May be null.</param>
        public void Deconstruct(out bool success, out ProblemSegment segment, out object? returnValue)
        {
            success = Success;
            segment = Segment;
            returnValue = ReturnValue;
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
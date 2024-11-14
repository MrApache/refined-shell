using System;
using RefinedShell.Execution;

namespace RefinedShell
{
    /// <summary>
    /// Represents a segment of a string that contains a problem, indicating the starting position,
    /// length, and associated execution error.
    /// </summary>
    public readonly struct ProblemSegment : IEquatable<ProblemSegment>
    {
        /// <summary>
        /// Gets the starting position of the segment in the string.
        /// </summary>
        public readonly int Start;

        /// <summary>
        /// Gets the length of the segment in the string.
        /// </summary>
        public readonly int Length;

        /// <summary>
        /// Gets the execution error associated with the segment.
        /// </summary>
        public readonly ExecutionError Error;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemSegment"/> structure.
        /// </summary>
        /// <param name="start">The starting position of the segment in the string.</param>
        /// <param name="length">The length of the segment in the string.</param>
        /// <param name="error">The execution error associated with the segment.</param>
        public ProblemSegment(int start, int length, ExecutionError error)
        {
            Start = start;
            Length = length;
            Error = error;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="ProblemSegment"/>.
        /// </summary>
        /// <param name="other">The <see cref="ProblemSegment"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(ProblemSegment other)
        {
            return Start == other.Start
                   && Length == other.Length
                   && Error == other.Error;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified object.
        /// </summary>
        /// <param name="obj">The <see cref="ProblemSegment"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return obj is ProblemSegment other && Equals(other);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="ProblemSegment"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="ProblemSegment"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length, Error);
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="ProblemSegment"/> in the format:
        /// {Success:[bool] Return:[object]}
        /// </summary>
        /// <returns><see cref="String"/> representation of this <see cref="ProblemSegment"/>.</returns>
        public override string ToString() => Error.ToString();

        /// <summary>
        /// Gets a <see cref="ProblemSegment"/> instance that represents the absence of a problem.
        /// This instance has a starting position of 0, a length of 0, and an associated execution error of <see cref="ExecutionError.None"/>.
        /// </summary>
        /// <remarks>
        /// This static property can be used to signify that there are no problems detected in the string segment.
        /// </remarks>
        /// <returns>A <see cref="ProblemSegment"/> instance representing no problem.</returns>
        public static ProblemSegment None => new ProblemSegment(0, 0, ExecutionError.None);

        public static ProblemSegment Empty => new ProblemSegment(0, 0, ExecutionError.EmptyInput);

        public static ProblemSegment CommandNotValid => new ProblemSegment(0, 0, ExecutionError.CommandNotValid);

        /// <summary>
        /// Compares whether two <see cref="ProblemSegment"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="ProblemSegment"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="ProblemSegment"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(ProblemSegment a, ProblemSegment b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares whether two <see cref="ProblemSegment"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="ProblemSegment"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="ProblemSegment"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(ProblemSegment a, ProblemSegment b)
        {
            return !(a == b);
        }
    }
}
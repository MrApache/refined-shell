using System;

namespace RefinedShell.Parsing
{
    /// <summary>
    /// Defines a contract for a type parser that can parse input into a specific type.
    /// </summary>
    public interface ITypeParser
    {
        /// <summary>
        /// Gets the number of arguments available for parsing.
        /// </summary>
        public uint ArgumentCount { get; }

        /// <summary>
        /// Gets the number of optional arguments available for parsing.
        /// </summary>
        public uint OptionalCount { get; }
        
        /// <summary>
        /// Determines whether the parser can parse the specified input.
        /// </summary>
        /// <param name="input">The input to check for parse capability.</param>
        /// <returns><c>true</c> if the parser can parse the input; otherwise, <c>false</c>.</returns>
        public bool CanParse(ReadOnlySpan<string?> input);

        /// <summary>
        /// Parses the specified input into an object of the target type.
        /// </summary>
        /// <param name="input">The input to parse.</param>
        /// <returns>The parsed object.</returns>
        public object Parse(ReadOnlySpan<string?> input);
    }
}
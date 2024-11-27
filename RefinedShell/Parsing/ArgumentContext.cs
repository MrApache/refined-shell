namespace RefinedShell.Parsing
{
    /// <summary>
    /// Provides information about an argument
    /// </summary>
    public readonly struct ArgumentInfo
    {
        /// <summary>
        /// Gets the number of elements associated with the argument.
        /// </summary>
        public readonly uint ElementCount;

        /// <summary>
        /// Indicates whether the argument is optional.
        /// </summary>
        public readonly bool IsOptional;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentInfo"/> struct with specified element count and required status.
        /// </summary>
        /// <param name="elementCount">The number of elements associated with the argument.</param>
        /// <param name="isOptional">Indicates if the argument is optional.</param>
        public ArgumentInfo(uint elementCount, bool isOptional)
        {
            ElementCount = elementCount;
            IsOptional = isOptional;
        }
    }

}
namespace RefinedShell.Execution
{
    /// <summary>
    /// Represents the various types of errors that can occur during command execution.
    /// </summary>
    public enum ExecutionError : byte
    {
        /// <summary>
        /// Indicates that no errors were found
        /// </summary>
        None,

        /// <summary>
        /// Indicates that an unknown token was encountered.
        /// </summary>
        UnknownToken,

        /// <summary>
        /// Indicates that the token was used in an invalid way.
        /// </summary>
        InvalidUsageOfToken,

        /// <summary>
        /// Indicates that an unexpected token was found (when a different token was expected).
        /// </summary>
        UnexpectedToken,

        /// <summary>
        /// Indicates that a command was not found.
        /// </summary>
        CommandNotFound,

        /// <summary>
        /// Indicates that not enough arguments were passed.
        /// </summary>
        InsufficientArguments,

        /// <summary>
        /// Indicates that too many arguments were passed.
        /// </summary>
        TooManyArguments,

        /// <summary>
        /// Indicates that an argument type is invalid.
        /// </summary>
        InvalidArgumentType,

        /// <summary>
        /// Indicates that the command does not return a result when one is expected.
        /// </summary>
        CommandHasNoReturnResult,

        EmptyInput,
        CommandNotValid,
        ArgumentError,
        Exception
    }
}
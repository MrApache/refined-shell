namespace RefinedShell.Commands
{
    /// <summary>
    /// Represents a command with read only properties
    /// </summary>
    public interface IReadOnlyCommand
    {
        /// <summary>
        /// Name of the command
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Array of parameter information necessary for executing the command.
        /// </summary>
        public Arguments Arguments { get; }

        /// <summary>
        /// Indicates whether the command produces a return value upon execution.
        /// </summary>
        public bool ReturnsResult { get; }
    }
}
using System;

namespace RefinedShell.Commands
{
    /// <summary>
    /// Represents a command that can be executed in a shell
    /// </summary>
    public interface ICommand : IReadOnlyCommand, IDisposable
    {
        /// <summary>
        /// Executes the command using the specified arguments.
        /// </summary>
        /// <param name="args">An array of objects representing the arguments for the command.</param>
        /// <returns>The result of the command execution.</returns>
        public ExecutionResult Execute(object?[] args);
        
        /// <summary>
        /// Validates whether the command can be executed with the provided arguments.
        /// </summary>
        /// <returns><c>bool</c> result</returns>
        public bool IsValid();
    }
}
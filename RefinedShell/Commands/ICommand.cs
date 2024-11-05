using System;
using System.Reflection;

namespace RefinedShell
{
    /// <summary>
    /// Represents a command that can be executed in a shell
    /// </summary>
    public interface ICommand : IDisposable
    {
        /// <summary>
        /// Name of the command
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Array of parameter information necessary for executing the command.
        /// </summary>
        public ParameterInfo[] Arguments { get; }

        /// <summary>
        /// Indicates whether the command produces a return value upon execution.
        /// </summary>
        public bool ReturnsResult { get; }

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
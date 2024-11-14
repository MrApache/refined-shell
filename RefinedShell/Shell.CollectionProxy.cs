using System;
using System.Collections.Generic;
using System.Linq;
using RefinedShell.Commands;
using RefinedShell.Utilities;

namespace RefinedShell
{
    public sealed partial class Shell
    {
        /// <summary>
        /// Gets the number of commands registered in the shell.
        /// </summary>
        public int Count => _commands.Count;

        /// <summary>
        /// Retrieves a command by its name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <returns><see cref="ICommand"/> instance if found; otherwise, <c>null</c>.</returns>
        public ICommand? GetCommand(StringToken name)
        {
            return _commands.Contains(name) ? _commands[name] : null;
        }

        /// <summary>
        /// Gets commands that match the specified predicate.
        /// </summary>
        /// <param name="func">The predicate to filter commands.</param>
        /// <returns>An <see cref="IEnumerable{ICommand}"/> of matching commands.</returns>
        public IEnumerable<ICommand> GetCommands(Func<ICommand, bool> func)
        {
            return _commands.Where(func);
        }

        /// <summary>
        /// Retrieves all registered commands.
        /// </summary>
        /// <returns>A read-only collection of all commands.</returns>
        public IReadOnlyCollection<ICommand> GetAllCommands()
        {
            return _commands;
        }
    }
}
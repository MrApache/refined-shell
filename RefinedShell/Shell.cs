using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RefinedShell.Commands;
using RefinedShell.Execution;
using RefinedShell.Interpreter;
using RefinedShell.Utilities;

namespace RefinedShell
{
    /// <summary>
    /// Represents a shell that can execute commands.
    /// </summary>
    public sealed partial class Shell
    {
        private readonly Dictionary<StringToken, StringToken> _aliases;
        private readonly Dictionary<StringToken, ExecutableExpression> _cache;
        private readonly CommandCollection _commands;
        private readonly Parser _parser;
        private readonly SemanticAnalyzer _analyzer;
        private readonly Compiler _compiler;
        private readonly IExecutor _executor;

        /// <summary>
        /// Gets the number of commands registered in the shell.
        /// </summary>
        public int Count => _commands.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shell"/> class.
        /// </summary>
        /// <param name="catchExceptions">Indicates whether to catch exceptions during command execution.</param>
        public Shell(bool catchExceptions = true)
        {
            _aliases = new Dictionary<StringToken, StringToken>(32);
            _cache = new Dictionary<StringToken, ExecutableExpression>(32);
            _commands = new CommandCollection();
            _parser = new Parser();
            _analyzer = new SemanticAnalyzer(_commands);
            _compiler = new Compiler(_commands);
            _executor = catchExceptions ? (IExecutor)new SafeExecutor() : new UnsafeExecutor();
        }

        /// <summary>
        /// Executes the specified input as a command.
        /// </summary>
        /// <param name="input">The command input to execute.</param>
        /// <returns>An <see cref="ExecutionResult"/> indicating the result of the execution.</returns>
        public ExecutionResult Execute(StringToken input)
        {
            if (input.IsEmpty) {
                return new ExecutionResult(false, null, ProblemSegment.None);
            }

            if(_aliases.TryGetValue(input, out StringToken alias)) {
                input = alias;
            }

            if (_cache.TryGetValue(input, out ExecutableExpression compiledExpression)) {
                return _executor.Execute(compiledExpression);
            }

            (Expression? expression, ProblemSegment error) = AnalyzeInternal(input);
            if (error != ProblemSegment.None) {
                return new ExecutionResult(false, null, error);
            }

            if(expression!.Count == 0)
                return new ExecutionResult(false, null, ProblemSegment.None);

            compiledExpression = _compiler.Compile(expression);
            _cache[input] = compiledExpression;

            return _executor.Execute(compiledExpression);
        }

        public ProblemSegment Analyze(StringToken input)
        {
            ValueTuple<Expression?, ProblemSegment> tuple = AnalyzeInternal(input);
            return tuple.Item2;
        }

        //Maybe add logs
        //Rewrite docs
        /// <summary>
        /// Registers all methods with <see cref="ShellCommandAttribute"/> in the specified source.
        /// </summary>
        /// <typeparam name="T">The type containing the methods to register.</typeparam>
        /// <param name="target">An instance of the type to use, or <c>null</c> for static methods.</param>
        public void RegisterAllWithAttribute<T>(T? target) where T : class
        {
            foreach (MemberInfo member in GetMembers<T>(target != null))
            {
                RegisterMemberInternal(member, target, member.GetName());
            }
        }

        /// <summary>
        /// Unregisters all methods with <see cref="ShellCommandAttribute"/> in the specified source.
        /// </summary>
        /// <typeparam name="T">The type containing the methods to unregister.</typeparam>
        /// <param name="target">An instance of the type to use, or <c>null</c> for static methods.</param>
        public void UnregisterAllWithAttribute<T>(T? target) where T : class
        {
            foreach (MemberInfo member in GetMembers<T>(target != null))
            {
                Unregister(member.GetName());
            }
        }

        /// <summary>
        /// Registers a single command with the specified name.
        /// </summary>
        /// <param name="d">The command delegate.</param>
        /// <param name="name">The name of the command.</param>
        public void Register(Delegate d, StringToken name)
        {
            if (name.IsEmpty)
                return;
            RegisterMethodInternal(d.Method, d.Target, name);
        }

        /// <summary>
        /// Unregisters a command with the specified name.
        /// </summary>
        /// <param name="name">The name of the command</param>
        public bool Unregister(StringToken name)
        {
            if (!_commands.Contains(name))
                return false;
            bool result = _commands.Remove(name, out ICommand command);
            command.Dispose();
            return result;
        }

        public void RegisterMember<T>(string memberName, string? commandName, T? target) where T : class
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            if (target != null)
                flags |= BindingFlags.Instance;

            MemberInfo[] members = typeof(T).GetMember(memberName, flags);
            if (members.Length == 0)
                throw new Exception($"Member '{memberName}' not found");
            RegisterMemberInternal(members[0], target, commandName ?? members[0].GetName());
        }

        /// <summary>
        /// Unregister all commands
        /// </summary>
        public void UnregisterAll()
        {
            foreach (ICommand command in _commands) {
                command.Dispose();
            }
            _cache.Clear();
            _commands.Clear();
        }

        /// <summary>
        /// Creates an alias for a command.
        /// </summary>
        /// <param name="alias">Name</param>
        /// <param name="input">Command</param>
        public void CreateAlias(StringToken alias, StringToken input)
        {
            if (alias.IsEmpty || input.IsEmpty)
                return;
            _aliases[alias] = input;
        }

        public void DeleteAlias(StringToken alias)
        {
            _aliases.Remove(alias);
        }

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

        /// <summary>
        /// Removes all invalid commands from the command collection.
        /// </summary>
        /// <remarks>
        /// This method iterates through the current list of commands and identifies those that are not valid
        /// by calling the <see cref="ICommand.IsValid"/> method. Each invalid command is then removed from the
        /// collection and disposed of properly to release any associated resources.
        /// </remarks>
        /// <example>
        /// <code>
        /// var commandCollection = new CommandCollection();
        /// commandCollection.RemoveNotValidCommands();
        /// </code>
        /// </example>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the command collection is modified during the enumeration process.
        /// </exception>
        public void RemoveNotValidCommands()
        {
            List<ICommand> notValidCommands = _commands.Where(c => !c.IsValid()).ToList();
            foreach (ICommand command in notValidCommands)
            {
                _commands.Remove(command);
                command.Dispose();
            }
        }
    }
}
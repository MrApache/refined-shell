using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RefinedShell.Commands;
using RefinedShell.Execution;
using RefinedShell.Parsing;
using RefinedShell.Utilities;

namespace RefinedShell
{
    /// <summary>
    /// Represents a shell that can execute commands.
    /// </summary>
    public sealed partial class Shell
    {
        private readonly ParserLibrary _parsers;
        private readonly CommandCollection _commands;

        private readonly Compiler _compiler;
        private readonly IExecutor _executor;

        private readonly Dictionary<StringToken, StringToken> _aliases;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shell"/> class.
        /// </summary>
        public Shell(bool catchExceptions = true, ParserLibrary? parsers = null)
        {
            _parsers = parsers ?? ParserLibrary.Default;
            _commands = new CommandCollection();
            _compiler = new Compiler(_commands, _parsers);
            _executor = catchExceptions ? (IExecutor)new SafeExecutor() : new UnsafeExecutor();
            _aliases = new Dictionary<StringToken, StringToken>(32);
        }

        /// <summary>
        /// Executes the specified input as a command.
        /// </summary>
        /// <param name="input">The command input to execute.</param>
        /// <returns>An <see cref="ExecutionResult"/> indicating the result of the execution.</returns>
        public ExecutionResult Execute(StringToken input)
        {
            if (input.IsEmpty) {
                return ExecutionResult.Empty;
            }

            if(_aliases.TryGetValue(input, out StringToken alias)) {
                input = alias;
            }

            ValueTuple<ExecutableExpression?, ProblemSegment> result = _compiler.Compile(input);
            if (result.Item1 == null)
                return ExecutionResult.Error(result.Item2);

            return _executor.Execute(result.Item1);
        }

        public ProblemSegment Analyze(StringToken input)
        {
            return _compiler.Analyze(input).problem;
        }

        //Maybe add logs
        //Rewrite docs
        /// <summary>
        /// Registers all methods with <see cref="ShellCommandAttribute"/> in the specified source.
        /// </summary>
        public void RegisterAllWithAttribute<T>(T? target, bool includeAll = true) where T : class
        {
            Type targetType = includeAll && target != null ? target.GetType() : typeof(T);
            foreach (MemberInfo member in GetMembers(targetType, target != null, includeAll)) {
                RegisterMemberInternal(member, target, member.GetName());
            }
        }

        /// <summary>
        /// Unregisters all methods with <see cref="ShellCommandAttribute"/> in the specified source.
        /// </summary>
        public void UnregisterAllWithAttribute<T>(T? target, bool includeAll = true) where T : class
        {
            Type targetType = includeAll && target != null ? target.GetType() : typeof(T);
            foreach (MemberInfo member in GetMembers(targetType, target != null, includeAll)) {
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
            _compiler.ClearCache();
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
        /// Removes all invalid commands from the command collection.
        /// </summary>
        /// <remarks>
        /// This method iterates through the current list of commands and identifies those that are not valid
        /// by calling the <see cref="ICommand.IsValid"/> method. Each invalid command is then removed from the
        /// collection and disposed of properly to release any associated resources.
        /// </remarks>
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
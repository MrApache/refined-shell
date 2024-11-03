using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RefinedShell.Execution;
using RefinedShell.Interpreter;
using RefinedShell.Utilities;

namespace RefinedShell
{
    /// <summary>
    /// Represents a shell that can execute commands.
    /// </summary>
    public sealed class Shell
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
            _aliases = new Dictionary<StringToken, StringToken>();
            _cache = new Dictionary<StringToken, ExecutableExpression>();
            _commands = new CommandCollection();
            _parser = new Parser();
            _analyzer = new SemanticAnalyzer(_commands);
            _compiler = new Compiler(_commands);
            _executor = catchExceptions ? (IExecutor)new SafeExecutor() : new UnsafeExecutor();
        }

        internal SemanticError Analyze(string input)
        {
            Expression expr = _parser.GetExpression(input);
            return _analyzer.Analyze(expr);
        }

        internal bool HasErrors(string input)
        {
            Expression expr = _parser.GetExpression(input);
            return _analyzer.HasErrors(expr);
        }

        /// <summary>
        /// Executes the specified input as a command.
        /// </summary>
        /// <param name="input">The command input to execute.</param>
        /// <returns>An <see cref="ExecutionResult"/> indicating the result of the execution.</returns>
        public ExecutionResult Execute(StringToken input)
        {
            if(_aliases.TryGetValue(input, out StringToken alias))
                input = alias;

            if (_cache.TryGetValue(input, out ExecutableExpression compiledExpression))
                return _executor.Execute(compiledExpression);

            Expression expr = _parser.GetExpression(input);
            if (_analyzer.Analyze(expr) != SemanticError.NoErrors)
                return ExecutionResult.Fail;
            compiledExpression = _compiler.Compile(expr);
            _cache[input] = compiledExpression;
            return _executor.Execute(compiledExpression);
        }

        //Maybe add logs
        /// <summary>
        /// Registers all methods with <see cref="ShellCommandAttribute"/> in the specified source as shell commands.
        /// </summary>
        /// <typeparam name="T">The type containing the methods to register.</typeparam>
        /// <param name="source">An instance of the type to use, or <c>null</c> for static methods.</param>
        public void RegisterAll<T>(T? source) where T : class
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            if (source != null)
                flags |= BindingFlags.Instance;

            IEnumerable<MethodInfo> methods = typeof(T)
                .GetMethods(flags)
                .WithAttribute<ShellCommandAttribute>();

            foreach (MethodInfo method in methods)
            {
                Type type = System.Linq.Expressions.Expression.GetDelegateType(method.GetParameters()
                    .Select(arg => arg.ParameterType)
                    .Append(method.ReturnType).ToArray());
                Delegate d = method.IsStatic ?
                    method.CreateDelegate(type) :
                    method.CreateDelegate(type, source);
                string name = method.GetCustomAttribute<ShellCommandAttribute>()!.Name ?? method.Name;
                Register(d, name);
            }
        }

        /// <summary>
        /// Registers a single command with the specified name.
        /// </summary>
        /// <param name="d">The command delegate.</param>
        /// <param name="name">The name of the command.</param>
        public void Register(Delegate d, StringToken name)
        {
            ThrowIfContains(name); // Maybe replace with logs
            _commands[name] = new DelegateCommand(name, d);
        }

        /// <summary>
        /// Creates an alias for a command.
        /// </summary>
        /// <param name="alias">Name</param>
        /// <param name="input">Command</param>
        public void CreateAlias(StringToken alias, StringToken input)
        {
            _aliases[alias] = input;
        }

        private void ThrowIfContains(StringToken name)
        {
            if(_commands.Contains(name))
                throw new ArgumentException($"Command with name '{name}' is already registered.");
        }

        /// <summary>
        /// Unregisters all methods with <see cref="ShellCommandAttribute"/> in the specified source.
        /// </summary>
        /// <typeparam name="T">The type containing the methods to unregister.</typeparam>
        /// <param name="source">An instance of the type to use, or <c>null</c> for static methods.</param>
        public void UnregisterAll<T>(T? source) where T : class
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            if (source != null)
                flags |= BindingFlags.Instance;

            IEnumerable<MethodInfo> methods = typeof(T)
                .GetMethods(flags)
                .WithAttribute<ShellCommandAttribute>();

            foreach (MethodInfo method in methods)
            {
                string name = method.GetCustomAttribute<ShellCommandAttribute>()!.Name ?? method.Name;
                Unregister(name.AsMemory());
            }
        }
        
        /// <summary>
        /// Unregisters a command with the specified name.
        /// </summary>
        /// <param name="name">The name of the command</param>
        public void Unregister(StringToken name)
        {
            if (!_commands.Contains(name))
                return;
            _commands.Remove(name);
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
    }
}
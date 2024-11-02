using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RefinedShell.Execution;
using RefinedShell.Interpreter;
using RefinedShell.Utilities;

namespace RefinedShell
{
    public sealed class Shell
    {
        private readonly Dictionary<StringToken, StringToken> _aliases;
        private readonly Dictionary<StringToken, ExecutableExpression> _cache;
        private readonly CommandCollection _commands;
        private readonly Parser _parser;
        private readonly SemanticAnalyzer _analyzer;
        private readonly Compiler _compiler;
        private readonly IExecutor _executor;

        public int Count => _commands.Count;

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

        public void Register(Delegate d, StringToken name)
        {
            ThrowIfContains(name);
            _commands[name] = new DelegateCommand(name, d);
        }

        public void CreateAlias(StringToken alias, StringToken input)
        {
            _aliases[alias] = input;
        }

        private void ThrowIfContains(StringToken name)
        {
            if(_commands.Contains(name))
                throw new ArgumentException($"Command with name '{name}' is already registered.");
        }

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

        public void Unregister(StringToken name)
        {
            if (!_commands.Contains(name))
                return;
            _commands.Remove(name);
        }

        public ICommand? GetCommand(StringToken name)
        {
            return _commands.Contains(name) ? _commands[name] : null;
        }

        public IEnumerable<ICommand> GetCommands(Func<ICommand, bool> func)
        {
            return _commands.Where(func);
        }

        public IEnumerable<string> GetCommands(Func<StringToken, bool> func)
        {
            return _commands.Where(kvp => func(kvp.Name)).Select(kvp => kvp.Name);
        }
    }
}
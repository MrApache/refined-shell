using System;
using System.Collections.Generic;
using System.Reflection;
using RefinedShell.Commands;
using RefinedShell.Interpreter;
using RefinedShell.Parsing;
using RefinedShell.Utilities;

namespace RefinedShell
{
    public sealed partial class Shell
    {
        private IEnumerable<MemberInfo> GetMembers<T>(bool instance)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            if (instance) {
                flags |= BindingFlags.Instance;
            }

            return typeof(T)
                .GetMembers(flags)
                .WithAttribute<MemberInfo, ShellCommandAttribute>();
        }

        // Maybe replace with logs
        private void ThrowIfContains(StringToken name)
        {
            if(_commands.Contains(name)) {
                throw new ArgumentException($"Command with name '{name}' is already registered.");
            }
        }

        private void RegisterPropertyInternal(PropertyInfo property, object? target, string name)
        {
            ThrowIfContains(name);
            MethodInfo? get = property.GetMethod;
            MethodInfo? set = property.SetMethod;
            ICommand result;

            if(get != null && set != null)
            {
                result = new GetSetProperty(name, property, target);
            }
            else if (get == null)
            {
                result = new SingleAccessProperty(name, property, target, property.SetMethod);
            }
            else
            {
                result = new SingleAccessProperty(name, property, target, property.GetMethod);
            }
            _commands[result.Name] = result;
        }

        private void RegisterMethodInternal(MethodInfo method, object? target, StringToken name)
        {
            ThrowIfContains(name);
            ParameterInfo[] parameters = method.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                if (!TypeParsers.Contains(parameter.ParameterType))
                    throw new Exception($"Parser for type '{parameter.ParameterType}' not found");
            }

            _commands[name] = new DelegateCommand(name, method, target);
        }

        private void RegisterMemberInternal(MemberInfo member, object? target, string name)
        {
            switch (member)
            {
                case MethodInfo method:
                {
                    RegisterMethodInternal(method, target, name);
                    break;
                }
                case PropertyInfo property:
                {
                    RegisterPropertyInternal(property, target, name);
                    break;
                }
            }
        }

        private (Expression? expression, ProblemSegment problem) AnalyzeInternal(StringToken input)
        {
            Expression? expression;
            ProblemSegment segment;
            try
            {
                expression = _parser.GetExpression(input);
                segment = _analyzer.Analyze(expression);
            }
            catch (InterpreterException e)
            {
                expression = null;
                segment = new ProblemSegment(e.Token.Start, e.Token.Length, e.Error);
            }
            return new ValueTuple<Expression?, ProblemSegment>(expression, segment);
        }
    }
}
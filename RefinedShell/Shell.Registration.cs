using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using RefinedShell.Commands;
using RefinedShell.Utilities;

namespace RefinedShell
{
    public sealed partial class Shell
    {
        private static IEnumerable<MemberInfo> GetMembers(Type type, bool instance, bool includeAll)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            if (instance) {
                flags |= BindingFlags.Instance;
            }

            Type? baseType = type;

            if (includeAll) {
                IEnumerable<MemberInfo> collection = Enumerable.Empty<MemberInfo>();
                while (baseType != null) {
                    collection = collection.Concat(baseType.GetMembers(flags)
                        .WithAttribute<MemberInfo, ShellFunctionAttribute>());
                    baseType = baseType.BaseType;
                }

                return collection;
            }

            return type
                .GetMembers(flags)
                .WithAttribute<MemberInfo, ShellFunctionAttribute>();
        }

        // Maybe replace with logs
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfContains(StringToken name)
        {
            if(_commands.Contains(name)) {
                throw new ArgumentException($"Command with name '{name}' is already registered.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfParserNotDefined(Type type)
        {
            if (!_parsers.Contains(type)) {
                throw new Exception($"Parser for type '{type}' not found");
            }
        }

        private void RegisterPropertyInternal(PropertyInfo property, object? target, string name)
        {
            ThrowIfContains(name);
            MethodInfo? get = property.GetMethod;
            MethodInfo? set = property.SetMethod;
            ICommand result;

            if(get != null && set != null) {
                ThrowIfParserNotDefined(property.PropertyType);
                result = new GetSetProperty(name, property, target);
            }
            else if (get == null) {
                ThrowIfParserNotDefined(property.PropertyType);
                result = new SingleAccessProperty(name, property, target, property.SetMethod);
            }
            else {
                result = new SingleAccessProperty(name, property, target, property.GetMethod);
            }
            AddCommandInCollection(result.Name, result);
        }

        private void RegisterMethodInternal(MethodInfo method, object? target, StringToken name)
        {
            ThrowIfContains(name);
            ParameterInfo[] parameters = method.GetParameters();
            foreach (ParameterInfo parameter in parameters) {
                ThrowIfParserNotDefined(parameter.ParameterType);
            }

            AddCommandInCollection(name, new DelegateCommand(name, method, target));
        }

        private void RegisterFieldInternal(FieldInfo field, object? target, StringToken name)
        {
            ThrowIfContains(name);
            if (!field.IsInitOnly) {
                ThrowIfParserNotDefined(field.FieldType);
            }

            AddCommandInCollection(name, new Field(name, field, target));
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
                case FieldInfo field:
                {
                    RegisterFieldInternal(field, target, name);
                    break;
                }
            }
        }

        internal CommandCollection GetCommandCollection() => _commands;

        private void AddCommandInCollection(StringToken name, ICommand command)
        {
            _commands[name] = command;
            InvokeOnCommandRegisteredEvent(command);
        }
    }
}

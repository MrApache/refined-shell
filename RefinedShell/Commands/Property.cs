using System;
using System.Reflection;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal abstract class Property : ICommand
    {
        public string Name => _name;
        public Arguments Arguments => _arguments;
        public bool ReturnsResult => _returnResult;

        private readonly string _name;
        private readonly bool _returnResult;
        private readonly Arguments _arguments;
        private readonly WeakReference? _target;
        private readonly bool _isStatic;

        private bool _disposed;

        protected Property(StringToken name, PropertyInfo p, object? target)
        {
            _name = name.ToString();
            _returnResult = p.GetMethod != null;
            if (p.SetMethod != null)
            {
                bool optional = p.GetMethod != null;
                ParameterInfo[] parameters = p.SetMethod.GetParameters();
                Argument[] arguments = new Argument[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo parameter = parameters[i];
                    arguments[i] = new Argument(parameter.ParameterType, parameter.Name, optional);
                }

                _arguments = new Arguments(arguments);
            }
            else
            {
                _arguments = new Arguments(Array.Empty<Argument>());
            }

            if(!(_isStatic = p.IsStatic())) {
                _target = new WeakReference(target);
            }
        }

        public abstract ExecutionResult Execute(object?[] args);

        public bool IsValid()
        {
            return !_disposed && ((_target != null && _target.IsAlive) || _isStatic);
        }

        public void Dispose()
        {
            if(_target != null)
                _target.Target = null;
            _disposed = true;
        }

        protected object? GetTarget()
        {
            return _target?.Target;
        }
    }
}
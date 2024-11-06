using System;
using System.Reflection;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal abstract class Property : ICommand
    {
        public string Name => _name;
        public ParameterInfo[] Arguments => _arguments;
        public bool ReturnsResult => _returnResult;

        private readonly string _name;
        private readonly bool _returnResult;
        private readonly ParameterInfo[] _arguments;
        private readonly WeakReference? _target;
        private readonly bool _isStatic;

        private bool _disposed;

        protected Property(StringToken name, PropertyInfo p, object? target)
        {
            _name = name.ToString();
            _returnResult = p.GetMethod != null;
            _arguments = p.SetMethod == null ? Array.Empty<ParameterInfo>() : p.SetMethod.GetParameters();
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
using System;
using System.Reflection;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal abstract class MemberCommand : ICommand
    {
        public string Name => _name;

        public abstract Arguments Arguments { get; }
        public abstract bool ReturnsResult { get; }

        private readonly WeakReference? _target;
        private readonly string _name;
        private readonly bool _isStatic;
        private bool _disposed;

        protected MemberCommand(StringToken name, MemberInfo memberInfo, object? target)
        {
            _name = name.ToString();
            if(!(_isStatic = memberInfo.IsStatic())) {
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
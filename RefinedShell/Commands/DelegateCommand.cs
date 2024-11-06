using System;
using System.Reflection;
using RefinedShell.Execution;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal sealed class DelegateCommand : ICommand
    {
        public string Name => _name;
        public ParameterInfo[] Arguments => _arguments;
        public bool ReturnsResult => _returnsResult;

        private readonly string _name;
        private readonly bool _returnsResult;
        private readonly ParameterInfo[] _arguments;

        private readonly MethodInfo _method;
        private readonly WeakReference? _target;

        private bool _disposed;

        internal DelegateCommand(StringToken name, MethodInfo method, object? target)
        {
            _name = name.ToString();
            _arguments = method.GetParameters();
            _method = method;
            if(!_method.IsStatic) {
                _target = new WeakReference(target);
            }
            _returnsResult = _method.ReturnType != typeof(void);
        }

        public ExecutionResult Execute(object?[] args)
        {
            if (!IsValid()) {
                return new ExecutionResult(false, 0, 0, ExecutionError.CommandNotValid, null);
            }
            object? returnValue = _method.Invoke(GetTarget(), args);
            return new ExecutionResult(true, 0, 0, ExecutionError.None, returnValue);
        }

        private object? GetTarget()
        {
            return _method.IsStatic ? null : _target!.Target;
        }

        public bool IsValid()
        {
            return !_disposed && ((_target != null && _target.IsAlive) || _method.IsStatic);
        }

        public void Dispose()
        {
            if(_target != null)
                _target.Target = null;
            _disposed = true;
        }
    }
}
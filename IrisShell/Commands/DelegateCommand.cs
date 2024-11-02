using System;
using System.Reflection;
using IrisShell.Interpreter;
using IrisShell.Utilities;

namespace IrisShell
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

        internal DelegateCommand(StringToken name, Delegate d)
        {
            _name = name.ToString();
            _arguments = d.Method.GetParameters();
            _method = d.Method;
            if(!_method.IsStatic)
                _target = new WeakReference(d);
            _returnsResult = _method.ReturnType != typeof(void);
        }

        public ExecutionResult Execute(object[] args)
        {
            if (!IsValid()) return ExecutionResult.Fail;
            object? returnValue = _method.Invoke(GetTarget(), args);
            return new ExecutionResult(true, returnValue);
        }

        private object? GetTarget()
        {
            return _method.IsStatic ? null : _target!.Target;
        }

        public bool IsValid()
        {
            return (_target != null && _target.IsAlive) || _method.IsStatic;
        }
    }
}
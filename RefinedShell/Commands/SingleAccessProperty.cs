using System.Reflection;
using RefinedShell.Execution;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal sealed class SingleAccessProperty : Property
    {
        private readonly MethodInfo _method;

        public SingleAccessProperty(StringToken name, PropertyInfo p, object? target, MethodInfo accessMethod)
            : base(name, p, target)
        {
            _method = accessMethod;
        }

        public override ExecutionResult Execute(object?[] args)
        {
            if (!IsValid()) {
                return new ExecutionResult(false, 0, 0, ExecutionError.CommandNotValid, null);
            }
            object? returnValue = _method.Invoke(GetTarget(), args);
            return new ExecutionResult(true, 0, 0, ExecutionError.None, returnValue);
        }
    }
}
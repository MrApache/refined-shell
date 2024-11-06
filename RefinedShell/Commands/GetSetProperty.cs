using System;
using System.Reflection;
using RefinedShell.Execution;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal sealed class GetSetProperty : Property
    {
        private readonly MethodInfo _get;
        private readonly MethodInfo _set;

        public GetSetProperty(StringToken name, PropertyInfo p, object? target) : base(name, p, target)
        {
            _get = p.GetMethod;
            _set = p.SetMethod;
        }

        public override ExecutionResult Execute(object?[] args)
        {
            if (!IsValid()) {
                return new ExecutionResult(false, 0, 0, ExecutionError.CommandNotValid, null);
            }

            if(args.Length != 0 && args[0] != Type.Missing)
            {
                ExecutionResult setResult = Set(args);
                if(!setResult.Success) {
                    return setResult;
                }
            }

            return Get();
        }

        private ExecutionResult Get()
        {
            object? returnValue = _get.Invoke(GetTarget(), null);
            return new ExecutionResult(true, 0, 0, ExecutionError.None, returnValue);
        }

        private ExecutionResult Set(object?[] args)
        {
            _set.Invoke(GetTarget(), args);
            return new ExecutionResult(true, 0, 0, ExecutionError.None, null);
        }
    }
}
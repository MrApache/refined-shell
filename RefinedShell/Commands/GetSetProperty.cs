using System;
using System.Reflection;
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
                return ExecutionResult.Error(ProblemSegment.CommandNotValid);
            }

            if(args.Length != 0 && args[0] != Type.Missing) {
                _set.Invoke(GetTarget(), args);
            }

            object? returnValue = _get.Invoke(GetTarget(), null);
            return ExecutionResult.Success(returnValue);
        }
    }
}
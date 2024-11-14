using System;
using System.Reflection;
using RefinedShell.Execution;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal sealed class Field : MemberCommand
    {
        public override bool ReturnsResult => _returnsResult;
        public override Arguments Arguments => _arguments;

        private readonly bool _returnsResult;
        private readonly Arguments _arguments;
        private readonly FieldInfo _field;

        public Field(StringToken name, FieldInfo field, object? target) : base(name, field, target)
        {
            _field = field;
            _returnsResult = true;
            _arguments = !field.IsInitOnly
                ? new Arguments(new Argument(field.FieldType, field.Name, true))
                : new Arguments();
        }

        public override ExecutionResult Execute(object?[] args)
        {
            if (!IsValid()) {
                return ExecutionResult.Error(new ProblemSegment(0, 0, ExecutionError.CommandNotValid));
            }

            if(args.Length != 0 && args[0] != Type.Missing && !_field.IsInitOnly) {
                _field.SetValue(GetTarget(), args[0]);
            }

            object? returnValue = _field.GetValue(GetTarget());
            return ExecutionResult.Success(returnValue);
        }
    }
}

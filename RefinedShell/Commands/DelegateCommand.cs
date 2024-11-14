using System.Reflection;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal sealed class DelegateCommand : MemberCommand
    {
        public override Arguments Arguments => _arguments;
        public override bool ReturnsResult => _returnsResult;

        private readonly bool _returnsResult;
        private readonly Arguments _arguments;
        private readonly MethodInfo _method;

        public DelegateCommand(StringToken name, MethodInfo method, object? target)
            : base(name, method, target)
        {
            _arguments = new Arguments(method.GetParameters().ToArguments());
            _returnsResult = method.ReturnType != typeof(void);
            _method = method;
        }

        public override ExecutionResult Execute(object?[] args)
        {
            if (!IsValid()) {
                return ExecutionResult.Error(ProblemSegment.CommandNotValid);
            }
            object? returnValue = _method.Invoke(GetTarget(), args);
            return ExecutionResult.Success(returnValue);
        }
    }
}
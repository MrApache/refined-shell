using RefinedShell.Interpreter;

namespace RefinedShell.Execution
{
    internal sealed class ExecutableCommand : ExecutableExpression
    {
        public readonly ICommand Command;
        private readonly IArgument[] _arguments;
        private readonly object?[] _pool;

        private readonly int _start;
        private readonly int _length;

        public ExecutableCommand(ICommand command, IArgument[] arguments, Token token)
        {
            Command = command;
            _arguments = arguments;
            _pool = new object[arguments.Length];
            _start = token.Start;
            _length = token.Length;
        }

        public override ExecutionResult Execute()
        {
            if (!Command.IsValid())
                return new ExecutionResult(false, _start, _length, ExecutionError.CommandNotValid, null);

            for (int i = 0; i < _arguments.Length; i++)
            {
                IArgument argument = _arguments[i];
                if (!argument.CanGetValue())
                    return new ExecutionResult(false, 0, 0, ExecutionError.ArgumentError, null);
                _pool[i] = argument.GetValue();
            }

            return Command.Execute(_pool);
        }
    }
}
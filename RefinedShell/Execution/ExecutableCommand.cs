namespace RefinedShell.Execution
{
    internal sealed class ExecutableCommand : ExecutableExpression
    {
        public readonly ICommand Command;
        private readonly IArgument[] _arguments;
        private readonly object[] _pool;

        public ExecutableCommand(ICommand command, IArgument[] arguments)
        {
            Command = command;
            _arguments = arguments;
            _pool = new object[arguments.Length];
        }

        public override ExecutionResult Execute()
        {
            if (!Command.IsValid())
                return new ExecutionResult(false, 0, 0, ExecutionError.CommandNotValid, null);

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
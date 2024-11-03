using System;
using System.Linq;

namespace RefinedShell.Execution
{
    internal sealed class ExecutableCommandSequence : ExecutableExpression
    {
        private readonly ExecutableCommand[] _commands;
        private readonly ExecutionResult[] _resultPool;

        public ExecutableCommandSequence(ExecutableCommand[] commands)
        {
            _commands = commands;
            int poolSize = _commands.Count(command => command.Command.ReturnsResult);
            _resultPool = poolSize == 0 ? Array.Empty<ExecutionResult>() : new ExecutionResult[poolSize];
        }

        public override ExecutionResult Execute()
        {
            int poolPtr = 0;
            bool success = true;
            foreach (ExecutableCommand command in _commands)
            {
                ExecutionResult result = command.Execute();
                if(command.Command.ReturnsResult)
                {
                    _resultPool[poolPtr++] = result;
                    success &= result.Success;
                }
            }

            return new ExecutionResult(success, 0, 0, ExecutionError.None, _resultPool);
        }
    }
}
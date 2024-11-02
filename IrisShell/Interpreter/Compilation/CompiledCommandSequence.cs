using System;
using System.Linq;

namespace IrisShell.Interpreter
{
    internal sealed class CompiledCommandSequence : CompiledExpression
    {
        private readonly CompiledCommand[] _commands;
        private readonly ExecutionResult[] _resultPool;

        public CompiledCommandSequence(CompiledCommand[] commands)
        {
            _commands = commands;
            int poolSize = _commands.Count(command => command.Command.ReturnsResult);
            _resultPool = poolSize == 0 ? Array.Empty<ExecutionResult>() : new ExecutionResult[poolSize];
        }

        public override ExecutionResult Execute()
        {
            int poolPtr = 0;
            bool success = true;
            foreach (CompiledCommand command in _commands)
            {
                ExecutionResult result = command.Execute();
                if(command.Command.ReturnsResult)
                {
                    _resultPool[poolPtr++] = result;
                    success &= result.Success;
                }
            }

            return new ExecutionResult(success, _resultPool);
        }
    }
}
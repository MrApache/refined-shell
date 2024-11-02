namespace IrisShell.Interpreter
{
    internal abstract class CompiledExpression
    {
        public abstract ExecutionResult Execute();
    }
}
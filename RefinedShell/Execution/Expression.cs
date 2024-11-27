using RefinedShell.Commands;

namespace RefinedShell.Execution
{
    internal sealed class Expression
    {
        public readonly ExecutableExpression Tree;
        public readonly IReadOnlyCommand[] Commands;

        public Expression(ExecutableExpression tree, IReadOnlyCommand[] commands)
        {
            Tree = tree;
            Commands = commands;
        }
    }
}

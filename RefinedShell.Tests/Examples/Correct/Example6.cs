using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example6 : IExample
{
    public string Input => "";
    public Expression Expression => new Expression();
    public InterpreterException? Exception => null;
    public List<(string, TokenType)> Tokens => [];

    public ExecutionResult ExecutionResult => new ExecutionResult(false, null, ProblemSegment.None);

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
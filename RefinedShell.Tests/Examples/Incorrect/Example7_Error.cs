using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example7_Error : IExample
{
    public string Input => ";";
    public Expression Expression => new Expression();
    public InterpreterException? Exception => null;
    public List<(string, TokenType)> Tokens =>
    [
        (";", TokenType.Semicolon)
    ];

    public ExecutionResult ExecutionResult => new ExecutionResult(false, null, ProblemSegment.None);

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
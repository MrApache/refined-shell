using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example22_Error : IExample
{
    public string Input => "command arg1, arg2";
    public Expression? Expression => null;

    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnknownToken, new Token(12, 2, TokenType.Unknown));

    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        (", ", TokenType.Unknown),
        ("arg2", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(12, 2, ExecutionError.UnknownToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
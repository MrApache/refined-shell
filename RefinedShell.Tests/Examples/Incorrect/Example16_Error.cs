using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example16_Error : IExample
{
    public string Input => "-command arg1 arg2";

    public Expression? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnknownToken, new Token(0, 1, TokenType.Unknown));

    public List<(string, TokenType)> Tokens =>
    [
        ("-", TokenType.Unknown),
        ("command", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(0, 1, ExecutionError.UnknownToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
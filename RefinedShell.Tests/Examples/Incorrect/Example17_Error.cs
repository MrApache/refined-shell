using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example17_Error : IExample
{
    public string Input => "-command arg@1 arg@2";
    public Expression? Expression => null;

    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnknownToken, new Token(0, 1, TokenType.Unknown));

    public List<(string, TokenType)> Tokens =>
    [
        ("-", TokenType.Unknown),
        ("command", TokenType.Identifier),
        ("arg", TokenType.Identifier),
        ("@", TokenType.Unknown),
        ("1", TokenType.Number),
        ("arg", TokenType.Identifier),
        ("@", TokenType.Unknown),
        ("2", TokenType.Number)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(0, 1, ExecutionError.UnknownToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
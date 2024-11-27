using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase12_Error : ITestCase
{
    public string Input => "-command arg1 arg2";

    public Node? Expression => null;
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
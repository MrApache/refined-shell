using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase22_Error : ITestCase
{
    public string Input => "print \"message \" \" ";
    public Node? Expression => null;

    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnknownToken, new Token(17, 2, TokenType.Unknown));

    public List<(string, TokenType)> Tokens =>
    [
        ("print", TokenType.Identifier),
        ("\"message \"", TokenType.String),
        ("\" ", TokenType.Unknown)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(17, 2, ExecutionError.UnknownToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
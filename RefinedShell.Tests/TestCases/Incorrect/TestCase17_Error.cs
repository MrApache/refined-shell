using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase17_Error : ITestCase
{
    public string Input => "command arg1 arg2)";
    public Node? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnexpectedToken, new Token(17, 1, TokenType.CloseParenthesis));
    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(17, 1, ExecutionError.UnexpectedToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
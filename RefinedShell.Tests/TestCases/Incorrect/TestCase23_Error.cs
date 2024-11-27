using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase23_Error : ITestCase
{
    public string Input => "command $ teleport, arg1 arg2 , $(getpos)";
    public Node? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnexpectedToken, new Token(10, 8, TokenType.Identifier));

    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("teleport", TokenType.Identifier),
        (", ", TokenType.Unknown),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier),
        (", ", TokenType.Unknown),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("getpos", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(10, 8, ExecutionError.UnexpectedToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
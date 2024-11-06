using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example11_Error : IExample
{
    public string Input => "teleport $(getplayerpos arg1 arg2 arg3) $(123arg2)";
    public Expression? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnexpectedToken, new Token(42, 3, TokenType.Number));
    public List<(string, TokenType)> Tokens =>
    [
        ("teleport", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("getplayerpos", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier),
        ("arg3", TokenType.Identifier),
        (")", TokenType.CloseParenthesis),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("123", TokenType.Number), //Error
        ("arg2", TokenType.Identifier), //Error
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(42, 3, ExecutionError.UnexpectedToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
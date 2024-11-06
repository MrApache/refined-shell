using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example27_Error : IExample
{
    public string Input => "command; teleport $(getplayerpos 1 2 $(spawn_position))) $(get_first_player)";
    public Expression? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.InvalidUsageOfToken, new Token(55, 1, TokenType.CloseParenthesis));
    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        (";", TokenType.Semicolon),
        ("teleport", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("getplayerpos", TokenType.Identifier),
        ("1", TokenType.Number),
        ("2", TokenType.Number),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("spawn_position", TokenType.Identifier),
        (")", TokenType.CloseParenthesis),
        (")", TokenType.CloseParenthesis),
        (")", TokenType.CloseParenthesis),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("get_first_player", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(55, 1, ExecutionError.InvalidUsageOfToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}
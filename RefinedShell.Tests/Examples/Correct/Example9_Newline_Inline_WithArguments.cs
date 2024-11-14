using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example9_Newline_Inline_WithArguments : StaticCommandsExample
{
    public override string Input => "$(command arg1 arg2)";

    public override Expression Expression =>
        new Expression(
            new CommandNode(new Token(2, 7, TokenType.Identifier), "command",
            [
                new ArgumentNode(new Token(9, 4, TokenType.Identifier), "arg1"),
                new ArgumentNode(new Token(13, 4, TokenType.Identifier), "arg2")
            ], true));

    public override List<(string, TokenType)> Tokens =>
    [
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("command", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success("arg1arg2");

    [ShellCommand("command")]
    private static string FormatString(string arg1, string arg2)
    {
        return arg1 + arg2;
    }
}
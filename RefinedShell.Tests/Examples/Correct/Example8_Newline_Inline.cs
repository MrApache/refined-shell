using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example8_Newline_Inline : StaticCommandsExample
{
    public override string Input => "$(command)";

    public override Expression Expression =>
        new Expression(
            new CommandNode(new Token(2, 7, TokenType.Identifier), "command", [], true));

    public override List<(string, TokenType)> Tokens =>
    [
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("command", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(993);

    [ShellCommand("command")]
    private static int GetRandom()
    {
        return 1000 - 7;
    }
}
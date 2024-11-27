using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase7 : StaticCommandsTestCase
{
    public override string Input => "$(command)";

    public override Node Expression =>
        new CommandNode(new Token(2, 7, TokenType.Identifier), "command", [], true);

    public override List<(string, TokenType)> Tokens =>
    [
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("command", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(993);

    [ShellFunction("command")]
    private static int GetRandom()
    {
        return 1000 - 7;
    }
}
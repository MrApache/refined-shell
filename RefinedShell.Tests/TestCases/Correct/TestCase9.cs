using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase9 : StaticCommandsTestCase
{
    public override string Input => "command 12345 67890";

    public override Node Expression =>
        new CommandNode(new Token(0, 7, TokenType.Identifier), "command",
        [
            new ArgumentNode(new Token(8, 5, TokenType.Number), "12345"),
            new ArgumentNode(new Token(14, 5, TokenType.Number), "67890")
        ]);

    public override List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("12345", TokenType.Number),
        ("67890", TokenType.Number)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(12345 + 67890);

    [ShellFunction("command")]
    private static int Command(short value1, int value2)
    {
        return value1 + value2;
    }
}
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase1 : StaticCommandsTestCase
{
    public override string Input => "Scale 3.7";

    public override Node Expression =>
        new CommandNode(new Token(0, 5, TokenType.Identifier), "Scale",
        [
            new ArgumentNode(new Token(6, 3, TokenType.Number), "3.7")
        ]);

    public override List<(string, TokenType)> Tokens =>
    [
        ("Scale", TokenType.Identifier),
        ("3.7", TokenType.Number)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(3.7);

    [ShellFunction]
    private static double Scale;
}
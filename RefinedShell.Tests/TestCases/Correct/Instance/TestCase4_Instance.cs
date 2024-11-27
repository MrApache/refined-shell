using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase4_Instance : TestCaseInstanceGetSetProperty
{
    public override string Input => "Health 10000";

    public override Node? Expression =>
        new CommandNode(new Token(0, 6, TokenType.Identifier), "Health",
        [
            new ArgumentNode(new Token(7, 5, TokenType.Number), "10000")
        ]);

    public override List<(string, TokenType)> Tokens =>
    [
        ("Health", TokenType.Identifier),
        ("10000", TokenType.Number)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(10000);
}
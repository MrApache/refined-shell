using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase5_Instance : TestCaseInstanceGetSetProperty
{
    public override string Input => "Health";

    public override Node Expression =>
        new CommandNode(new Token(0, 6, TokenType.Identifier), "Health", []);

    public override List<(string, TokenType)> Tokens =>
    [
        ("Health", TokenType.Identifier)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(0);
}
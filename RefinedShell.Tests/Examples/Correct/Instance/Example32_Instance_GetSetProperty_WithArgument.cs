using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example32_Instance_GetSetProperty_WithArgument : Example_Instance_GetSetProperty
{
    public override string Input => "Health 10000";

    public override Expression Expression => new Expression(
        new CommandNode(new Token(0, 6, TokenType.Identifier), "Health",
        [
            new ArgumentNode(new Token(7, 5, TokenType.Number), "10000")
        ]));

    public override List<(string, TokenType)> Tokens =>
    [
        ("Health", TokenType.Identifier),
        ("10000", TokenType.Number)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(10000);
}
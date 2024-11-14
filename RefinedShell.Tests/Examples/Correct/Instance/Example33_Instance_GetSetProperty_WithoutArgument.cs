using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example33_Instance_GetSetProperty_WithoutArgument : Example_Instance_GetSetProperty
{
    public override string Input => "Health";

    public override Expression Expression => new Expression(
        new CommandNode(new Token(0, 6, TokenType.Identifier), "Health", []));

    public override List<(string, TokenType)> Tokens =>
    [
        ("Health", TokenType.Identifier)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(0);
}
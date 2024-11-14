using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example13_NewlineSequence : StaticCommandsExample
{
    public override string Input => " ; teleport";

    public override Expression Expression =>
        new Expression(
            new CommandNode(new Token(3, 8, TokenType.Identifier),
                "teleport", [])
        );
    public override List<(string, TokenType)> Tokens =>
    [
        (";", TokenType.Semicolon),
        ("teleport", TokenType.Identifier)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success();

    [ShellCommand("teleport")] private static void Teleport(){}
}
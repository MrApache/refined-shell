using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example41_ReadOnlyField_Assign_Error : StaticCommandsExample
{
    public override string Input => "Code 127";

    public override Expression Expression => new Expression(
        new CommandNode(new Token(0, 4, TokenType.Identifier), "Code",
            [
                new ArgumentNode(new Token(5, 3, TokenType.Number), "127")
            ]));

    public override List<(string, TokenType)> Tokens =>
    [
        ("Code", TokenType.Identifier),
        ("127", TokenType.Number)
    ];

    public override ExecutionResult ExecutionResult =>
        ExecutionResult.Error(new ProblemSegment(5, 3, ExecutionError.TooManyArguments));

    [ShellCommand]
    private static readonly byte Code;
}
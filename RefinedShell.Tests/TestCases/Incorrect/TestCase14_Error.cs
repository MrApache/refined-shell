using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase14_Error : StaticCommandsTestCase
{
    public override string Input => " ; teleport";

    public override Node? Expression => null;

    public override InterpreterException Exception =>
        new InterpreterException(ExecutionError.InvalidUsageOfToken, new Token(1, 1, TokenType.Semicolon));

    public override List<(string, TokenType)> Tokens =>
    [
        (";", TokenType.Semicolon),
        ("teleport", TokenType.Identifier)
    ];

    public override ExecutionResult ExecutionResult =>
        ExecutionResult.Error(new ProblemSegment(1, 1, ExecutionError.InvalidUsageOfToken));

    [ShellFunction("teleport")] private static void Teleport(){}
}
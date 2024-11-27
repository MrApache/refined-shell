using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples;

internal sealed class Example31_Error : ITestCase
{
    public string Input => "tryParse 123102312321321";

    public Node? Expression =>
        new CommandNode(
            new Token(0, 8, TokenType.Identifier), "tryParse",
            [
                new ArgumentNode(new Token(9, 15, TokenType.Number), "123102312321321")
            ]);

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("tryParse", TokenType.Identifier),
        ("123102312321321", TokenType.Number)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(9, 15, ExecutionError.InvalidArgumentType));

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }

    [ShellFunction("tryParse")]
    private static void TryParse(int value = 0)
    {
    }
}
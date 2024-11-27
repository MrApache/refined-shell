using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct.Instance;

internal sealed class TestCase8_Instance : ITestCase
{
    public string Input => "Code";

    public Node Expression => new CommandNode(new Token(0, 4, TokenType.Identifier), "Code", []);

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("Code", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult => ExecutionResult.Success((byte)127);

    [ShellFunction]
    private readonly byte Code = 127;

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterMember(nameof(Code), null, this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.Unregister(nameof(Code));
    }
}
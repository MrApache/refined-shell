using System.Collections.Generic;
using JetBrains.Annotations;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase7_Instance : ITestCase
{
    public string Input => "SetID 532034123";

    public Node Expression =>
        new CommandNode(new Token(0, 5, TokenType.Identifier), "SetID",
        [
            new ArgumentNode(new Token(6, 9, TokenType.Number), "532034123")
        ]);

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("SetID", TokenType.Identifier),
        ("532034123", TokenType.Number)
    ];

    public ExecutionResult ExecutionResult => ExecutionResult.Success();

    private int ID
    {
        set => _id = value;
    }

    [UsedImplicitly]
    private int _id;

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterMember(nameof(ID), "SetID", this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.Unregister("SetID");
    }
}
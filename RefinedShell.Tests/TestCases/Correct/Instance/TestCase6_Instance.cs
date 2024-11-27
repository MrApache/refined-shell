using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples;

internal sealed class TestCase6_Instance : ITestCase
{
    public string Input => "GetID";

    public Node Expression =>
        new CommandNode(new Token(0, 5, TokenType.Identifier), "GetID", []);

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("GetID", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult => ExecutionResult.Success(GetHashCode());

    private int ID => GetHashCode();

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterMember(nameof(ID), "GetID", this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.Unregister("GetID");
    }
}
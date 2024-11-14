using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example2_Instance_StringArgument : IExample
{
    public string Input => "print \"Hello world@-42=[]]-'l.\"";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 5, TokenType.Identifier), "print",
            [
                new ArgumentNode(new Token(6, 25, TokenType.String), "Hello world@-42=[]]-'l.")
            ]));

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
        [
            ("print", TokenType.Identifier),
            ("\"Hello world@-42=[]]-'l.\"", TokenType.String)
        ];

    public ExecutionResult ExecutionResult => ExecutionResult.Success();

    private void Print(string message)
    {
        Console.WriteLine(message);
    }

    public void RegisterCommands(Shell shell)
    {
        shell.Register(Print, "print");
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.Unregister("print");
    }
}
using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example2 : IExample
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

    public ExecutionResult ExecutionResult => new ExecutionResult(true, null, ProblemSegment.None);

    [ShellCommand("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }
}
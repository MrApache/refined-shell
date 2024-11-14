using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal abstract class Example_Instance_GetSetProperty : IExample
{
    public abstract string Input { get; }

    public abstract Expression Expression { get; }

    public InterpreterException? Exception => null;

    public abstract List<(string, TokenType)> Tokens { get; }

    public abstract ExecutionResult ExecutionResult { get; }

    [ShellCommand]
    public virtual int Health { get; set; }

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterMember(nameof(Health), "Health", this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.Unregister("Health");
    }
}
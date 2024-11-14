using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples;

internal abstract class StaticCommandsExample : IExample
{
    public abstract string Input { get; }
    public abstract Expression? Expression { get; }
    public virtual InterpreterException? Exception => null;
    public abstract List<(string, TokenType)> Tokens { get; }
    public abstract ExecutionResult ExecutionResult { get; }

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }
}
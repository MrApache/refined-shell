using System;

namespace RefinedShell.Tests;

internal sealed class ClassWithCommands
{
    public ClassWithCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
        shell.Register(GetRandom, "random");
    }

    [ShellFunction]
    private void Print(string message)
    {
        Console.WriteLine(message);
    }

    private int GetRandom()
    {
        return Random.Shared.Next();
    }
}
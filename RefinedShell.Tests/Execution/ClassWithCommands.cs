using System;

namespace RefinedShell.Tests;

internal sealed class ClassWithCommands
{
    public ClassWithCommands(Shell shell)
    {
        shell.RegisterAll(this);
        shell.Register(GetRandom, "random");
    }

    [ShellCommand]
    private void Print(string message)
    {
        Console.WriteLine(message);
    }

    private int GetRandom()
    {
        return Random.Shared.Next();
    }
}
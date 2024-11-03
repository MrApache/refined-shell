using System;

namespace RefinedShell.Tests;

internal abstract class DefaultShellSetup_Static
{
    protected readonly Shell Shell;

    protected DefaultShellSetup_Static()
    {
        Shell = new Shell();
        Shell.RegisterAll<DefaultShellSetup_Static>(null);
        Shell.Register(GetRandom, "random");
    }

    [ShellCommand]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private static int GetRandom()
    {
        return Random.Shared.Next();
    }

    [ShellCommand("setValue")]
    private static void SetValue(int value) {}
}
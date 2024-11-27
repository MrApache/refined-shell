using System;

namespace RefinedShell.Tests;

internal abstract class DefaultShellSetup_Static
{
    protected readonly Shell Shell;

    protected DefaultShellSetup_Static()
    {
        Shell = new Shell();
        Shell.RegisterAllWithAttribute<DefaultShellSetup_Static>(null);
        Shell.Register(GetRandom, "random");
    }

    [ShellFunction]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private static int GetRandom()
    {
        return Random.Shared.Next();
    }

    [ShellFunction("setValue")]
    private static void SetValue(int value) {}
}
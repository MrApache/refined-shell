using System;
using NUnit.Framework;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Shell))]
internal sealed class Aliases
{
    private readonly Shell _shell;

    public Aliases()
    {
        _shell = new Shell();
        _shell.RegisterAllWithAttribute<Aliases>(null);
        _shell.CreateAlias("phw","print hello_world");
        _shell.CreateAlias("gv","getValue");
    }

    [ShellFunction("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    [ShellFunction("print_err")]
    private static void PrintError(string message)
    {
        Console.WriteLine($"[Error] {message}");
    }

    [ShellFunction("getValue")]
    private static int GetValue() => 993;

    [Test]
    public void Execute_Success()
    {
        bool result = _shell.Execute("phw").IsSuccess;
        Assert.That(result, Is.True);
    }

    [Test]
    public void Execute_Fail()
    {
        bool result = _shell.Execute("pwd").IsSuccess;
        Assert.That(result, Is.False);
    }

    [Test]
    public void Execute_Redefine()
    {
        _shell.CreateAlias("phw", "print_err file_not_found");
        bool result = _shell.Execute("phw").IsSuccess;
        Assert.That(result, Is.True);
        _shell.CreateAlias("phw","print hello_world");
    }

    [Test]
    public void Execute_CommandIsEmpty()
    {
        _shell.CreateAlias("gv", "");
        int result = (int)_shell.Execute("gv").ReturnValue!;
        Assert.That(result, Is.EqualTo(993));
    }

    [Test]
    public void Execute_AliasIsEmpty()
    {
        _shell.CreateAlias("", "getValue");
        bool result = _shell.Execute("").IsSuccess;
        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteAlias()
    {
        bool result = _shell.Execute("phw").IsSuccess;
        Assert.That(result, Is.True);
        _shell.DeleteAlias("phw");
        result = _shell.Execute("phw").IsSuccess;
        Assert.That(result, Is.False);
    }
}
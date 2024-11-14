using System;
using System.Reflection;
using NUnit.Framework;
using RefinedShell.Execution;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Shell))]
[TestOf(typeof(IExecutor))]
[TestOf(typeof(SafeExecutor))]
[TestOf(typeof(UnsafeExecutor))]
internal sealed class Executors
{
    private readonly Shell _unsafe;
    private readonly Shell _safe;

    public Executors()
    {
        _unsafe = new Shell(false);
        _safe = new Shell();
        _unsafe.RegisterAllWithAttribute<Executors>(null);
        _safe.RegisterAllWithAttribute<Executors>(null);
    }

    [ShellCommand("getPlayer")]
    private static void GetPlayer(bool throwException)
    {
        if (throwException)
            throw new ArgumentException();
    }

    [Test]
    public void Execute_Unsafe()
    {
        bool result = _unsafe.Execute("getPlayer false").IsSuccess;
        Assert.That(result, Is.True);
    }

    [Test]
    public void Execute_Unsafe_Exception()
    {
        Assert.Throws<TargetInvocationException>(() => _unsafe.Execute("getPlayer true"));
    }

    [Test]
    public void Execute_Safe()
    {
        _safe.Execute("getPlayer false");
    }

    [Test]
    public void Execute_Safe_Exception()
    {
        ExecutionResult result = _safe.Execute("getPlayer true");
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.ReturnValue, Is.TypeOf<ArgumentException>());
    }
}
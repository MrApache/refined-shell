using System;
using NUnit.Framework;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(DelegateCommand))]
internal sealed class InstanceCommands
{
    private readonly Shell _shell;

    public InstanceCommands()
    {
        _shell = new Shell();
    }

    [Test]
    public void DirectlyExecution_Dispose()
    {
        _shell.Register(GetRandom, "random");

        ICommand command = _shell.GetCommand("random")!;
        bool result = command.Execute([]).Success;
        Assert.That(result, Is.True);

        _shell.Unregister("random");

        result = command.Execute([]).Success;
        Assert.That(result, Is.False);
    }

    [Test]
    public void DirectlyExecution_WeakReference()
    {
        ICommand random = FirstDirectlyExecution();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        bool result = random.Execute([]).Success;
        Assert.That(result, Is.False);
        _shell.RemoveNotValidCommands();
    }

    private ICommand FirstDirectlyExecution()
    {
        // ReSharper disable once ObjectCreationAsStatement
        new ClassWithCommands(_shell);
        ICommand command = _shell.GetCommand("random")!;
        bool result = command.Execute([]).Success;
        Assert.That(result, Is.True);
        return command;
    }

    [Test]
    public void Execute()
    {
        FirstExecution();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        bool success = _shell.Execute("Print error").Success;
        Assert.That(success, Is.False, "GC error");
        _shell.RemoveNotValidCommands();
        Assert.That(_shell.Count, Is.EqualTo(0));
    }

    private void FirstExecution()
    {
        // ReSharper disable once ObjectCreationAsStatement
        new ClassWithCommands(_shell);
        bool success = _shell.Execute("Print success").Success;
        Assert.That(success, Is.True);
    }

    [Test]
    public void Execute_Remove_Execute()
    {
        _shell.Register(DivideRandom, "drandom");
        _shell.Register(GetRandom, "random");
        bool success = _shell.Execute("drandom $(random)").Success;
        Assert.That(success, Is.True);
        _shell.Unregister("drandom");
        success = _shell.Execute("drandom $(random)").Success;
        Assert.That(success, Is.False);
        _shell.Unregister("random");
    }

    private static int DivideRandom(int random)
    {
        return random / 2;
    }
    
    private static int GetRandom()
    {
        return Random.Shared.Next();
    }
}
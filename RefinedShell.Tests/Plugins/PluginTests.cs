using NUnit.Framework;

namespace RefinedShell.Tests.Plugins;

internal sealed class PluginTests
{
    private readonly Shell _shell;
    private readonly EventListener _listener;

    public PluginTests()
    {
        _shell = new Shell();
        _listener = new EventListener();
        _shell.AttachPlugin(_listener);
    }

    [Test]
    public void AttachPlugin()
    {
        Shell shell = new Shell();
        EventListener listener = new EventListener();
        shell.AttachPlugin(listener);
        shell.RegisterMember(nameof(AttachPlugin), "attach", this);
        Assert.That(listener.IsAttached, Is.True);
        Assert.That(listener.AttachedShell, Is.EqualTo(shell));
    }

    [Test]
    public void AttachSamePluginInstance()
    {
        Shell shell = new Shell();
        EventListener listener = new EventListener();
        shell.AttachPlugin(listener);
        shell.AttachPlugin(listener);
        Assert.That(listener.AttachCount, Is.EqualTo(1));
    }

    [Test]
    public void Registration()
    {
        _shell.RegisterAllWithAttribute(this);
        Assert.That(_listener.RegisteredCommands.Count, Is.EqualTo(3));
        _shell.UnregisterAllWithAttribute(this);
    }

    [Test]
    public void Unregistration()
    {
        _shell.RegisterAllWithAttribute(this);
        _shell.UnregisterAllWithAttribute(this);
        Assert.That(_listener.RegisteredCommands.Count, Is.EqualTo(0));
    }

    [Test]
    public void BeforeExecution()
    {
        const string input = "empty; pipeline; count";
        _shell.RegisterAllWithAttribute(this);
        _shell.Execute(input);
        Assert.That(_listener.Commands.Count, Is.EqualTo(3));
        Assert.That(_listener.Input, Is.EqualTo(input));
        _shell.UnregisterAllWithAttribute(this);
    }

    [Test]
    public void AfterExecution()
    {
        const string input = "empty; pipeline; count";
        _shell.RegisterAllWithAttribute(this);
        ExecutionResult result = _shell.Execute(input);
        Assert.That(_listener.Result, Is.EqualTo(result));
        _shell.UnregisterAllWithAttribute(this);
    }

    [ShellFunction("empty")]
    private static void Empty(){}

    [ShellFunction("pipeline")]
    private static void Pipeline(){}

    [ShellFunction("count")]
    private static void Count() {}
}

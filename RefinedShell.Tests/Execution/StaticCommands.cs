using NUnit.Framework;
using RefinedShell.Execution;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(DelegateCommand))]
internal sealed class StaticCommands : DefaultShellSetup_Static
{
    [Test]
    public void ExecuteStatic_Attribute()
    {
        bool result = Shell.Execute("Print hello_world").Success;
        Assert.That(result, Is.True);
    }

    [Test]
    public void ExecuteStatic_Manual()
    {
        ExecutionResult result = Shell.Execute("random");
        Assert.That(result.Success, Is.True);
        Assert.That(result.ReturnValue, Is.TypeOf<int>());
    }

    [Test]
    public void ExecuteStatic_Combined()
    {
        ExecutionResult result = Shell.Execute("Print $(random)");
        Assert.That(result.Success, Is.True);
    }
}
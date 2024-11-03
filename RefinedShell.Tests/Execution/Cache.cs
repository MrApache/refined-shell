using NUnit.Framework;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Shell))]
internal sealed class Cache : DefaultShellSetup_Static
{
    [Test]
    public void ExecuteTwice()
    {
        Shell.Execute("Print refined_shell");
        Shell.Execute("Print refined_shell");
    }
}
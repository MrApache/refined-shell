using NUnit.Framework;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Shell))]
[TestOf(typeof(CommandCollection))]
internal sealed class CommandCollectionTest : DefaultShellSetup_Static
{
    [Test]
    public void GetCount()
    {
        Assert.That(Shell.Count, Is.EqualTo(3));
    }
}
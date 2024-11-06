using NUnit.Framework;
using RefinedShell.Execution;
using RefinedShell.Tests.Examples;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Compiler))]
internal sealed class CompilerTestCases
{
    private readonly Shell _shell;

    public CompilerTestCases()
    {
        _shell = new Shell();
    }

    [Test]
    public void ExecuteTestCases()
    {
        foreach (IExample example in ExampleCollection.Examples)
        {
            example.RegisterCommands(_shell);
            ExecutionResult actualResult = _shell.Execute(example.Input);
            Assert.That(actualResult, Is.EqualTo(example.ExecutionResult), $"Input: {example.Input}, {actualResult.Error}");
            example.UnregisterCommands(_shell);
        }
    }
}
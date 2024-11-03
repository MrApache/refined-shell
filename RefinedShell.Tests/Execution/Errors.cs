using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Execution;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(ExecutionError))]
internal sealed class Errors : DefaultShellSetup_Static
{
    private readonly Dictionary<string, ExecutionError> _testCases =
        new Dictionary<string, ExecutionError>
        {
            {"random", ExecutionError.None},
            {"ran@dom", ExecutionError.UnknownToken},
            {"(random", ExecutionError.InvalidUsageOfToken},
            {"$)random)", ExecutionError.UnexpectedToken},
            {"get_hash", ExecutionError.CommandNotFound},
            {"Print", ExecutionError.InsufficientArguments},
            {"Print hello world", ExecutionError.TooManyArguments},
            {"setValue one", ExecutionError.InvalidArgumentType},
            {"Print $(setValue 123)", ExecutionError.CommandHasNoReturnResult}
        };

    [Test]
    public void ExecuteTestCases()
    {
        foreach ((string input, ExecutionError expectedError) in _testCases)
        {
            Assert.That(Shell.Execute(input).Error, Is.EqualTo(expectedError));
        }
    }
}
using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Commands;
using RefinedShell.Execution;
using RefinedShell.Tests.Examples;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(ICommand))]
[TestOf(typeof(Shell))]
internal sealed class Validation
{
    private readonly Shell _shell;

    public Validation()
    {
        _shell = new Shell();
    }

    [Test]
    public void ExecuteTestCases_Unregister_ReturnsFalse()
    {
        foreach (ITestCase example in TestCaseCollection.Examples)
        {
            example.RegisterCommands(_shell);
            IReadOnlyCollection<ICommand> commands = _shell.GetAllCommands();
            example.UnregisterCommands(_shell);
            foreach (ICommand command in commands)
            {
                ExecutionResult result = command.Execute([]);
                Assert.That(result, Is.EqualTo(new ExecutionResult(false, null, new ProblemSegment(0, 0, ExecutionError.CommandNotValid))));
            }
        }
    }

    [Test]
    public void ExecuteTestCases_GC_ReturnsFalse()
    {
        foreach (Type exampleType in TestCaseCollection.CorrectInstanceTypes)
        {
            Register(exampleType);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            IReadOnlyCollection<ICommand> commands = _shell.GetAllCommands();
            foreach (ICommand command in commands)
            {
                ExecutionResult result = command.Execute([]);
                Assert.That(result, Is.EqualTo(new ExecutionResult(false, null, new ProblemSegment(0, 0, ExecutionError.CommandNotValid))));
            }
            _shell.UnregisterAll();
        }
    }

    private void Register(Type type)
    {
        ITestCase testCase = (ITestCase)Activator.CreateInstance(type)!;
        testCase.RegisterCommands(_shell);
    }
}
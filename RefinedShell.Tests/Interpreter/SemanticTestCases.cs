using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Semantic))]
internal sealed class SemanticTestCases
{
    private readonly Dictionary<string, ExecutionError> _testCases
        = new()
        {
            {
                "command arg1 arg2",
                ExecutionError.None
            },
            {
                "command1 arg1 arg2",
                ExecutionError.InsufficientArguments
            },
            {
                "command1",
                ExecutionError.InsufficientArguments
            },
            {
                "command1 arg1 arg2 arg3 arg4 arg5",
                ExecutionError.TooManyArguments
            },
            {
                "command2 value",
                ExecutionError.InvalidArgumentType
            },
            {
                "command2 21474836470",
                ExecutionError.InvalidArgumentType
            },
            {
                "command2 $(getValue)",
                ExecutionError.None
            },
            {
                "command2 $(convertValue $(getValue)) ",
                ExecutionError.None
            },
            {
                "command2 $(convertValue $(getValue)) arg1",
                ExecutionError.TooManyArguments
            },
            {
                "command2 $(convertValue)",
                ExecutionError.InsufficientArguments
            },
            {
                "command2 $(command)",
                ExecutionError.CommandHasNoReturnResult
            },
            {
                "not_found arg1 arg2",
                ExecutionError.CommandNotFound
            }
        };

    private Shell _shell = null!;

    [ShellFunction("command")]
    private static void Command(string arg1, string arg2) { }

    [ShellFunction("command1")]
    private static void Command1(string arg1, string arg2, string arg3) {}

    [ShellFunction("command2")]
    private static void Command2(int value) {}

    [ShellFunction("getValue")]
    private static int GetValue() => 0;

    [ShellFunction("convertValue")]
    private static int CovertValue(int value) => 0;

    [SetUp]
    public void Setup()
    {
        _shell = new Shell();
        _shell.RegisterAllWithAttribute<SemanticTestCases>(null);
    }

    [Test]
    public void Analyze()
    {
        foreach ((string input, ExecutionError errorType) in _testCases)
        {
            ProblemSegment error = _shell.Analyze(input);
            Assert.That(error.Error, Is.EqualTo(errorType), $"Test case: {input}");
        }
    }
}

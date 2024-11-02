using System;
using System.Collections.Generic;
using IrisShell.Interpreter;
using NUnit.Framework;

namespace IrisShell.Tests;

[TestOf(typeof(Compiler))]
[TestFixture]
internal sealed class CompilerTestCases
{
    [ShellCommand("command")]
    private static string Command(string arg1, string arg2)
    {
        return arg1 + arg2;
    }

    [ShellCommand("teleport")]
    private static int Teleport(int pos)
    {
        return pos;
    }

    [ShellCommand("getplayerpos")]
    private static int GetPlayerPos() => 1;

    [ShellCommand("teleport_2")]
    private static void Teleport(int pos, string name) {}

    [ShellCommand("getplayername")]
    private static string? GetPlayerName(bool execute)
    {
        if (execute)
            return "cutie";
        return null;
    }

    [ShellCommand("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private readonly Dictionary<string, ExecutionResult> _testCases = new Dictionary<string, ExecutionResult>
    {
        {
            "command arg1 arg2; teleport $(getplayerpos)",
            new ExecutionResult(true, new[]
            {
                new ExecutionResult(true, "arg1arg2"),
                new ExecutionResult(true, 1)
            })
        },
        {
            "$(getplayerpos)",
            new ExecutionResult(true, 1)
        },
        {
            "command arg1 arg2",
            new ExecutionResult(true, "arg1arg2")
        },
        {
            "teleport_2 $(getplayerpos) cute",
            new ExecutionResult(true, null)
        },
        {
            "teleport_2 $(getplayerpos) $(getplayername true)",
            new ExecutionResult(true, null)
        },
        {
            "teleport_2 $(getplayerpos) $(getplayername false)",
            new ExecutionResult(true, null)
        },
        {
            "$(getplayername true)",
            new ExecutionResult(true, "cutie")
        },
        {
            "$(getplayername false)",
            new ExecutionResult(true, null)
        },
        {
            "getplayername true",
            new ExecutionResult(true, "cutie")
        },
        {
            "getplayername false",
            new ExecutionResult(true, null)
        },
        {
            "print hello_world",
            new ExecutionResult(true, null)
        }
    };

    private readonly Shell _shell;

    public CompilerTestCases()
    {
        _shell = new Shell();
        _shell.RegisterAll<CompilerTestCases>(null);
    }

    [Test]
    public void ExecuteTestCases()
    {
        foreach ((string input, ExecutionResult expectedResult) in _testCases)
        {
            ExecutionResult actualResult = _shell.Execute(input);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
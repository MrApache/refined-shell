using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Execution;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Compiler))]
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

    /*
    [ShellCommand("vec_length")]
    private static float GetVector2Length(Vector2 vector)
    {
        return vector.Length();
    }
    */

    [ShellCommand("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private readonly Dictionary<string, ExecutionResult> _testCases = new Dictionary<string, ExecutionResult>
    {
        {
            "command arg1 arg2; teleport $(getplayerpos)",
            new ExecutionResult(true, 0,0,ExecutionError.None ,new[]
            {
                new ExecutionResult(true, 0, 0, ExecutionError.None, "arg1arg2"),
                new ExecutionResult(true, 0, 0, ExecutionError.None,1)
            })
        },
        {
            "$(getplayerpos)",
            new ExecutionResult(true, 0, 0, ExecutionError.None, 1)
        },
        {
            "command arg1 arg2",
            new ExecutionResult(true, 0, 0, ExecutionError.None, "arg1arg2")
        },
        {
            "teleport_2 $(getplayerpos) cute",
            new ExecutionResult(true, 0, 0, ExecutionError.None, null)
        },
        {
            "teleport_2 $(getplayerpos) $(getplayername true)",
            new ExecutionResult(true, 0, 0, ExecutionError.None, null)
        },
        {
            "teleport_2 $(getplayerpos) $(getplayername false)",
            new ExecutionResult(false, 0, 0, ExecutionError.ArgumentError, null)
        },
        {
            "$(getplayername true)",
            new ExecutionResult(true, 0, 0, ExecutionError.None, "cutie")
        },
        {
            "$(getplayername false)",
            new ExecutionResult(true, 0, 0, ExecutionError.None, null)
        },
        {
            "getplayername true",
            new ExecutionResult(true, 0, 0, ExecutionError.None, "cutie")
        },
        {
            "getplayername false",
            new ExecutionResult(true, 0, 0, ExecutionError.None, null)
        },
        {
            "print hello_world",
            new ExecutionResult(true, 0, 0, ExecutionError.None, null)
        },
        /*
        {
            "vec_length 100 4521",
            new ExecutionResult(true,0,0,ExecutionError.None, new Vector2(100, 4521).Length())
        },
        */
        {
            "print \"hello, world =)\"",
            new ExecutionResult(true, null, ProblemSegment.None)
        },
        {
            "",
            new ExecutionResult(false, null, ProblemSegment.None)
        }
    };

    private readonly Shell _shell;

    public CompilerTestCases()
    {
        //TypeParsers.AddParser<Vector2>(new Vector2Parser());
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
        //TypeParsers.Remove<Vector2>();
    }
}
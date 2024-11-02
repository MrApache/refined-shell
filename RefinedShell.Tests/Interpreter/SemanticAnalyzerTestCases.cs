using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(SemanticAnalyzer))]
    internal sealed class SemanticAnalyzerTestCases
    {
        private readonly Dictionary<string, SemanticError.ErrorType> _testCases
            = new Dictionary<string, SemanticError.ErrorType>
            {
                {
                    "command arg1 arg2",
                    SemanticError.ErrorType.ErrorsNotFound
                },
                {
                    "command1 arg1 arg2",
                    SemanticError.ErrorType.TooFewArguments
                },
                {
                    "command1",
                    SemanticError.ErrorType.TooFewArguments
                },
                {
                    "command1 arg1 arg2 arg3 arg4 arg5",
                    SemanticError.ErrorType.TooManyArguments
                },
                {
                    "command2 value",
                    SemanticError.ErrorType.InvalidArgumentType
                },
                {
                    "command2 21474836470",
                    SemanticError.ErrorType.InvalidArgumentType
                },
                {
                    "command2 $(getValue)",
                    SemanticError.ErrorType.ErrorsNotFound
                },
                {
                    "command2 $(convertValue $(getValue)) ",
                    SemanticError.ErrorType.ErrorsNotFound
                },
                {
                    "command2 $(convertValue $(getValue)) arg1",
                    SemanticError.ErrorType.TooManyArguments
                },
                {
                    "command2 $(convertValue)",
                    SemanticError.ErrorType.TooFewArguments
                },
                {
                    "command2 $(command)",
                    SemanticError.ErrorType.InlineCommandNoResult
                },
                {
                    "not_found arg1 arg2",
                    SemanticError.ErrorType.CommandNotFound
                }
            };

        private Shell _shell = null!;

        [ShellCommand("command")]
        private static void Command(string arg1, string arg2) { }

        [ShellCommand("command1")]
        private static void Command1(string arg1, string arg2, string arg3) {}

        [ShellCommand("command2")]
        private static void Command2(int value) {}

        [ShellCommand("getValue")]
        private static int GetValue() => 0;

        [ShellCommand("convertValue")]
        private static int CovertValue(int value) => 0;

        [SetUp]
        public void Setup()
        {
            _shell = new Shell();
            _shell.RegisterAll<SemanticAnalyzerTestCases>(null);
        }

        [Test]
        public void Analyze()
        {
            foreach ((string input, SemanticError.ErrorType errorType) in _testCases)
            {
                SemanticError error = _shell.Analyze(input);
                Assert.That(error.Error, Is.EqualTo(errorType));
            }
        }

        [Test]
        public void HasErrors()
        {
            foreach ((string input, SemanticError.ErrorType errorType) in _testCases)
            {
                bool actual = _shell.HasErrors(input);
                bool expected = errorType != SemanticError.ErrorType.ErrorsNotFound;
                Assert.That(actual == expected, Is.True);
            }
        }
    }
}
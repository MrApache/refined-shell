using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using RefinedShell.Execution;
using RefinedShell.Interpreter;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase31_Error : ITestCase
{
    public string Input => "ResetLayout 0";

    public Node? Expression =>
        new CommandNode(new Token(0, 11, TokenType.Identifier), "ResetLayout",
        [
            new ArgumentNode(new Token(12, 1, TokenType.Number), "0")
        ]);

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("ResetLayout", TokenType.Identifier),
        ("0", TokenType.Number)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(12, 1, ExecutionError.InsufficientArguments));

    [ShellFunction]
    private static void ResetLayout(ResetOptions options) {}

    public void RegisterCommands(Shell shell)
    {
        ParserLibrary.Default.AddParser<ResetOptions>(new ResetOptionsParser());
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
        ParserLibrary.Default.Remove<ResetOptions>();
    }

    private sealed class ResetOptionsParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(2, true);
            yield return new ArgumentInfo(1, true);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            bool result = float.TryParse(input[0], out float _) && float.TryParse(input[1], out float _);

            if (input.Length == 3)
                result &= bool.TryParse(input[2], out bool _);

            return result;
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            if(input.Length == 3)
                return new ResetOptions(new Vector2(float.Parse(input[0]!), float.Parse(input[1]!)), bool.Parse(input[3]!));

            return new ResetOptions(new Vector2(float.Parse(input[0]!), float.Parse(input[1]!)));
        }
    }

    private sealed class ResetOptions(Vector2 startPosition = default, bool dispose = false)
    {
        [UsedImplicitly]
        public readonly Vector2 Start = startPosition;
        [UsedImplicitly]
        public readonly bool Dispose = dispose;
    }
}
using System;
using RefinedShell.Commands;
using RefinedShell.Utilities;

namespace RefinedShell.test
{
    public sealed class PluginContext
    {
        public readonly Shell Shell;

        internal PluginContext(Shell shell)
        {
            Shell = shell;
        }

        public readonly ref struct Lifetime
        {
            public readonly ICommand Command;

            internal Lifetime(ICommand command)
            {
                Command = command;
            }
        }

        public readonly ref struct Execution
        {
            public readonly ReadOnlySpan<IReadOnlyCommand> Command;
            public readonly StringToken Input;
            public readonly ExecutionResult? Result;

            internal Execution(ReadOnlySpan<IReadOnlyCommand> command, StringToken input, ExecutionResult? result = null)
            {
                Command = command;
                Input = input;
                Result = result;
            }
        }
    }
}

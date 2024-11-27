using System;
using RefinedShell.Execution;

namespace RefinedShell.Interpreter
{
    internal sealed class InterpreterException : Exception
    {
        public readonly Token Token;
        public readonly ExecutionError Error;

        public InterpreterException(ExecutionError error, Token token)
        {
            Error = error;
            Token = token;
        }
    }
}

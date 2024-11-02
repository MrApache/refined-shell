using System;

namespace RefinedShell.Interpreter
{
    internal sealed class InterpreterException : Exception
    {
        public readonly Token Token;

        public InterpreterException(string message, Token token) : base(message)
        {
            Token = token;
        }
    }
}
using System;

namespace RefinedShell.Interpreter
{
    [Flags]
    internal enum TokenType
    {
        Unknown = 0,
        Value = 2,
        Semicolon = 4,
        Dollar = 8,
        OpenParenthesis = 16,
        CloseParenthesis = 32,
        String = 64,
        EndOfLine = 128
    }
}
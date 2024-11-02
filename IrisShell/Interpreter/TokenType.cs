using System;

namespace IrisShell.Interpreter
{
    [Flags]
    internal enum TokenType : byte
    {
        Unknown = 0,
        Value = 2,
        Semicolon = 4,
        Dollar = 8,
        OpenParenthesis = 16,
        CloseParenthesis = 32,
        EndOfLine = 64
    }
}
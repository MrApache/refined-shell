using System;

namespace RefinedShell.Interpreter
{
    [Flags]
    internal enum TokenType : short
    {
        Unknown = 0,
        EndOfLine = 1,
        Semicolon = 2,
        Dollar = 4,
        OpenParenthesis = 8,
        CloseParenthesis = 16,
        String = 32,
        Number = 64,
        Whitespace = 128,
        Identifier = 256,
        Ampersand = 512,
        VerticalBar = 1024
    }
}

namespace RefinedShell.Interpreter
{
    internal readonly struct TokenDefinition
    {
        public readonly TokenType TokenType;
        public readonly string Pattern;

        public TokenDefinition(TokenType tokenType, string pattern)
        {
            TokenType = tokenType;
            Pattern = pattern;
        }

        public void Deconstruct(out TokenType tokenType, out string pattern)
        {
            tokenType = TokenType;
            pattern = Pattern;
        }
    }
}
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RefinedShell.Interpreter
{
    internal sealed class Lexer
    {
        private readonly List<TokenDefinition> _tokenDefinitions;

        private string _input;
        private int _position;
        private int _previousPosition;
        private Token _currentToken;

        public Lexer()
        {
            _tokenDefinitions = new List<TokenDefinition>
            {
                new TokenDefinition(TokenType.Whitespace, @"^\s+"),
                new TokenDefinition(TokenType.String, "^\"{1}[^\"]*\"{1}"),
                new TokenDefinition(TokenType.Identifier, "^[a-zA-Z_][a-zA-Z0-9_]*"),
                new TokenDefinition(TokenType.Number, "^[0-9]+([.][0-9]*)?"),
                new TokenDefinition(TokenType.Semicolon, "^;"),
                new TokenDefinition(TokenType.Dollar, @"^\$"),
                new TokenDefinition(TokenType.OpenParenthesis, @"^\("),
                new TokenDefinition(TokenType.CloseParenthesis, @"^\)")
            };
            _input = string.Empty;
            _currentToken = default;
        }

        public void SetInputString(string input)
        {
            _input = input;
            _position = 0;
            _previousPosition = 0;
        }

        public Token GetPreviousToken()
        {
            _position = _previousPosition;
            return GetNextToken();
        }

        public Token GetNextToken()
        {
            _previousPosition = _currentToken.Start;
            Token nextToken = FindNextToken();
            _currentToken = nextToken;
            return nextToken;
        }

        private Token FindNextToken()
        {
            while(_position < _input.Length)
            {
                Token token = GoNext();
                if(token.Type == TokenType.Whitespace)
                    continue;

                if(token.Type == TokenType.Unknown)
                {
                    int start = _position;
                    Start:
                    _position++;
                    Token nextToken = FindNextToken();
                    if (nextToken.Type == TokenType.Unknown)
                    {
                        goto Start;
                    }

                    return new Token(start, _position - start, TokenType.Unknown);
                }

                return token;
            }

            return new Token(_input.Length, 0, TokenType.EndOfLine);
        }

        private Token GoNext()
        {
            string slice = _input.Substring(_position, _input.Length - _position);
            foreach ((TokenType tokenType, string pattern) in _tokenDefinitions)
            {
                Match match = Regex.Match(slice, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if(match.Success)
                {
                    Token token = new Token(_position, match.Length, tokenType);
                    _position += match.Length;
                    return token; // return token;
                }
            }

            return new Token(0, 0, TokenType.Unknown);
        }
    }
}
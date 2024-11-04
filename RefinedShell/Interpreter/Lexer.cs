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
                //new TokenDefinition(TokenType.Value,"^[a-zA-Z_][a-zA-Z0-9_]*$"),
                //new TokenDefinition(TokenType.Substitution,@"^[$]\([a-zA-Z0-9_ ]+\)$")
                new TokenDefinition(TokenType.Value,@"^\s*[a-zA-Z0-9_]*$"),
                //new TokenDefinition(TokenType.Semicolon,"[;]"),
                //new TokenDefinition(TokenType.Dollar,@"^\$"),
                //new TokenDefinition(TokenType.OpenParenthesis,@"\("),
                //new TokenDefinition(TokenType.CloseParenthesis,@"\)$"),
                //new TokenDefinition(TokenType.Quotes, "\""),
                new TokenDefinition(TokenType.String, "^\"{1}[^\"]*\"{1}$"),
            };
            _input = string.Empty;
            _currentToken = default;
        }

        //Command -- ^[a-zA-Z_][a-zA-Z0-9_]*$
        //String -- "^\"{1}[^\"]*\"{1}$"
        //String --

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

        private static bool IsSpecialSymbol(char c)
        {
            return c == ';' || c == '$' || c == '(' || c == ')' || c == ' ';
        }

        private Token FindNextToken()
        {
            while (_position < _input.Length && _input[_position] == ' ')
            {
                _position++;
            }

            if (_position >= _input.Length)
            {
                return new Token(_input.Length, 0, TokenType.EndOfLine);
            }

            switch (_input[_position])
            {
                case ';': return new Token(_position++, 1, TokenType.Semicolon);
                case '$': return new Token(_position++, 1, TokenType.Dollar);
                case '(': return new Token(_position++, 1, TokenType.OpenParenthesis);
                case ')': return new Token(_position++, 1, TokenType.CloseParenthesis);
            }

            int start = _position;

            while (_position < _input.Length && !IsSpecialSymbol(_input[_position]))
            {
                if (_input[_position] != '"')
                {
                    _position++;
                }
                else
                {
                    if(FindEnfOfString())
                        break;
                    return new Token(start, _position - start, TokenType.Unknown);
                }
            }

            int length = _position - start;
            string slice = _input.Substring(start, length);
            foreach ((TokenType tokenType, string pattern) in _tokenDefinitions)
            {
                if (Regex.IsMatch(slice, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase))
                {
                    return new Token(start, length, tokenType);
                }
            }

            return new Token(start, length, TokenType.Unknown);
        }

        private bool FindEnfOfString()
        {
            _position++;
            int quotes = 1;
            while (quotes != 2)
            {
                if (_position >= _input.Length)
                    return false;
                if (_input[_position] == '"')
                {
                    quotes++;
                }
                _position++;
            }
            return true;
        }
    }
}
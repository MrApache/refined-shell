using System;
using RefinedShell.Matching;

namespace RefinedShell.Interpreter
{
    internal sealed class Lexer
    {
        private static readonly (TokenType Type, Matcher Regex)[] _regex;

        static Lexer()
        {
            _regex = new[]
            {
                /*
                (TokenType.Whitespace, new Regex(@"^\s+", RegexOptions.Compiled)),
                (TokenType.String, new Regex("^\"{1}[^\"]*\"{1}", RegexOptions.Compiled)),
                (TokenType.Identifier, new Regex("^[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled)),
                (TokenType.Number, new Regex("^[-|+]?[0-9]+([.][0-9]*)?", RegexOptions.Compiled)),
                (TokenType.Semicolon, new Regex("^;", RegexOptions.Compiled)),
                (TokenType.Dollar, new Regex(@"^\$", RegexOptions.Compiled)),
                (TokenType.OpenParenthesis, new Regex(@"^\(", RegexOptions.Compiled)),
                (TokenType.CloseParenthesis, new Regex(@"^\)", RegexOptions.Compiled)),
                (TokenType.Ampersand, new Regex("^&", RegexOptions.Compiled)),
                (TokenType.VerticalBar, new Regex(@"^\|", RegexOptions.Compiled))
                */

                (TokenType.Whitespace, new Matcher(@"^\s+")),
                (TokenType.Dollar, new Matcher(@"^\$")),
                (TokenType.OpenParenthesis, new Matcher(@"^\(")),
                (TokenType.CloseParenthesis, new Matcher(@"^\)")),
                (TokenType.Semicolon, new Matcher("^;")),
                (TokenType.String, new Matcher("^\"[^\"]*\"")),
                (TokenType.Identifier, new Matcher("^[a-zA-Z_][a-zA-Z0-9_]*")),
                (TokenType.Number, new Matcher("^[-|+]?[0-9]+([.][0-9]*)?")),
                //(TokenType.Ampersand, new Matcher("^&")),
                //(TokenType.VerticalBar, new Matcher(@"^\|"))
            };
        }

        private string _input;
        private int _position;
        private Token _currentToken;
        private Token _previousToken;

        public Lexer()
        {
            _input = string.Empty;
            _currentToken = default;
        }

        public void SetInputString(string input)
        {
            _input = input;
            _position = 0;
            _previousToken = default;
            _currentToken = default;
        }

        public Token GetPreviousToken()
        {
            _position = _currentToken.Start;
            return _previousToken;
        }

        public Token GetNextToken()
        {
            _previousToken = _currentToken;
            return _currentToken = FindNextToken();
        }

        private Token FindNextToken()
        {
            while(_position < _input.Length) {
                Token token = GetToken();
                if(token.Type == TokenType.Whitespace)
                    continue;

                if (token.Type == TokenType.Unknown) {
                    int start = _position;

                    while (true) {
                        _position++;
                        Token nextToken = FindNextToken();

                        if (nextToken.Type != TokenType.Unknown) {
                            _position = nextToken.Start;
                            return new Token(start, nextToken.Start - start, TokenType.Unknown);
                        }
                    }
                }

                return token;
            }

            return new Token(_input.Length, 0, TokenType.EndOfLine);
        }

        private Token GetToken()
        {
            ReadOnlySpan<char> input = _input.AsSpan(_position);
            foreach ((TokenType type, Matcher regex) in _regex) {
                Match match = regex.Match(input);
                if (match.Success) {
                    Token token = new Token(_position, match.Length, type);
                    _position += match.Length;
                    return token;
                }
            }
            return new Token(0, 0, TokenType.Unknown);
        }
    }
}

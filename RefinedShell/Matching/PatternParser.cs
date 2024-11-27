using System.Collections.Generic;
using RefinedShell.Utilities;
using Exception = System.Exception;

namespace RefinedShell.Matching
{
    internal static class PatternParser
    {
        private static int _position;
        private static string _pattern = string.Empty;
        private static List<Token> _tokens = null!;

        private static void MoveRight()
        {
            _position++;
        }

        private static void MoveLeft()
        {
            _position--;
        }

        private static char CurrentChar()
        {
            return _pattern[_position];
        }

        private static void Push(Token token)
        {
            _tokens.Add(token);
        }

        private static Token? Pop()
        {
            if (_position == 0)
                return null;
            Token token = _tokens[^1];
            _tokens.RemoveAt(_tokens.Count - 1);
            return token;
        }

        public static List<Token> Parse(string pattern)
        {
            _pattern = pattern;
            _tokens = new List<Token>(pattern.Length);
            for(_position = 0; _position < pattern.Length; _position++) {
                Token token = GetToken(pattern[_position]);
                _tokens.Add(token);
            }

            _position = 0;
            return _tokens;
        }

        private static Token GetInvertedNumberClass()
        {
            _tokens.Add(new Token("["));
            _tokens.Add(new Token("^"));
            _tokens.Add(new Token(RegexTokens.Numbers));
            return new Token("]");
        }

        private static Token GetInvertedWordClass()
        {
            _tokens.Add(new Token("["));
            _tokens.Add(new Token("^"));
            _tokens.Add(new Token(RegexTokens.WordCharacters, Modifier.Symbol));
            return new Token("]");
        }

        private static Token GetWordClass()
        {
            _tokens.Add(new Token("["));
            _tokens.Add(new Token(RegexTokens.WordCharacters, Modifier.Symbol));
            return new Token("]");
        }

        private static Token GetWhitespaceClass()
        {
            _tokens.Add(new Token("["));
            _tokens.Add(new Token(RegexTokens.Whitespaces));
            return new Token("]");
        }

        private static Token GetNumberClass()
        {
            _tokens.Add(new Token("["));
            _tokens.Add(new Token(RegexTokens.Numbers));
            return new Token("]");
        }

        private static Token GetEscape()
        {
            MoveRight();
            return CurrentChar() switch
            {
                '0' => new Token(RegexTokens.Null, Modifier.Escape),
                'a' => new Token(RegexTokens.Bell, Modifier.Escape),
                'd' => GetNumberClass(),
                'n' => new Token(RegexTokens.Newline, Modifier.Escape),
                'r' => new Token(RegexTokens.CarriageReturn, Modifier.Escape),
                's' => GetWhitespaceClass(),
                't' => new Token(RegexTokens.Tab, Modifier.Escape),
                'v' => new Token(RegexTokens.VerticalTab, Modifier.Escape),
                'w' => GetWordClass(),

                'D' => GetInvertedNumberClass(),
                'W' => GetInvertedWordClass(),
                _ => new Token(GetSymbol(CurrentChar()), Modifier.Symbol)
            };
        }

        private static Token GetToken(char symbol)
        {
            switch (symbol) {
                case '\\': return GetEscape();
                case '[':
                {
                    _tokens.Add(new Token("["));
                    return new Token(ParseGroup());
                }
                case '^': return new Token(RegexTokens.StartOfLine);
                case '$': return new Token(RegexTokens.EndOfLine);
                case '|': return new Token(RegexTokens.Or);
                case '{': return ParseCountCondition();
                case '(': return new Token("(");
                case ')': return new Token(")");
                case '.': return new Token(".");
                case ']': return new Token("]");
                case '*': return new Token("*");
                default:
                {
                    string s = GetSymbol(symbol);
                    Token? previous = Pop();

                    if (previous == null) {
                        return new Token(s, Modifier.Symbol);
                    }

                    if (previous.Modifier != Modifier.Symbol) {
                        Push(previous);
                        return new Token(s, Modifier.Symbol);
                    }
                    return new Token(previous.Value + s, Modifier.Symbol);
                }
            }
        }

        private static Token ParseCountCondition()
        {
            List<char> numbers = new List<char>(8);
            _tokens.Add(new Token("{"));
            while (CurrentChar() != '}') {
                MoveRight();
                if (_position >= _pattern.Length) {
                    throw new Exception("expected: '}' ");
                }
                if (char.IsNumber(CurrentChar())) {
                    numbers.Add(CurrentChar());
                }
                else if (CurrentChar() == ',') {
                    _tokens.Add(new Token(numbers.AsString()));
                    _tokens.Add(new Token(","));
                    numbers.Clear();
                }
                else if (CurrentChar() == '}') {
                    _tokens.Add(new Token(numbers.AsString()));
                    numbers.Clear();
                }
                else {
                    throw new Exception($"Unsupported symbol: {CurrentChar()}");
                }
            }

            return new Token("}");
        }

        private static string GetSymbol(char symbol)
        {
            return symbol.ToString();
        }

        private static string ParseGroup()
        {
            HashSet<char> chars = new HashSet<char>();
            int start = _position;
            char current;
            bool isRangeApplicable = false;

            MoveRight();
            while((current = CurrentChar()) != ']')
            {
                Token token = GetToken(current);
                switch (token.Value) {
                    case "-":
                    {
                        if (_pattern[_position - 1] == '\\'
                            || _pattern[_position - 1] == '['
                            || _pattern[_position + 1] == ']') {
                            chars.Add('-');
                            isRangeApplicable = true;
                            break;
                        }

                        if (isRangeApplicable) {
                            isRangeApplicable = false;
                            string rangeToken = Token.CreateRange(_pattern[start + 1], _pattern[_position + 1]);
                            start = ++_position;
                            chars.AddRange(rangeToken);
                        }

                        break;
                    }
                    case "|":
                        _tokens.Add(new Token(chars.AsString()));
                        _tokens.Add(new Token(RegexTokens.Or));
                        start = _position;
                        chars.Clear();
                        break;
                    case "^":
                        _tokens.Add(new Token("^"));
                        start = _position;
                        break;
                    default:
                        chars.AddRange(token.Value);
                        isRangeApplicable = true;
                        break;
                }
                MoveRight();
            }
            MoveLeft();
            return chars.AsString();
        }
    }
}

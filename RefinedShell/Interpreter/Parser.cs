using System;
using System.Collections.Generic;
using RefinedShell.Execution;

namespace RefinedShell.Interpreter
{
    internal sealed class Parser
    {
        private readonly Lexer _lexer;
        private Token _currentToken;

        public Parser()
        {
            _lexer = new Lexer();
        }

        private void GetNextToken()
        {
            _currentToken = _lexer.GetNextToken();
        }

        private void GetPreviousToken()
        {
            _currentToken = _lexer.GetPreviousToken();
        }

        private Token? Match(TokenType types)
        {
            Token current = _currentToken;
            TokenType result = current.Type & types;
            if(result == current.Type && result != 0)
            {
                GetNextToken();
                return current;
            }

            return null;
        }

        public Expression GetExpression(ReadOnlySpan<char> input)
        {
            Expression expression = new Expression();
            _lexer.SetInputString(input.ToString());
            GetNextToken();
            while(true)
            {
                CommandNode commandNode;
                Token token = _currentToken;
                switch (token.Type)
                {
                    case TokenType.Value:
                    {
                        commandNode = ParseCommand(input);
                        break;
                    }
                    case TokenType.Semicolon:
                    {
                        GetNextToken();
                        continue;
                    }
                    case TokenType.Dollar:
                    {
                        commandNode = ParseInlineCommand(input);
                        break;
                    }
                    case TokenType.EndOfLine:
                    {
                        return expression;
                    }
                    case TokenType.OpenParenthesis:
                    {
                        //throw new InterpreterException("Invalid use of '('", _currentToken);
                        throw new InterpreterException(ExecutionError.InvalidUsageOfToken, _currentToken);
                    }
                    case TokenType.CloseParenthesis:
                    {
                        //throw new InterpreterException("Invalid use of ')'", _currentToken);
                        throw new InterpreterException(ExecutionError.InvalidUsageOfToken, _currentToken);
                    }
                    case TokenType.Unknown:
                    default:
                    {
                        //throw new InterpreterException("Unknown token", _currentToken);
                        throw new InterpreterException(ExecutionError.UnknownToken, _currentToken);
                    }
                }
                expression.Add(commandNode);
                if (Match(TokenType.Semicolon) == null)
                    break;
            }

            return expression;
        }

        private List<Node> ParseArguments(ReadOnlySpan<char> input, bool inlineArguments = false)
        {
            List<Node> list = new List<Node>();
            while (true)
            {
                Token token = _currentToken;
                GetNextToken();
                switch (token.Type)
                {
                    case TokenType.Value:
                    {
                        ReadOnlySpan<char> argument = input.Slice(token.Start, token.Length);
                        list.Add(new ArgumentNode(token, argument.ToString()));
                        break;
                    }
                    case TokenType.Dollar:
                    {
                        GetPreviousToken();
                        list.Add(ParseInlineCommand(input));
                        break;
                    }
                    case TokenType.EndOfLine:
                    case TokenType.Semicolon:
                    {
                        GetPreviousToken();
                        return list;
                    }
                    case TokenType.OpenParenthesis:
                    {
                        //throw new InterpreterException("Invalid use of '('", token);
                        throw new InterpreterException(ExecutionError.InvalidUsageOfToken, token);
                    }
                    case TokenType.CloseParenthesis:
                    {
                        if(inlineArguments)
                        {
                            GetPreviousToken();
                            return list;
                        }
                        //throw new InterpreterException("Invalid use of ')'", token);
                        throw new InterpreterException(ExecutionError.InvalidUsageOfToken, token);
                    }
                    case TokenType.Unknown:
                    default:
                    {
                        //throw new InterpreterException("Unknown token", token);
                        throw new InterpreterException(ExecutionError.UnknownToken, token);
                    }
                }
            }
        }

        private CommandNode ParseInlineCommand(ReadOnlySpan<char> input)
        {
            if (Match(TokenType.Dollar) == null)
            {
                throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                //throw new InterpreterException("'$' expected", _currentToken);
            }

            if(Match(TokenType.OpenParenthesis) == null)
            {
                //throw new InterpreterException("'(' expected", _currentToken);
                throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
            }

            CommandNode command = ParseCommand(input, true);
            if (Match(TokenType.CloseParenthesis) == null)
                //throw new InterpreterException("')' expected", _currentToken);
                throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);

            return command;
        }
        
        private CommandNode ParseCommand(ReadOnlySpan<char> input, bool inline = false)
        {
            if (Match(TokenType.Value | TokenType.Dollar) == null)
            {
                //throw new InterpreterException("Unexpected token", _currentToken);
                throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
            }

            GetPreviousToken();
            Token commandToken = _currentToken;
            ReadOnlySpan<char> commandName = input.Slice(_currentToken.Start, _currentToken.Length);
            GetNextToken();
            List<Node> arguments = ParseArguments(input, inline);
            return new CommandNode(commandToken, commandName.ToString(), arguments.ToArray(), inline);
        }
    }
}
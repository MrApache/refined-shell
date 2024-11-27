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
            GetNextToken();
            Token current = _currentToken;
            TokenType result = current.Type & types;
            if(result == current.Type && result != 0) {
                return current;
            }

            return null;
        }

        private void ThrowIfNull(Token? token)
        {
            if (token != null)
                return;

            switch (_currentToken.Type) {
                case TokenType.Unknown: throw new InterpreterException(ExecutionError.UnknownToken, _currentToken);
                case TokenType.EndOfLine:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.Semicolon:
                    throw new InterpreterException(ExecutionError.InvalidUsageOfToken, _currentToken);
                case TokenType.Dollar:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.OpenParenthesis:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.CloseParenthesis:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.String:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.Number:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.Whitespace:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.Identifier:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.Ampersand:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
                case TokenType.VerticalBar:
                    throw new InterpreterException(ExecutionError.UnexpectedToken, _currentToken);
            }
        }

        private void ConsumeOpenParenthesis()
        {
            Token? token = Match(TokenType.OpenParenthesis);
            ThrowIfNull(token);
        }

        private void ConsumeCloseParenthesis()
        {
            Token? token = Match(TokenType.CloseParenthesis);
            ThrowIfNull(token);
        }

        private void ConsumeAmpersand()
        {
            Token? token = Match(TokenType.Ampersand);
            ThrowIfNull(token);
        }

        private void ConsumeVerticalBar()
        {
            Token? token = Match(TokenType.VerticalBar);
            ThrowIfNull(token);
        }

        private void ConsumeDollar()
        {
            Token? token = Match(TokenType.Dollar);
            ThrowIfNull(token);
        }

        private Token ConsumeSemicolon()
        {
            Token? token = Match(TokenType.Semicolon);
            ThrowIfNull(token);
            return token!.Value;
        }

        private Token ConsumeIdentifier()
        {
            Token? token = Match(TokenType.Identifier);
            ThrowIfNull(token);
            return token!.Value;
        }

        private Token ConsumeNumber()
        {
            Token? token = Match(TokenType.Number);
            ThrowIfNull(token);
            return token!.Value;
        }

        private Token? TryConsumeArgument()
        {
            return Match(TokenType.Identifier | TokenType.Number
                    | TokenType.String | TokenType.Dollar);
        }

        private Token ConsumeCommand()
        {
            Token? token = Match(TokenType.Identifier | TokenType.Dollar);
            ThrowIfNull(token);
            return token!.Value;
        }

        private Token? ConsumeEndOfLine()
        {
            Token? token = Match(TokenType.EndOfLine);
            ThrowIfNull(token);
            return token!.Value;
        }

        private Token? TryConsumeLogicalToken()
        {
            return Match(TokenType.Semicolon | TokenType.Ampersand
                    | TokenType.VerticalBar);
        }

        public Node GetExpression(ReadOnlySpan<char> input)
        {
            _currentToken = default;
            _lexer.SetInputString(input.ToString());
            Node tree = ParseExpression(input);
            ConsumeEndOfLine();
            return tree;
        }

        private Node ParseExpression(ReadOnlySpan<char> input)
        {
            CommandNode first = ParseCommand(input);
            Node second;
            Token token = TryConsumeLogicalToken() ?? default;
            switch(token.Type)
            {
                case TokenType.Ampersand:
                {
                    ConsumeAmpersand();
                    second = ParseExpression(input);
                    return new LogicalNode(first, second, LogicalNode.LogicalNodeType.And);
                }
                case TokenType.VerticalBar:
                {
                    ConsumeVerticalBar();
                    second = ParseExpression(input);
                    return new LogicalNode(first, second, LogicalNode.LogicalNodeType.Or);
                }
                case TokenType.Semicolon:
                {
                    second = ParseExpression(input);
                    return new SequenceNode(first, second);
                }
                default:
                {
                    GetPreviousToken();
                    break;
                }
            }
            return first;
        }

        private CommandNode ParseCommand(ReadOnlySpan<char> input, bool inline = false)
        {
            CommandNode node = null!;
            Token token = ConsumeCommand();
            GetPreviousToken();
            switch (token.Type)
            {
                case TokenType.Identifier:
                {
                    token = ConsumeCommand();
                    ReadOnlySpan<char> commandName = input.Slice(token.Start, token.Length);
                    List<Node> arguments = ParseArguments(input);
                    node = new CommandNode(token, commandName.ToString(), arguments.ToArray(), inline);
                    break;
                }
                case TokenType.Dollar:
                {
                    node = ParseInlineCommand(input);
                    break;
                }
            }
            return node;
        }

        private CommandNode ParseInlineCommand(ReadOnlySpan<char> input)
        {
            ConsumeDollar();
            ConsumeOpenParenthesis();
            CommandNode command = ParseCommand(input, true);
            ConsumeCloseParenthesis();
            return command;
        }

        private List<Node> ParseArguments(ReadOnlySpan<char> input)
        {
            List<Node> list = new List<Node>();
            while (true) {
                Token? token = TryConsumeArgument();
                if(token == null)
                    break;

                switch (token.Value.Type)
                {
                    case TokenType.String:
                    {
                        ReadOnlySpan<char> argument = input.Slice(token.Value.Start + 1, token.Value.Length - 2); //Remove quotes
                        list.Add(new ArgumentNode(token.Value, argument.ToString()));
                        break;
                    }
                    case TokenType.Number:
                    case TokenType.Identifier:
                    {
                        ReadOnlySpan<char> argument = input.Slice(token.Value.Start, token.Value.Length);
                        list.Add(new ArgumentNode(token.Value, argument.ToString()));
                        break;
                    }
                    case TokenType.Dollar:
                    {
                        GetPreviousToken();
                        list.Add(ParseInlineCommand(input));
                        break;
                    }
                }
            }
            GetPreviousToken();
            return list;
        }
    }
}

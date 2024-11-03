using System;
using System.Linq;
using System.Reflection;
using RefinedShell.Execution;
using RefinedShell.Parsing;

namespace RefinedShell.Interpreter
{
    internal sealed class SemanticAnalyzer
    {
        private readonly CommandCollection _collection;

        public SemanticAnalyzer(CommandCollection collection)
        {
            _collection = collection;
        }

        public SemanticError Analyze(Expression expression)
        {
            foreach (CommandNode commandNode in expression)
            {
                SemanticError error = ValidateCommand(commandNode);
                if (error.Error != ExecutionError.None)
                    return error;
            }

            return SemanticError.None;
        }

        public bool HasErrors(Expression expression)
        {
            foreach (CommandNode commandNode in expression)
            {
                SemanticError error = ValidateCommand(commandNode);
                if (error.Error != ExecutionError.None)
                    return true;
            }

            return false;
        }

        private SemanticError ValidateCommand(CommandNode commandNode)
        {
            if(!_collection.TryGetCommand(commandNode.Command, out ICommand? command))
            {
                return new SemanticError(commandNode.Token.Start,
                    commandNode.Token.Length,
                    ExecutionError.CommandNotFound);
            }

            if(commandNode.Inline && !command.ReturnsResult)
            {
                return new SemanticError(commandNode.Token.Start,
                    commandNode.Token.Length,
                    ExecutionError.CommandHasNoReturnResult);
            }

            Node[] actualArguments = commandNode.Arguments;
            int expectedArguments = command.Arguments.Sum(parameter => (int)TypeParsers.GetParser(parameter.ParameterType).OptionsCount);
            if(actualArguments.Length < expectedArguments)
            {
                if(actualArguments.Length == 0)
                {
                    return new SemanticError(commandNode.Token.Start + commandNode.Token.Length, 1,
                        ExecutionError.InsufficientArguments);
                }

                (int start, int length) = GetArgumentSlice(actualArguments);
                return new SemanticError(start, length, ExecutionError.InsufficientArguments);
            }

            if(actualArguments.Length > expectedArguments)
            {
                (int start, int length) = GetArgumentSlice(actualArguments);
                return new SemanticError(start, length, ExecutionError.TooManyArguments);
            }

            return ValidateArguments(command, commandNode.Arguments);
        }

        private SemanticError ValidateArguments(ICommand command, Node[] arguments)
        {
            int start = 0;

            for (int i = 0; i < command.Arguments.Length; i++)
            {
                ParameterInfo parameter = command.Arguments[i];
                ITypeParser parser = TypeParsers.GetParser(parameter.ParameterType);

                int length = (int)parser.OptionsCount;
                ReadOnlySpan<Node> argumentSlice = arguments.AsSpan(start, length);
                SemanticError error = ValidateArgument(parser, argumentSlice);
                if (error != SemanticError.None)
                    return error;
                start += length;
                i += length - 1;
            }

            return SemanticError.None;
        }
        
        private SemanticError ValidateArgument(ITypeParser parser, ReadOnlySpan<Node> arguments)
        {
            if (arguments.Length == 1 && arguments[0] is ArgumentNode an)
            {
                string[] arg = { an.Argument };
                if(!parser.CanParse(arg))
                {
                    return new SemanticError(an.Token.Start, an.Token.Length,
                        ExecutionError.InvalidArgumentType);
                }
            }
            if (ContainsCommandNode(arguments))
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    switch (arguments[i])
                    {
                        case ArgumentNode _:
                        {
                            SemanticError error = ValidateArgument(parser, arguments.Slice(i, 1));
                            if (error != SemanticError.None)
                                return error;
                            break;
                        }
                        case CommandNode c:
                        {
                            SemanticError error = ValidateCommand(c);
                            if (error != SemanticError.None)
                                return error;
                            break;
                        }
                    }
                }

                return SemanticError.None;
            }

            string[] rawArgs = new string[arguments.Length];
            int start = ((ArgumentNode)arguments[0]).Token.Start;
            int length = 0;
            for (int i = 0; i < arguments.Length; i++)
            {
                ArgumentNode node = (ArgumentNode)arguments[i];
                length += node.Token.Length;
                rawArgs[i] = node.Argument;
            }

            if (!parser.CanParse(rawArgs))
            {
                return new SemanticError(start, length,
                    ExecutionError.InvalidArgumentType);
            }

            return SemanticError.None;
        }

        private static Token GetTokenFromArgumentNode(Node node)
        {
            Token token;
            if (node is ArgumentNode an)
            {
                token = an.Token;
            }
            else
            {
                token = ((CommandNode)node).Token;
            }
            return token;
        }

        private static (int start, int length) GetArgumentSlice(Node[] arguments)
        {
            Token token = GetTokenFromArgumentNode(arguments[0]);
            int start = token.Start;
            int length = token.Length;
            for (int i = 1; i < arguments.Length; i++)
            {
                length += GetTokenFromArgumentNode(arguments[i]).Length;
            }

            return new ValueTuple<int, int>(start, length);
        }
        
        private static bool ContainsCommandNode(ReadOnlySpan<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (node is CommandNode)
                    return true;
            }

            return false;
        }
    }
}
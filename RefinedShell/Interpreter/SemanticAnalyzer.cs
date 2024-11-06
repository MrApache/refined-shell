using System;
using RefinedShell.Commands;
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

        public ProblemSegment Analyze(Expression expression)
        {
            foreach (CommandNode commandNode in expression)
            {
                ProblemSegment error = ValidateCommand(commandNode);
                if (error.Error != ExecutionError.None)
                    return error;
            }

            return ProblemSegment.None;
        }

        private ProblemSegment ValidateCommand(CommandNode commandNode)
        {
            if(!_collection.TryGetCommand(commandNode.Command, out ICommand? command))
            {
                return new ProblemSegment(commandNode.Token.Start,
                    commandNode.Token.Length,
                    ExecutionError.CommandNotFound);
            }

            if(commandNode.Inline && !command.ReturnsResult)
            {
                return new ProblemSegment(commandNode.Token.Start,
                    commandNode.Token.Length,
                    ExecutionError.CommandHasNoReturnResult);
            }

            Node[] actualArguments = commandNode.Arguments;

            int minimalCount = 0;
            int maximumCount = 0;
            int firstOptional = -1;

            for (int i = 0; i < command.Arguments.Count; i++)
            {
                Argument argument = command.Arguments[i];
                if (argument.IsOptional && firstOptional == -1)
                    firstOptional = i;
                ITypeParser parser = TypeParsers.GetParser(argument.Type);
                if(!argument.IsOptional)
                    minimalCount += (int)parser.ArgumentCount;
                maximumCount += (int)(parser.ArgumentCount + parser.OptionalCount);
            }

            if(actualArguments.Length < minimalCount)
            {
                if(actualArguments.Length == 0)
                {
                    return new ProblemSegment(commandNode.Token.Start, commandNode.Token.Length,
                        ExecutionError.InsufficientArguments);
                }

                (int start, int length) = GetArgumentSlice(actualArguments);
                return new ProblemSegment(start, length, ExecutionError.InsufficientArguments);
            }

            if(actualArguments.Length > minimalCount && actualArguments.Length < maximumCount)
            {
                int current = actualArguments.Length - minimalCount;
                int temp = 0;

                for (int i = firstOptional; i < command.Arguments.Count; i++)
                {
                    Argument argument = command.Arguments[i];
                    ITypeParser parser = TypeParsers.GetParser(argument.Type);
                    temp += (int)parser.OptionalCount;
                    if (temp == current) {
                        break;
                    }
                    if(temp > current) {
                        (int start, int length) = GetArgumentSlice(actualArguments);
                        return new ProblemSegment(start, length, ExecutionError.InsufficientArguments);
                    }
                }
            }
            else if(actualArguments.Length > maximumCount)
            {
                (int start, int length) = GetArgumentSlice(actualArguments);
                return new ProblemSegment(start, length, ExecutionError.TooManyArguments);
            }

            return ValidateArguments(command, commandNode.Arguments);
        }

        private ProblemSegment ValidateArguments(ICommand command, Node[] arguments)
        {
            int start = 0;

            for (int i = 0; i < command.Arguments.Count; i++)
            {
                Argument argument = command.Arguments[i];
                ITypeParser parser = TypeParsers.GetParser(argument.Type);
                if (argument.IsOptional)
                {
                    if(i + parser.ArgumentCount >= command.Arguments.Count)
                        break;
                }

                int length = (int)parser.ArgumentCount;
                ReadOnlySpan<Node> argumentSlice = arguments.AsSpan(start, length);
                ProblemSegment error = ValidateArgument(parser, argumentSlice);
                if (error != ProblemSegment.None)
                    return error;
                start += length;
                i += length - 1;
            }

            return ProblemSegment.None;
        }

        private ProblemSegment ValidateArgument(ITypeParser parser, ReadOnlySpan<Node> arguments)
        {
            if (arguments.Length == 1 && arguments[0] is ArgumentNode an)
            {
                string[] arg = { an.Argument };
                if(!parser.CanParse(arg))
                {
                    return new ProblemSegment(an.Token.Start, an.Token.Length,
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
                            ProblemSegment error = ValidateArgument(parser, arguments.Slice(i, 1));
                            if (error != ProblemSegment.None)
                                return error;
                            break;
                        }
                        case CommandNode c:
                        {
                            ProblemSegment error = ValidateCommand(c);
                            if (error != ProblemSegment.None)
                                return error;
                            break;
                        }
                    }
                }

                return ProblemSegment.None;
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
                return new ProblemSegment(start, length,
                    ExecutionError.InvalidArgumentType);
            }

            return ProblemSegment.None;
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
            Token startToken = GetTokenFromArgumentNode(arguments[0]);
            int start = startToken.Start;

            Token endToken = GetTokenFromArgumentNode(arguments[^1]);
            int end = endToken.Start + endToken.Length;

            return new ValueTuple<int, int>(start, end - start);
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
        
        private static int GetTotalArgumentsCountForArgumentType(Argument argument)
        {
            ITypeParser parser = TypeParsers.GetParser(argument.Type);
            return (int)(parser.ArgumentCount + parser.OptionalCount);
        }

        private static int GetMinimalArgumentCount(Argument argument)
        {
            ITypeParser parser = TypeParsers.GetParser(argument.Type);
            return (int)parser.ArgumentCount;
        }
    }
}
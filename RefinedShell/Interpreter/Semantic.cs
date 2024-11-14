using System;
using System.Collections.Generic;
using RefinedShell.Commands;
using RefinedShell.Execution;
using RefinedShell.Parsing;

namespace RefinedShell.Interpreter
{
    internal sealed class Semantic
    {
        private readonly CommandCollection _collection;
        private readonly ParserLibrary _parsers;

        public Semantic(CommandCollection collection, ParserLibrary parsers)
        {
            _collection = collection;
            _parsers = parsers;
        }

        public ProblemSegment Analyze(Expression expression)
        {
            foreach (CommandNode commandNode in expression) {
                ProblemSegment error = ValidateCommand(commandNode);
                if (error.Error != ExecutionError.None)
                    return error;
            }

            return ProblemSegment.None;
        }

        private ProblemSegment ValidateArguments(Arguments argsDecl, Node[] argsNode)
        {
            if (argsDecl.Count > 0 && argsNode.Length == 0) {
                if (argsDecl[0].IsOptional) {
                    return ProblemSegment.None;
                }
                return new ProblemSegment(0, 0, ExecutionError.InsufficientArguments);
            }

            int start = 0;
            int required = 0;
            int tempCounter = 0;
            for (int i = 0; i < argsDecl.Count; i++) {
                Argument arg = argsDecl[i];
                ITypeParser parser = _parsers.GetParser(arg.Type);
                IEnumerator<ArgumentInfo> argInfo = parser.GetArgumentInfo();
                while (argInfo.MoveNext()) {
                    ArgumentInfo current = argInfo.Current;
                    int length = (int)current.ElementCount;

                    if (current.IsOptional) {
                        if (required % argsNode.Length == 0) {
                            if (start + length > argsNode.Length) {
                                if (argsNode.Length == tempCounter)
                                    return ProblemSegment.None;
                                (int Start, int Length) = GetArgumentSlice(argsNode);
                                return new ProblemSegment(Start, Length, ExecutionError.InsufficientArguments);
                            }
                        }
                        else {
                            (int Start1, int Length1) = GetArgumentSlice(argsNode);
                            return new ProblemSegment(Start1, Length1, ExecutionError.InsufficientArguments);
                        }

                        tempCounter += length;
                    }

                    if (start + length > argsNode.Length) {
                        (int Start, int Length) = GetArgumentSlice(argsNode);
                        return new ProblemSegment(Start, Length, ExecutionError.InsufficientArguments);
                    }

                    Span<Node> slice = argsNode.AsSpan(start, length);
                    ProblemSegment segment = ValidateArgument(parser, slice);
                    if (segment != ProblemSegment.None)
                        return segment;
                    start += length;
                    required += length;
                }
            }

            if (start < argsNode.Length) {
                (int Start, int length) = GetArgumentSlice(argsNode);
                return new ProblemSegment(Start, length, ExecutionError.TooManyArguments);
            }

            return ProblemSegment.None;
        }

        private ProblemSegment ValidateCommand(CommandNode commandNode)
        {
            if(!_collection.TryGetCommand(commandNode.Command, out ICommand? command)) {
                return new ProblemSegment(commandNode.Token.Start,
                    commandNode.Token.Length,
                    ExecutionError.CommandNotFound);
            }

            if(commandNode.Inline && !command.ReturnsResult) {
                return new ProblemSegment(commandNode.Token.Start,
                    commandNode.Token.Length,
                    ExecutionError.CommandHasNoReturnResult);
            }

            return ValidateArguments(command.Arguments, commandNode.Arguments);
        }

        private ProblemSegment ValidateArgument(ITypeParser parser, ReadOnlySpan<Node> arguments)
        {
            if (arguments.Length == 1 && arguments[0] is ArgumentNode an) {
                string[] arg = { an.Argument };
                if(!parser.CanParse(arg)) {
                    return new ProblemSegment(an.Token.Start, an.Token.Length,
                        ExecutionError.InvalidArgumentType);
                }
            }
            if (ContainsCommandNode(arguments)) {
                for (int i = 0; i < arguments.Length; i++) {
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
            for (int i = 0; i < arguments.Length; i++) {
                ArgumentNode node = (ArgumentNode)arguments[i];
                length += node.Token.Length;
                rawArgs[i] = node.Argument;
            }

            if (!parser.CanParse(rawArgs)) {
                return new ProblemSegment(start, length,
                    ExecutionError.InvalidArgumentType);
            }

            return ProblemSegment.None;
        }

        private static Token GetTokenFromArgumentNode(Node node)
        {
            Token token;
            if (node is ArgumentNode an) {
                token = an.Token;
            }
            else {
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
            foreach (Node node in nodes) {
                if (node is CommandNode)
                    return true;
            }

            return false;
        }
    }
}
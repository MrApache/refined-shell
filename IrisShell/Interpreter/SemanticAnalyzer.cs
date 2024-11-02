using System;
using System.Reflection;
using IrisShell.Parsing;

namespace IrisShell.Interpreter
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
                if (error.Error != SemanticError.ErrorType.ErrorsNotFound)
                    return error;
            }

            return SemanticError.NoErrors;
        }

        public bool HasErrors(Expression expression)
        {
            foreach (CommandNode commandNode in expression)
            {
                SemanticError error = ValidateCommand(commandNode);
                if (error.Error != SemanticError.ErrorType.ErrorsNotFound)
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
                    SemanticError.ErrorType.CommandNotFound);
            }

            if(commandNode.Inline && !command.ReturnsResult)
            {
                return new SemanticError(commandNode.Token.Start,
                    commandNode.Token.Length,
                    SemanticError.ErrorType.InlineCommandNoResult);
            }

            Node[] actualArguments = commandNode.Arguments;
            ParameterInfo[] expectedArguments = command.Arguments;
            if(actualArguments.Length < expectedArguments.Length)
            {
                if(actualArguments.Length == 0)
                {
                    return new SemanticError(commandNode.Token.Start + commandNode.Token.Length, 1,
                        SemanticError.ErrorType.TooFewArguments);
                }

                (int start, int length) = GetArgumentSlice(actualArguments);
                return new SemanticError(start, length, SemanticError.ErrorType.TooFewArguments);
            }

            if(actualArguments.Length > expectedArguments.Length)
            {
                (int start, int length) = GetArgumentSlice(actualArguments);
                return new SemanticError(start, length, SemanticError.ErrorType.TooManyArguments);
            }

            return ValidateArguments(actualArguments, expectedArguments);
        }

        private SemanticError ValidateArguments(Node[] actual, ParameterInfo[] expected)
        {
            for (int i = 0; i < actual.Length; i++)
            {
                Node actualArg = actual[i];
                ParameterInfo expectedArg = expected[i];
                switch (actualArg)
                {
                    case CommandNode cn:
                    {
                        SemanticError error = ValidateCommand(cn);
                        if (error.Error != SemanticError.ErrorType.ErrorsNotFound)
                            return error;
                        break;
                    }
                    case ArgumentNode an:
                    {
                        Type type = expectedArg.ParameterType;
                        ITypeParser parser = TypeParsers.GetParser(type);
                        string[] arg = { an.Argument };
                        if(!parser.CanParse(arg))
                        {
                            return new SemanticError(an.Token.Start, an.Token.Length,
                                SemanticError.ErrorType.InvalidArgumentType);
                        }
                        break;
                    }
                }
            }

            return SemanticError.NoErrors;
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
    }
}
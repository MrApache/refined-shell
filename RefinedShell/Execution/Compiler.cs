using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RefinedShell.Interpreter;
using RefinedShell.Parsing;

namespace RefinedShell.Execution
{
    internal sealed class Compiler
    {
        private readonly CommandCollection _commandCollection;

        public Compiler(CommandCollection collection)
        {
            _commandCollection = collection;
        }

        public ExecutableExpression Compile(Expression expression)
        {
            if (expression.Count > 1)
            {
                int i = 0;
                ExecutableCommand[] sequence = new ExecutableCommand[expression.Count];
                foreach (CommandNode commandNode in expression)
                {
                    sequence[i++] = CompileCommand(commandNode);
                }

                return new ExecutableCommandSequence(sequence);
            }

            List<CommandNode>.Enumerator enumerator = expression.GetEnumerator();
            enumerator.MoveNext();
            CommandNode node = enumerator.Current!;
            enumerator.Dispose();
            return CompileCommand(node);
        }

        private ExecutableCommand CompileCommand(CommandNode commandNode)
        {
            ICommand command = _commandCollection[commandNode.Command];
            IArgument[] arguments = ParseArguments(command, commandNode.Arguments);
            return new ExecutableCommand(command, arguments);
        }

        private IArgument[] ParseArguments(ICommand command, Node[] argumentNodes)
        {
            IArgument[] compiledArguments = new IArgument[command.Arguments.Length];
            int start = 0;

            for (int i = 0; i < command.Arguments.Length; i++)
            {
                ParameterInfo parameter = command.Arguments[i];
                ITypeParser parser = TypeParsers.GetParser(parameter.ParameterType);

                int length = (int)parser.OptionsCount;
                ReadOnlySpan<Node> argumentSlice = argumentNodes.AsSpan(start, length);
                compiledArguments[i] = ParseArgument(parser, argumentSlice);
                start += length;
                i += length - 1;
            }

            return compiledArguments;
        }

        private IArgument ParseArgument(ITypeParser parser, ReadOnlySpan<Node> argumentNodes)
        {
            if (argumentNodes.Length == 1 && argumentNodes[0] is ArgumentNode simpleNode)
            {
                string[] arg = { simpleNode.Argument };
                return new ParsedArgument(parser.Parse(arg));
            }
            if (ContainsCommandNode(argumentNodes))
            {
                IArgument[] arguments = new IArgument[argumentNodes.Length];
                for (int i = 0; i < argumentNodes.Length; i++)
                {
                    Node node = argumentNodes[i];
                    arguments[i] = CompileArgument(node);
                }

                return new RuntimeArgument(arguments, parser);
            }
            string[] rawArgs = argumentNodes.ToArray().Select(n => ((ArgumentNode)n).Argument).ToArray();
            object parserArgument = parser.Parse(rawArgs);
            return new ParsedArgument(parserArgument);
        }

        private IArgument CompileArgument(Node node)
        {
            return node switch
            {
                CommandNode commandNode => new ExecutableInlineCommand(CompileCommand(commandNode)),
                ArgumentNode argumentNode => new UnparsedArgument(argumentNode.Argument),
                _ => throw new InvalidOperationException("Unknown node type")
            };
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RefinedShell.Commands;
using RefinedShell.Interpreter;
using RefinedShell.Parsing;
using RefinedShell.Utilities;

namespace RefinedShell.Execution
{
    internal sealed class Compiler
    {
        private readonly CommandCollection _commandCollection;
        private readonly ParserLibrary _parsers;

        private readonly Dictionary<StringToken, ExecutableExpression> _cache;
        private readonly Parser _parser;
        private readonly Semantic _analyzer;

        public Compiler(CommandCollection collection, ParserLibrary parsers, int cache = 32)
        {
            _commandCollection = collection;
            _parsers = parsers;
            _cache = new Dictionary<StringToken, ExecutableExpression>(cache);
            _parser = new Parser();
            _analyzer = new Semantic(collection, parsers);
        }

        public (Expression? expression, ProblemSegment problem) Analyze(StringToken input)
        {
            Expression? expression;
            ProblemSegment segment;
            try
            {
                expression = _parser.GetExpression(input);
                segment = _analyzer.Analyze(expression);
            }
            catch (InterpreterException e)
            {
                expression = null;
                segment = new ProblemSegment(e.Token.Start, e.Token.Length, e.Error);
            }
            return new ValueTuple<Expression?, ProblemSegment>(expression, segment);
        }

        public void ClearCache()
        {
            _cache.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ValueTuple<ExecutableExpression?, ProblemSegment> Success(ExecutableExpression expr)
        {
            return new ValueTuple<ExecutableExpression?, ProblemSegment>(expr, ProblemSegment.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ValueTuple<ExecutableExpression?, ProblemSegment> Error(ProblemSegment segment)
        {
            return new ValueTuple<ExecutableExpression?, ProblemSegment>(null, segment);
        }

        public ValueTuple<ExecutableExpression?, ProblemSegment> Compile(StringToken input)
        {
            if (_cache.TryGetValue(input, out ExecutableExpression compiledExpression)) {
                return Success(compiledExpression);
            }

            (Expression? expression, ProblemSegment error) = Analyze(input);
            if (error != ProblemSegment.None) {
                return Error(error);
            }

            if (expression!.Count == 0) // unnamed error
                return Error(ProblemSegment.None);

            compiledExpression = Compile(expression);
            _cache[input] = compiledExpression;

            return Success(compiledExpression);
        }

        private ExecutableExpression Compile(Expression expression)
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
            IArgument[] arguments = ParseArguments(command.Arguments, commandNode.Arguments);
            return new ExecutableCommand(command, arguments, commandNode.Token);
        }

        private IArgument[] ParseArguments(Arguments argsDecl, Node[] argsNode)
        {
            IArgument[] compiledArguments = new IArgument[argsDecl.Count];
            int start = 0;
            for (int i = 0; i < argsDecl.Count; i++) {
                bool skip = false;
                Argument arg = argsDecl[i];
                ITypeParser parser = _parsers.GetParser(arg.Type);
                IEnumerator<ArgumentInfo> argInfo = parser.GetArgumentInfo();
                while (argInfo.MoveNext() && !skip) {
                    ArgumentInfo current = argInfo.Current;
                    int length = (int)current.ElementCount;
                    if (arg.IsOptional && start + length > argsNode.Length) {
                        compiledArguments[i] = new ParsedArgument(Type.Missing);
                        skip = true;
                        continue;
                    }

                    Span<Node> slice = argsNode.AsSpan(start, length);
                    compiledArguments[i] = ParseArgument(parser, slice);
                    start += length;
                    break;
                }
            }

            return compiledArguments;
        }

        /*private IArgument[] ParseArguments(ICommand command, Node[] argumentNodes)
        {
            IArgument[] compiledArguments = new IArgument[command.Arguments.Count];
            int start = 0;

            for (int i = 0; i < command.Arguments.Count; i++)
            {
                Argument argument = command.Arguments[i];
                ITypeParser parser = _parsers.GetParser(argument.Type);
                if (argument.IsOptional)
                {
                    if(i >= argumentNodes.Length)
                    {
                        compiledArguments[i] = new ParsedArgument(Type.Missing);
                        continue;
                    }
                }

                int length = (int)parser.ArgumentCount;
                ReadOnlySpan<Node> argumentSlice = argumentNodes.AsSpan(start, length);
                compiledArguments[i] = ParseArgument(parser, argumentSlice);
                start += length;
                i += length - 1;
            }

            return compiledArguments;
        }*/

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
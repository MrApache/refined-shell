using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<StringToken, Expression> _cache;
        private readonly Parser _parser;
        private readonly Semantic _analyzer;

        private IReadOnlyCommand[] _usedCommands;
        private uint _position;

        public Compiler(CommandCollection collection, ParserLibrary parsers, int cache = 32)
        {
            _commandCollection = collection;
            _parsers = parsers;
            _cache = new Dictionary<StringToken, Expression>(cache);
            _parser = new Parser();
            _analyzer = new Semantic(collection, parsers);
            _usedCommands = Array.Empty<IReadOnlyCommand>();
        }

        public void ClearCache()
        {
            _cache.Clear();
        }

        public InternalResult<Node> Analyze(StringToken input)
        {
            Node? expression;
            InternalResult<Node> analysis;
            try {
                expression = _parser.GetExpression(input);
                analysis = _analyzer.Analyze(expression);
            }
            catch (InterpreterException e) {
                expression = null;
                analysis = new InternalResult<Node>(null, new ProblemSegment(e.Token.Start, e.Token.Length, e.Error), 0);
            }
            return new InternalResult<Node>(expression, analysis.Segment, analysis.CommandsCount);
        }

        public InternalResult<Expression> Compile(StringToken input)
        {
            if (_cache.TryGetValue(input, out Expression expression)) {
                return new InternalResult<Expression>(expression, ProblemSegment.None, (uint)expression.Commands.Length);
            }

            InternalResult<Node> result = Analyze(input);
            if (result.Segment != ProblemSegment.None) {
                return new InternalResult<Expression>(result.Segment);
            }

            _usedCommands = new IReadOnlyCommand[result.CommandsCount];

            ExecutableExpression executableExpression = CompileNode(result.Expression!);
            expression = new Expression(executableExpression, _usedCommands);
            _usedCommands = Array.Empty<IReadOnlyCommand>();
            _position = 0;
            _cache[input] = expression;

            return new InternalResult<Expression>(expression, ProblemSegment.None, (uint)expression.Commands.Length);
        }

        private ExecutableExpression CompileNode(Node node)
        {
            return node switch
            {
                CommandNode cn => CompileCommand(cn),
                LogicalNode ln => CompileLogicalExpression(ln),
                SequenceNode sq => CompileSequence(sq),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        internal void CompileNode_TestCompatible(Node node, uint size)
        {
            _usedCommands = new IReadOnlyCommand[size];
            CompileNode(node);
            _usedCommands = Array.Empty<IReadOnlyCommand>();
            _position = 0;
        }

        private ExecutableExpression CompileLogicalExpression(LogicalNode logicalNode)
        {
            ExecutableCommand first = CompileCommand(logicalNode.First);
            ExecutableExpression second = CompileNode(logicalNode.Second);
            return logicalNode.Type switch
            {
                LogicalNode.LogicalNodeType.And => new CommandAND(first, second),
                LogicalNode.LogicalNodeType.Or => new CommandOR(first, second),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private ExecutableSequence CompileSequence(SequenceNode sequenceNode)
        {
            ExecutableCommand first = CompileCommand(sequenceNode.First);
            ExecutableExpression second = CompileNode(sequenceNode.Second);
            return new ExecutableSequence(first, second);
        }

        private ExecutableCommand CompileCommand(CommandNode commandNode)
        {
            ICommand command = _commandCollection[commandNode.Command];
            _usedCommands[_position++] = command;
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
                IEnumerator<ArgumentInfo> argInfo = parser.GetArgumentInfo(); //Possible memory allocations
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

        private IArgument ParseArgument(ITypeParser parser, ReadOnlySpan<Node> argumentNodes)
        {
            if (argumentNodes.Length == 1 && argumentNodes[0] is ArgumentNode simpleNode) {
                string[] arg = { simpleNode.Argument };
                return new ParsedArgument(parser.Parse(arg));
            }
            if (ContainsCommandNode(argumentNodes)) {
                IArgument[] arguments = new IArgument[argumentNodes.Length];
                for (int i = 0; i < argumentNodes.Length; i++) {
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
            return node switch {
                CommandNode commandNode => new ExecutableInlineCommand(CompileCommand(commandNode)),
                ArgumentNode argumentNode => new UnparsedArgument(argumentNode.Argument),
                _ => throw new InvalidOperationException("Unknown node type")
            };
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

using BenchmarkDotNet.Attributes;
using RefinedShell.Execution;
using RefinedShell.Interpreter;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class StepByStepPerformance
{
    private readonly Parser _parser = new Parser();
    private readonly Shell _shell = new Shell();
    private readonly Semantic _semantic;
    private readonly Compiler _compiler;
    private readonly Lexer _lexer;
    private readonly Node _expression =
        new CommandNode(new Token(0, 3, TokenType.Identifier), "set",
        [
            new ArgumentNode(new Token(4, 5, TokenType.Identifier), "irisu"),
            new ArgumentNode(new Token(10, 9, TokenType.Number), "572923412")
        ]);

    public StepByStepPerformance()
    {
        _shell.RegisterAllWithAttribute<CommandExecution>(null);
        _semantic = new Semantic(_shell.GetCommandCollection(), ParserLibrary.Default);
        _compiler = new Compiler(_shell.GetCommandCollection(), ParserLibrary.Default);
        _lexer = new Lexer();
    }

    [ShellFunction("set")]
    private static void SetValue(string name, int value) { }

    [Benchmark]
    public void Lexer()
    {
        // ReSharper disable once NotAccessedVariable
        int i = 0;
        _lexer.SetInputString("set irisu 572923412");
        while (_lexer.GetNextToken().Type != TokenType.EndOfLine) {
            i++;
        }
    }

    [Benchmark]
    public void Parser()
    {
        _parser.GetExpression("set irisu 572923412");
    }

    [Benchmark]
    public void Semantic()
    {
        _semantic.Analyze(_expression);
    }

    [Benchmark]
    public void Compiler()
    {
        _compiler.CompileNode_TestCompatible(_expression, 1);
    }

    //Before optimizations
    //| Method   | Mean       | Error    | StdDev   | Gen0   | Allocated |
    //|--------- |-----------:|---------:|---------:|-------:|----------:|
    //| Lexer    |   827.8 ns | 16.41 ns | 23.01 ns | 0.1984 |    1248 B |
    //| Parser   | 1,113.5 ns |  7.26 ns |  6.07 ns | 0.2995 |    1888 B |
    //| Semantic |   128.5 ns |  2.49 ns |  2.44 ns | 0.0305 |     192 B |
    //| Compiler |   132.0 ns |  0.66 ns |  0.55 ns | 0.0572 |     360 B |

    //With semantic memory optimizations #1
    //Replace array allocations with ArrayPool<string>.Shared
    //| Method   | Mean       | Error    | StdDev   | Gen0   | Allocated |
    //|--------- |-----------:|---------:|---------:|-------:|----------:|
    //| Lexer    |   839.9 ns | 14.50 ns | 12.85 ns | 0.1984 |    1248 B |
    //| Parser   | 1,148.4 ns |  4.47 ns |  3.96 ns | 0.2995 |    1888 B |
    //| Semantic |   235.3 ns |  1.09 ns |  0.96 ns | 0.0100 |      64 B |
    //| Compiler |   136.1 ns |  0.70 ns |  0.54 ns | 0.0572 |     360 B |

    //With semantic memory optimizations #2
    //Reduced ArrayPool<string>.Shared.Rent calls from 2 to 1 #2
    //| Method   | Mean       | Error    | StdDev   | Median     | Gen0   | Allocated |
    //|--------- |-----------:|---------:|---------:|-----------:|-------:|----------:|
    //| Lexer    |   862.5 ns | 16.95 ns | 26.39 ns |   852.0 ns | 0.1984 |    1248 B |
    //| Parser   | 1,194.7 ns | 23.88 ns | 50.38 ns | 1,168.8 ns | 0.2995 |    1888 B |
    //| Semantic |   166.5 ns |  2.80 ns |  2.34 ns |   165.5 ns | 0.0100 |      64 B |
    //| Compiler |   137.9 ns |  1.40 ns |  1.31 ns |   137.9 ns | 0.0572 |     360 B |

    //Used custom regex
    //| Method   | Mean     | Error   | StdDev  | Gen0   | Allocated |
    //|--------- |---------:|--------:|--------:|-------:|----------:|
    //| Lexer    | 382.9 ns | 3.26 ns | 2.89 ns |      - |         - |
    //| Parser   | 641.1 ns | 6.07 ns | 5.38 ns | 0.0687 |     432 B |
    //| Semantic | 158.1 ns | 0.40 ns | 0.31 ns | 0.0100 |      64 B |
    //| Compiler | 136.5 ns | 0.58 ns | 0.51 ns | 0.0572 |     360 B |
}
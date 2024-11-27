using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class CommandExecution
{
    private readonly Shell _shell;
    private readonly string[] _input;

    public CommandExecution()
    {
        _shell = new Shell();
        _shell.RegisterAllWithAttribute<CommandExecution>(null);
        _input = new string[10000];
        for (int i = 0; i < 10000; i++) {
            _input[i] = $"set irisu {Random.Shared.Next()}";
        }
    }

    [ShellFunction("set")]
    private static void SetValue(string name, int value) { }

    [Benchmark]
    public void ExecuteWithCaching()
    {
        _shell.Execute("set irisu 993");
    }

    [Benchmark]
    public void ExecuteWithoutCaching()
    {
        string input = _input[Random.Shared.Next(0, 10000)];
        _shell.Execute(input);
    }

    // Before
    // | Method                | Mean        | Error       | StdDev      | Gen0   | Gen1   | Allocated |
    // |---------------------- |------------:|------------:|------------:|-------:|-------:|----------:|
    // | ExecuteWithCaching    |    108.0 ns |     0.38 ns |     0.32 ns |      - |      - |         - |
    // | ExecuteWithoutCaching | 10,207.5 ns | 1,138.09 ns | 3,355.70 ns | 0.4883 | 0.1221 |    3098 B |
    
    // After
    //| Method                | Mean        | Error       | StdDev       | Median      | Gen0   | Gen1   | Allocated |
    //|---------------------- |------------:|------------:|-------------:|------------:|-------:|-------:|----------:|
    //| ExecuteWithCaching    |    105.3 ns |     0.48 ns |      0.40 ns |    105.3 ns |      - |      - |         - |
    //| ExecuteWithoutCaching | 20,518.3 ns | 3,405.19 ns | 10,040.28 ns | 17,853.1 ns | 0.3891 | 0.0992 |    2445 B |

    //After fix 1
    //| Method                | Mean     | Error   | StdDev  | Allocated |
    //|---------------------- |---------:|--------:|--------:|----------:|
    //| ExecuteWithCaching    | 106.2 ns | 0.85 ns | 0.71 ns |         - |
    //| ExecuteWithoutCaching | 239.8 ns | 3.96 ns | 3.31 ns |         - |

    //After fix 2
    //| Method                | Mean     | Error   | StdDev  | Allocated |
    //|---------------------- |---------:|--------:|--------:|----------:|
    //| ExecuteWithCaching    | 104.0 ns | 0.79 ns | 0.70 ns |         - |
    //| ExecuteWithoutCaching | 235.6 ns | 1.87 ns | 1.57 ns |         - |
}

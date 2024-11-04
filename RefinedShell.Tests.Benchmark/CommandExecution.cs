using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class CommandExecution
{
    private readonly Shell _shell;

    public CommandExecution()
    {
        _shell = new Shell();
        _shell.RegisterAll<CommandExecution>(null);
    }

    [ShellCommand("set")]
    private static void SetValue(string name, int value) { }

    [Benchmark]
    public void ExecuteWithCaching()
    {
        _shell.Execute("set irisu 993");
    }

    [Benchmark]
    public void ExecuteWithoutCaching()
    {
        _shell.ExecuteWithoutCache("set irisu 993");
    }
}

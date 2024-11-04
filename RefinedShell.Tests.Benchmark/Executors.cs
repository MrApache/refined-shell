using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

public class Executors
{
    private readonly Shell _safe;
    private readonly Shell _unsafe;

    public Executors()
    {
        _safe = new Shell();
        _safe.RegisterAll<CommandExecution>(null);

        _unsafe = new Shell(false);
        _unsafe.RegisterAll<CommandExecution>(null);
    }

    [ShellCommand("set")]
    private static void SetValue(string name, int value) { }

    [Benchmark]
    public void ExecuteWithSafeExecutor()
    {
        _safe.Execute("set irisu 993");
    }

    [Benchmark]
    public void ExecuteWithUnsafeExecutor()
    {
        _unsafe.Execute("set irisu 993");
    }
}
using BenchmarkDotNet.Attributes;
using RefinedShell.Commands;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class ShellMethods
{
    private readonly Shell _shell;

    public ShellMethods()
    {
        _shell = new Shell();
        _shell.RegisterAllWithAttribute<ShellMethods>(null);
        _shell.RegisterAllWithAttribute<CommandCollection>(null);
    }

    [ShellCommand("set")]
    private static void SetValue(string name, int value) { }

    //[Benchmark]
    public void GetAllCommands()
    {
        _shell.GetAllCommands();
    }

    //[Benchmark]
    public void GetAllCommands_Func()
    {
        _shell.GetCommands(_ => true);
    }

    //[Benchmark]
    public void GetCommandsThatStartsWith()
    {
        IEnumerable<ICommand> result = _shell.GetAllCommands().Where(c => c.Name.StartsWith("set"));
    }

    [Benchmark]
    public void GetCommandsThatStartsWith_Func()
    {
        for (int i = 0; i < 10000; i++)
        {
            IEnumerable<ICommand> result = _shell.GetCommands(c => c.Name.StartsWith("set"));
        }
    }
}
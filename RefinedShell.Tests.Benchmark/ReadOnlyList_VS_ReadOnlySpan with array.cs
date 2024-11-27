using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using RefinedShell.Commands;
// ReSharper disable NotAccessedVariable

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class ReadOnlyList_VS_ReadOnlySpan_with_array
{
    [Benchmark]
    public void List()
    {
        List<IReadOnlyCommand> commands = new List<IReadOnlyCommand>(Count);
        for (int i = 0; i < commands.Count; i++) {
            commands[i] = null!;
        }
        SendList(commands.AsReadOnly());
    }

    [Params(0, 10, 100)]
    public int Count;

    private readonly IReadOnlyCommand[] _commands0 = [];
    private readonly IReadOnlyCommand[] _commands10 = new IReadOnlyCommand[10];
    private readonly IReadOnlyCommand[] _commands100 = new IReadOnlyCommand[100];

    [Benchmark]
    public void Span()
    {
        IReadOnlyCommand[] collection = Count switch
        {
            0 => _commands0,
            10 => _commands10,
            _ => _commands100
        };

        ReadOnlySpan<IReadOnlyCommand> commands = new ReadOnlySpan<IReadOnlyCommand>(collection);
        SendSpan(commands);
    }

    private void SendList(ReadOnlyCollection<IReadOnlyCommand> commands)
    {
        int count = 0;
        foreach (IReadOnlyCommand command in commands) {
            count += 1 * 2;
        }
    }

    private void SendSpan(ReadOnlySpan<IReadOnlyCommand> commands)
    {
        int count = 0;
        foreach (IReadOnlyCommand command in commands) {
            count += 1 * 2;
        }
    }
}
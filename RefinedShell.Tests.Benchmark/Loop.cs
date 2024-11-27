using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

public class Loop
{
    [Benchmark]
    public void For()
    {
        int i = 0;
        for (;;) {
            i++;
            if(i == 100000000)
                break;
        }
        // ReSharper disable once FunctionNeverReturns
    }

    [Benchmark]
    public void While()
    {
        int i = 0;
        while(true) {
            i++;
            if(i == 100000000)
                break;
        }
        // ReSharper disable once FunctionNeverReturns
    }

    [Benchmark]
    public void Goto()
    {
        int i = 0;
        Start:
        i++;
        if (i == 100000000)
            return;
        goto Start;
    }
}
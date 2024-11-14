using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class Yield
{
    [Benchmark]
    public void GetEnumerator()
    {
        for (int i = 0; i < 2; i++) {
            IEnumerator<int> enumerator = List();
        }
    }

    public IEnumerator<int> List()
    {
        yield return 2;
        yield return 2;
    }
}
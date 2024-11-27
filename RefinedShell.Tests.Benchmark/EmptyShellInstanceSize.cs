using BenchmarkDotNet.Attributes;
using System.Text;
using System.Text.RegularExpressions;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class EmptyShellInstanceSize
{
    [Benchmark]
    public void ShellInstance()
    {
        Shell shell = new Shell();
    }

    [Benchmark]
    public void RegexInstance()
    {
        Regex regex = new Regex(string.Empty);
    }

    [Benchmark]
    public void StringBuilder()
    {
        StringBuilder builder = new StringBuilder();
    }
}

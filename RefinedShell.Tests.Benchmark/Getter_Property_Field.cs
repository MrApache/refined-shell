using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

public class Getter_Property_Field
{
    private readonly Test _test = new Test();
    private string value;

    [Benchmark]
    public void ReadField()
    {
        value = _test.Field;
    }

    [Benchmark]
    public void ReadProperty()
    {
        value = _test.Property;
    }

    private sealed class Test
    {
        public readonly string Field = "String";
        public string Property => Field;
    }
}